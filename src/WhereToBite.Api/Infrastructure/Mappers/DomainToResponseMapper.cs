using System.Linq;
using System.Collections.Generic;
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
                var mappedLastInspection = MapLastInspection(establishment.GetLastInspection());

                return new EstablishmentResponse
                {
                    Address = establishment.Address,
                    Name = establishment.Name,
                    Status = establishment.EstablishmentStatus.Name,
                    Type = establishment.Type,
                    DineSafeId = establishment.DineSafeId,
                    NumberOfInspection = establishment.Inspections.Count,
                    Longitude = establishment.Location.X,
                    Latitude = establishment.Location.Y,
                    LastInspection = mappedLastInspection
                };
            }).ToList();
        }

        private static LastInspection MapLastInspection(Inspection inspection)
        {
            if (inspection == null)
            {
                return LastInspection.Empty;
            }

            return new LastInspection
            {
                Date = inspection.Date,
                Status = inspection.InspectionStatus.Name,
                NumberOfInfractions = inspection.Infractions.Count
            };
        }
    }
}