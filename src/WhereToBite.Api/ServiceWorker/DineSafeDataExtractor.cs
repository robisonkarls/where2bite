﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Abstraction.Models;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.ServiceWorker
{
    internal sealed class DineSafeDataExtractor : IDineSafeDataExtractor
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IOptions<DineSafeSettings> _dineSafeSettings;
        private readonly TimeSpan _httpRequestTimeOut = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _databaseOperationTimeOut = TimeSpan.FromMinutes(30);
        private readonly ILogger<DineSafeDataExtractor> _logger;
        private readonly IDineSafeClient _dineSafeClient;

        
        public DineSafeDataExtractor(
            IEstablishmentRepository establishmentRepository, 
            IOptions<DineSafeSettings> dineSafeSettings, 
            ILogger<DineSafeDataExtractor> logger, 
            IDineSafeClient dineSafeClient)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _dineSafeSettings = dineSafeSettings ?? throw new ArgumentNullException(nameof(dineSafeSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dineSafeClient = dineSafeClient ?? throw new ArgumentNullException(nameof(dineSafeClient));
        }

        public async void Extract(object info)
        {
            var dineSafeData = await DownloadDineSafeDataAsync();
            
            await PersistDineSafeDataAsync(dineSafeData);
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
                        var establishment = CreateEstablishment(dineSafeEstablishment);

                        var storedEstablishment =
                            await _establishmentRepository.AddIfNotExistsAsync(establishment, databaseCts.Token);

                        storedEstablishment.AddNewInspections(ExtractInspections(dineSafeEstablishment));
                    }
                }
            }
        }

        private async Task<DineSafeData> DownloadDineSafeDataAsync()
        {
            using var cts = new CancellationTokenSource(_httpRequestTimeOut);
            
            using (_logger.BeginScope("Starting DineSafe Data Download"))
            {
                var dineSafeMetadata = await _dineSafeClient.GetMetadataAsync(cts.Token);

                var url = GetDineSafeUrl(dineSafeMetadata);

                return await _dineSafeClient.GetEstablishmentsAsync(url!, cts.Token);
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
                        DateTime.Parse(x._ConvictionDate),
                        x.CourtOutcome,
                        decimal.TryParse(x.AmountFined, out var parsedAmountFined)
                            ? parsedAmountFined
                            : 0));
            }

            return Enumerable.Empty<Infraction>();
        }
        
        
        

        

        private static Establishment CreateEstablishment(DineSafeEstablishment dineSafeEstablishment)
        {
            return new Establishment(
                dineSafeEstablishment.Id,
                dineSafeEstablishment.Name,
                dineSafeEstablishment.Type,
                dineSafeEstablishment.Address,
                dineSafeEstablishment.Longitude,
                dineSafeEstablishment.Latitude,
                dineSafeEstablishment.Status);
        }

        private Uri GetDineSafeUrl(DineSafeMetadata dineSafeMetadata)
        {
            return dineSafeMetadata.Result.Resources
                .Where(r => r.PackageId == Guid.Parse(_dineSafeSettings.Value.DineSafeId))
                .Select(r => new Uri(r.Url))
                .FirstOrDefault();
        }

        public void Dispose()
        {
            _dineSafeClient.Dispose();
        }
    }
}