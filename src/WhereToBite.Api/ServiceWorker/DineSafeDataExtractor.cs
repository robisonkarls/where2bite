using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using WhereToBite.Core.DataExtractor.Concrete;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Abstraction.Models;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.ServiceWorker
{
    [UsedImplicitly]
    internal sealed class DineSafeDataExtractor : IDineSafeDataExtractor
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IOptions<DineSafeSettings> _dineSafeSettings;
        private readonly TimeSpan _httpRequestTimeOut = TimeSpan.FromSeconds(180);
        private readonly TimeSpan _databaseOperationTimeOut = TimeSpan.FromHours(1);
        private readonly ILogger<DineSafeDataExtractor> _logger;
        private readonly IDineSafeClient _dineSafeClient;
        private readonly IMemoryCache _memoryCache;
        private DateTime _lastUpdate;
        
        public DineSafeDataExtractor(
            IEstablishmentRepository establishmentRepository, 
            IOptions<DineSafeSettings> dineSafeSettings, 
            ILogger<DineSafeDataExtractor> logger, 
            IDineSafeClient dineSafeClient, 
            IMemoryCache memoryCache)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _dineSafeSettings = dineSafeSettings ?? throw new ArgumentNullException(nameof(dineSafeSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dineSafeClient = dineSafeClient ?? throw new ArgumentNullException(nameof(dineSafeClient));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async void Extract(object info)
        {
            DineSafeData dineSafeData = null;
            
            using (var cts = new CancellationTokenSource(_httpRequestTimeOut))
            {
                if (await IsDineSafeOutdatedAsync(cts.Token))
                {

                    using (_logger.BeginScope($"DineSafe update found on {DateTime.Now}"))
                    {
                        dineSafeData = await DownloadDineSafeDataAsync(cts.Token);
                    }
                }
                else
                {
                    _logger.LogInformation($"No update at this time. {DateTime.Now}");   
                }
            }

            if (dineSafeData != null)
            {
                await PersistDineSafeDataAsync(dineSafeData);
            }
            else
            {
                _logger.LogInformation("No DineSafe Data to process");
            }
        }

        private async Task<bool> IsDineSafeOutdatedAsync(CancellationToken cancellationToken)
        {
            var lastUpdate = await _dineSafeClient.GetLastUpdateAsync(cancellationToken);

            if (_memoryCache.TryGetValue(CacheKeys.LastModifiedDate, out DateTime cachedLastModifiedDate))
            {
                _lastUpdate = cachedLastModifiedDate;
            }

            if (_lastUpdate < lastUpdate)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(24))
                    .SetSize(1024);

                _lastUpdate = _memoryCache.Set(
                    CacheKeys.LastModifiedDate,
                    lastUpdate,
                    cacheEntryOptions);

                _logger.LogInformation($"Found update for DineSafe on {_lastUpdate}");
            }
            else
            {
                _logger.LogInformation($"No updates found for DineSafe");
                return false;
            }
            return true;
        }

        private async Task PersistDineSafeDataAsync(DineSafeData dineSafeData)
        {
            using var databaseCts =  new CancellationTokenSource(_databaseOperationTimeOut);
            
            using (_logger.BeginScope("Begin DineSafe Data Collection"))
            {
                if (dineSafeData.Establishments != null)
                {
                    foreach (var dineSafeEstablishment in dineSafeData.Establishments)
                    {
                        _logger.LogInformation($"Started processing establishment: {dineSafeEstablishment.Name}");
                        
                        var establishment = CreateEstablishment(dineSafeEstablishment);

                        var storedEstablishment =
                            await _establishmentRepository.AddIfNotExistsAsync(establishment, databaseCts.Token);

                        storedEstablishment.AddNewInspections(ExtractInspections(dineSafeEstablishment));

                        await _establishmentRepository.UnitOfWork.SaveEntitiesAsync(databaseCts.Token);
                        
                        _logger.LogInformation($"Finished processing establishment: {establishment.Name}");
                    }
                }
            }
        }

        private async Task<DineSafeData> DownloadDineSafeDataAsync(CancellationToken cancellationToken)
        {
            using (_logger.BeginScope("Starting DineSafe Data Download"))
            {
                var dineSafeMetadata = await _dineSafeClient.GetMetadataAsync(cancellationToken);

                var url = GetDineSafeUrl(dineSafeMetadata);

                return await _dineSafeClient.GetEstablishmentsAsync(url!, cancellationToken);
            }
        }

        private static IEnumerable<Inspection> ExtractInspections(DineSafeEstablishment dineSafeEstablishment)
        {
            if (dineSafeEstablishment.Inspections == null)
            {
                return Enumerable.Empty<Inspection>();
            }
            
            var inspections = new List<Inspection>();
            
            foreach (var dineSafeInspection in dineSafeEstablishment.Inspections)
            {
                var inspection = new Inspection(dineSafeInspection.Status, dineSafeInspection.Date);

                inspection.AddNewInfractions(ExtractInfractions(dineSafeInspection));

                inspections.Add(inspection);
            }
            
            return inspections;
        }

        private static IEnumerable<Infraction> ExtractInfractions(DineSafeInspection dineSafeInspection)
        {
            if (dineSafeInspection.Infractions != null)
            {
                return dineSafeInspection.Infractions.Select(x =>
                    new Infraction(
                        x.Severity,
                        x.Action,
                        string.IsNullOrEmpty(x._ConvictionDate) 
                            ? DateTime.MinValue
                            : DateTime.Parse(x._ConvictionDate),
                        x.CourtOutcome,
                        decimal.TryParse(x.AmountFined, out var parsedAmountFined)
                            ? parsedAmountFined
                            : 0));
            }

            return Enumerable.Empty<Infraction>();
        }
        
        private static Establishment CreateEstablishment(DineSafeEstablishment dineSafeEstablishment)
        {
            return new(
                dineSafeEstablishment.Id,
                dineSafeEstablishment.Name,
                dineSafeEstablishment.Type,
                dineSafeEstablishment.Address,
                dineSafeEstablishment.Status,
                new Point(double.Parse(dineSafeEstablishment.Longitude), double.Parse(dineSafeEstablishment.Latitude)));
        }

        private Uri GetDineSafeUrl(DineSafeMetadata dineSafeMetadata)
        {
            return dineSafeMetadata.Result.Resources
                .Where(r => r.PackageId == _dineSafeSettings.Value.DineSafeId)
                .Select(r => new Uri(r.Url))
                .FirstOrDefault();
        }

        public void Dispose()
        {
            _dineSafeClient.Dispose();
        }
    }
}