using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

                var metadataStream = await GetAsync(_metadataUri, cancellationToken);

                if (metadataStream == null)
                {
                    return null;
                }

                try
                {
                    return await 
                        JsonSerializer.DeserializeAsync<DineSafeMetadata>(metadataStream, null, cancellationToken);
                }
                catch (JsonException exception)
                {
                    _logger.LogError($"Error: {exception}");
                    throw;
                }
            }
        }

        public async Task<DineSafeData> GetEstablishmentsAsync([NotNull] Uri resourceUri, CancellationToken cancellationToken)
        {
            if (resourceUri == null)
            {
                throw new ArgumentNullException(nameof(resourceUri));
            }

            using (_logger.BeginScope("Started Establishments Request"))
            {
                _logger.LogInformation($"URL: {resourceUri.Host}");

                var establishmentsStream = await GetAsync(resourceUri, cancellationToken);

                if (establishmentsStream == null)
                {
                    return null;
                }
                
                var serializer = new XmlSerializer(typeof(DineSafeData));

                return (DineSafeData) serializer.Deserialize(establishmentsStream);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<Stream> GetAsync([NotNull] Uri uri, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started Request");

            try
            {
                var responseMessage = await _httpClient.GetAsync(uri, cancellationToken);

                if (responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Request completed");
                    return await responseMessage.Content.ReadAsStreamAsync();
                }
                
                _logger.LogError($"Error requesting for {uri.Host}");
                _logger.LogError($"Failed: {responseMessage.ReasonPhrase}");
                
                return default;
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError($"Failed: {exception.Message}");
                throw;
            }
        }
    }
}