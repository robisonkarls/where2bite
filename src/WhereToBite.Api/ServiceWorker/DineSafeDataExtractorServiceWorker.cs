using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Abstraction.Models;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.ServiceWorker
{
    public class DineSafeDataExtractorServiceWorker : IHostedService, IDisposable
    {
        private readonly IDineSafeClient _dineSafeClient;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IOptions<DineSafeSettings> _dineSafeSettings;
        private readonly ILogger<DineSafeDataExtractorServiceWorker> _logger;

        public DineSafeDataExtractorServiceWorker(
            IDineSafeClient dineSafeClient, 
            IEstablishmentRepository establishmentRepository, 
            IOptions<DineSafeSettings> dineSafeSettings,
            ILogger<DineSafeDataExtractorServiceWorker> logger)
        {
            _dineSafeSettings  = dineSafeSettings ?? throw new ArgumentNullException(nameof(dineSafeSettings));
            _dineSafeClient = dineSafeClient ?? throw new ArgumentNullException(nameof(dineSafeClient));
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _dineSafeSettings = dineSafeSettings;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var dineSafeMetadata = await _dineSafeClient.GetMetadataAsync(cancellationToken);

            var url = GetDineSafeUrl(dineSafeMetadata);
            
            var dineSafeData = await _dineSafeClient.GetEstablishmentsAsync(url!, cancellationToken);

            if (dineSafeData.Establishments != null)
            {
                foreach (var dineSafeEstablishment in dineSafeData.Establishments)
                {
                    var establishment = new Establishment(
                        dineSafeEstablishment.Id,
                        dineSafeEstablishment.Name,
                        dineSafeEstablishment.Type,
                        dineSafeEstablishment.Address,
                        dineSafeEstablishment.Longitude,
                        dineSafeEstablishment.Latitude,
                        dineSafeEstablishment.Status);
                    
                    await _establishmentRepository.AddIfNotExistsAsync(establishment, cancellationToken);
                }
            }
        }

        private Uri GetDineSafeUrl(DineSafeMetadata dineSafeMetadata)
        {
            return dineSafeMetadata.Result.Resources
                .Where(r => r.PackageId == Guid.Parse(_dineSafeSettings.Value.DineSafeId))
                .Select(r => new Uri(r.Url))
                .FirstOrDefault();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}