using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WhereToBite.Api;
using WhereToBite.Api.ServiceWorker;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Abstraction.Models;
using WhereToBite.Core.DataExtractor.Concrete;
using WhereToBite.Infrastructure;
using WhereToBite.Infrastructure.Repositories;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Api.ServiceWorker
{
    public class DineSafeDataExtractorTests
    {
        private readonly DineSafeDataExtractor _dineSafeDataExtractor;
        private readonly WhereToBiteContext _whereToBiteContext;

        public DineSafeDataExtractorTests()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            _whereToBiteContext = new WhereToBiteContext(options);
            
            _dineSafeDataExtractor = CreateDineSafeDataExtractor();
        }

        [Fact]
        public void ShouldCreateInstance()
        {
            Assert.NotNull(_dineSafeDataExtractor);
        }

        [Fact]
        public void ShouldBeInstanceOfIDineSafeDataExtractor()
        {
            Assert.IsAssignableFrom<IDineSafeDataExtractor>(_dineSafeDataExtractor);
        }

        [Fact]
        public void ShouldDoNothingWhenThereIsNoUpdate()
        {
            // act
            _dineSafeDataExtractor.Extract(new object());
            
            // assert
            Assert.Empty(_whereToBiteContext.Establishments);
        }

        [Fact]
        public void ShouldUpdateWithNewRecordWhenThereIsUpdate()
        {
            // arrange
            var dineSafeDataExtractor = CreateDineSafeDataExtractor(true);
            
            // act
            dineSafeDataExtractor.Extract(new object());

            _whereToBiteContext.SaveChangesAsync();
            
            var establishments = _whereToBiteContext.Establishments.ToList();
            
            // assert
            Assert.Single(establishments);
        }

        private DineSafeDataExtractor CreateDineSafeDataExtractor(bool haveUpdate = false)
        {
            var repository = new EstablishmentRepository(_whereToBiteContext);
            var dineSafeDataExtractorLogger = NullLogger<DineSafeDataExtractor>.Instance;
            var dineSafeSettings = Options.Create(new DineSafeSettings
            {
                MetadataUrl = "http://localhost/metadataHost",
                DineSafeId = Guid.Empty,
                DineSafeLastUpdateUrl = "http://localhost/lastUpdateHost"
            });
            var packageId = Guid.NewGuid();
            var client = new FakeDineSafeClient(packageId);
            
            var memoryCache = new FakeMemoryCache(haveUpdate ? DateTime.Now.AddDays(-2) : DateTime.Now);
            
            var actual =
                new DineSafeDataExtractor(repository, dineSafeSettings, dineSafeDataExtractorLogger, client, memoryCache);
            return actual;
        }
    }
    
    internal sealed class FakeMemoryCache : IMemoryCache
    {
        private readonly Dictionary<string, object> _dictionary;
        
        public FakeMemoryCache(DateTime date)
        {
            _dictionary = new Dictionary<string, object> 
            {
                {
                    CacheKeys.LastModifiedDate, date
                }
            };
        }

        public void Dispose() { }

        public bool TryGetValue(object key, out object value)
        {
            return _dictionary.TryGetValue(key.ToString()!, out value);
        }

        public ICacheEntry CreateEntry(object key)
        {
            return new FakeEntry();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeEntry : ICacheEntry
    {
        public void Dispose() {}

        public object Key { get; }
        public object Value { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public IList<IChangeToken> ExpirationTokens { get; }
        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }
        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }
    }

    internal sealed class FakeDineSafeClient : IDineSafeClient
    {
        private readonly Guid _packageId;

        public FakeDineSafeClient(Guid packageId)
        {
            _packageId = packageId;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<DineSafeMetadata> GetMetadataAsync(CancellationToken cancellationToken)
        {
            var metadata = new DineSafeMetadata
            {
                Result = new DineSafeResult
                {
                    Resources = new List<DineSafeResource> {new DineSafeResource
                    {
                        PackageId = _packageId,
                        Url = "http://localhost/fake"
                    }}
                }
            };
            
            return Task.FromResult(metadata);
        }

        public Task<DineSafeData> GetEstablishmentsAsync(Uri resourceUri, CancellationToken cancellationToken)
        {
            var dineSafeData = new DineSafeData
            {
                Establishments = new[]
                {
                    new DineSafeEstablishment
                    {
                        Name = "fakeName", 
                        Status = "Pass", 
                        Address = "Some Address",
                        Id = 1,
                        Type = "O+",
                        Longitude = "0001",
                        Latitude = "0001"
                    }
                }
            };

            return Task.FromResult(dineSafeData);
        }

        public Task<DineSafeLastUpdate> GetLastUpdateAsync(CancellationToken cancellationToken)
        {
            var lastUpdate = DateTime.Today.AddDays(-1);
            
            return Task.FromResult(new DineSafeLastUpdate {LastUpdate = lastUpdate});
        }
    }
}