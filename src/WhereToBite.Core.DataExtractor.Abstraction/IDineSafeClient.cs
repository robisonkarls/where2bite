using System;
using System.Threading;
using System.Threading.Tasks;
using WhereToBite.Core.DataExtractor.Abstraction.Models;

namespace WhereToBite.Core.DataExtractor.Abstraction
{
    public interface IDineSafeClient
    {
        Task<DineSafeMetadata> GetMetadataAsync(CancellationToken cancellationToken);

        Task<DineSafeEstablishment> GetEstablishmentsAsync(Uri resourceUri, CancellationToken cancellationToken);
    }
}