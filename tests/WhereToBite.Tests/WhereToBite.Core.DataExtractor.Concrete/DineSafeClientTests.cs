using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Concrete;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Core.DataExtractor.Concrete
{
    public class DineSafeClientTests : IDisposable
    {
        private readonly DineSafeClient _client;

        public DineSafeClientTests()
        {
            var payloadFileLocation = Path.Combine(Directory.GetCurrentDirectory(), "Setup", "metadata.json");
            
            var httpClient = new HttpClient(new DineMockHttpMessageHandler(HttpStatusCode.OK, payloadFileLocation));
            
            _client = CreateDineSafeClient("http://localhost", httpClient);
        }

        private static DineSafeClient CreateDineSafeClient(string host, HttpClient httpClient)
        {
            var logger = NullLogger<DineSafeClient>.Instance;

            var dineSafeSettings = Options.Create(new DineSafeSettings {MetadataUrl = host});

            return new DineSafeClient(
                dineSafeSettings,
                httpClient,
                logger);
        }

        [Fact]
        public void ShouldInstantiate()
        {
            Assert.NotNull(_client);
        }

        [Fact]
        public void ShouldImplementDineSafeClientInterface()
        {
            Assert.IsAssignableFrom<IDineSafeClient>(_client);
        }

        [Fact]
        public async Task ShouldGetMetadata()
        {
            var actual = await _client.GetMetadataAsync(CancellationToken.None);

            Assert.NotNull(actual);
            Assert.NotNull(actual.Result);
            Assert.True(actual.Success);
        }

        [Fact]
        public async Task ShouldReturnDefaultValueInCaseOfBadResponse()
        {
            var payloadFileLocation = Path.Combine(Directory.GetCurrentDirectory(), "Setup", "metadata.json");
            var httpClient = new HttpClient(new DineMockHttpMessageHandler(HttpStatusCode.InternalServerError, payloadFileLocation));
            
            var client = CreateDineSafeClient("http://localhost", httpClient);

            var actual = await client.GetMetadataAsync(CancellationToken.None);
            
            Assert.Null(actual);
        }

        [Fact]
        public async Task ShouldThrowIfResourceUrlIsNull()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() =>
                _client.GetEstablishmentsAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldGetEstablishments()
        {
            var payloadFileLocation = Path.Combine(Directory.GetCurrentDirectory(), "Setup", "establishments_data.xml");
            var httpClient = new HttpClient(new DineMockHttpMessageHandler(HttpStatusCode.OK, payloadFileLocation));
            
            var client = CreateDineSafeClient("http://localhost", httpClient);

            var actual = await client.GetEstablishmentsAsync(new Uri("http://localhost"), CancellationToken.None);
            
            Assert.NotNull(actual);
            Assert.Equal(3, actual.Establishments.ToArray().Length);
        }

        [Fact]
        public async Task ShouldReturnNullInCaseServerError()
        {
            var payloadFileLocation = Path.Combine(Directory.GetCurrentDirectory(), "Setup", "establishments_data.xml");
            var httpClient = new HttpClient(new DineMockHttpMessageHandler(HttpStatusCode.InternalServerError, payloadFileLocation));
            
            var client = CreateDineSafeClient("http://localhost", httpClient);

            var actual = await client.GetEstablishmentsAsync(new Uri("http://localhost"), CancellationToken.None);
            
            Assert.Null(actual);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }

    internal sealed class DineMockHttpMessageHandler : HttpMessageHandler
    {
        private HttpStatusCode HttpStatusCode { get; }
        private string PayloadFileLocation { get; }
        public DineMockHttpMessageHandler(HttpStatusCode httpStatusCode, string payloadFileLocation)
        {
            HttpStatusCode = httpStatusCode;
            PayloadFileLocation = payloadFileLocation;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var fileStream = File.OpenRead(PayloadFileLocation);
            
            var memoryStream = new MemoryStream();

            fileStream.CopyTo(memoryStream);

            memoryStream.Position = 0;

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode,
                Content = new StreamContent(memoryStream)
            });
        }
    }
}