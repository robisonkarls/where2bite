using System.Collections.Generic;
using System.Linq;
using WhereToBite.Api.Model;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.Infrastructure.Mappers
{
    public class DomainToResponseMapper : IDomainMapper
    {
        public IReadOnlyList<EstablishmentResponse> MapEstablishmentResponses(IEnumerable<Establishment> establishments)
        {
            return establishments.Select(establishment =>
            {
                var lastInspection = establishment.GetLastInspection();
            
                return new EstablishmentResponse
                {
                    Address = establishment.Address,
                    Name = establishment.Name,
                    Status = establishment.EstablishmentStatus.Name,
                    Type = establishment.Type,
                    DineSafeId = establishment.DineSafeId,
                    NumberOfInspection = establishment.Inspections.Count,
                    LastInspection = new LastInspection
                    {
                        Date = lastInspection.Date,
                        Status = lastInspection.InspectionStatus.Name,
                        NumberOfInfractions = lastInspection.Infractions.Count
                    }
                };
            }).ToList();
        }
    }
}