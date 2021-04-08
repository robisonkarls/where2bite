using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WhereToBite.Api.ServiceWorker
{
    internal sealed class DineSafeDataExtractorServiceWorker : IHostedService, IDisposable
    {
        private readonly ILogger<DineSafeDataExtractorServiceWorker> _logger;
        private readonly IDineSafeDataExtractor _dineSafeDataExtractor;
        private Timer _timer;

        public DineSafeDataExtractorServiceWorker(
            ILogger<DineSafeDataExtractorServiceWorker> logger, 
            IDineSafeDataExtractor dineSafeDataExtractor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dineSafeDataExtractor = dineSafeDataExtractor ?? throw new ArgumentNullException(nameof(dineSafeDataExtractor));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed hosted service running");
            
            _timer = new Timer(_dineSafeDataExtractor.Extract, null, TimeSpan.Zero, TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Finished importing DineSafe Database at {DateTime.Now}");
            _timer.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}