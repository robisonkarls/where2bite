using System.Collections.Generic;
using WhereToBite.Api.Model;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.Infrastructure.Mappers
{
    public interface IDomainMapper
    {
        IReadOnlyList<EstablishmentResponse> MapEstablishmentResponses(IEnumerable<Establishment> establishments);
    }
}