using System;
using System.Threading;
using System.Threading.Tasks;
using WhereToBite.Core.DataExtractor.Abstraction.Models;

namespace WhereToBite.Core.DataExtractor.Abstraction
{
    public interface IDineSafeClient : IDisposable
    {
        Task<DineSafeMetadata> GetMetadataAsync(CancellationToken cancellationToken);

        Task<DineSafeData> GetEstablishmentsAsync(Uri resourceUri, CancellationToken cancellationToken);
    }
}