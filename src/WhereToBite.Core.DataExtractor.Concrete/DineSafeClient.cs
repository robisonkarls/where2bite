using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Abstraction.Models;

namespace WhereToBite.Core.DataExtractor.Concrete
{
    internal sealed class DineSafeClient : IDineSafeClient, IDisposable
    {
        private readonly Uri _metadataUri;
        private readonly HttpClient _httpClient;
        private readonly ILogger<DineSafeClient> _logger;

        public DineSafeClient(
            [NotNull]IOptions<DineSafeSettings> dineSafeSettings, 
            [NotNull]HttpClient httpClient, 
            [NotNull]ILogger<DineSafeClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _metadataUri = new Uri(dineSafeSettings.Value.MetadataUrl);
        }
        
        public async Task<DineSafeMetadata> GetMetadataAsync(CancellationToken cancellationToken)
        {
            using (_logger.BeginScope("Started Metadata Request"))
            {
                _logger.LogInformation($"URL: {_metadataUri.Host} ");
                return await GetAsync<DineSafeMetadata>(_metadataUri, cancellationToken);
            }
        }

        public Task<DineSafeEstablishment> GetEstablishmentsAsync([NotNull] Uri resourceUri, CancellationToken cancellationToken)
        {
            if (resourceUri == null)
            {
                throw new ArgumentNullException(nameof(resourceUri));
            }
            
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken)
        {
            using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _logger.LogInformation("Started Request");

            Stream responseStream;
            
            try
            {
                var responseMessage = await _httpClient.GetAsync(uri, tokenSource.Token);
                
                if (!responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error requesting for {uri.Host}");
                    _logger.LogError($"Request Failed: {responseMessage.ReasonPhrase}");
                    return default;
                }
                
                responseStream = await responseMessage.Content.ReadAsStreamAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                throw;
            }

            try
            {
                _logger.LogInformation("Request completed");
                return await JsonSerializer.DeserializeAsync<T>(responseStream, null, tokenSource.Token);
            }
            catch (JsonException e)
            {
                _logger.LogError($"Error: {e.Message}");
                throw;
            }
        }
    }
}