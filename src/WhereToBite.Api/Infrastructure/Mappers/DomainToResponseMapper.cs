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
                    NumberOfInspections = establishment.Inspections.Count,
                    Longitude = establishment.Location.X,
                    Latitude = establishment.Location.Y,
                    LastInspection = mappedLastInspection,
                    OverallAmountFined = establishment.Inspections.Select(x => x.Infractions.Sum(infraction => infraction.AmountFined)).Sum(),
                    OverallNumberOfInfractions = establishment.Inspections.Sum(x => x.Infractions.Count)
                };
            }).ToList();
        }

        public IReadOnlyList<InspectionResponse> MapInspectionResponses(IEnumerable<Inspection> inspections)
        {
            return inspections.Select(inspection =>
            {
                return new InspectionResponse
                {
                    Date = inspection.Date,
                    Status = inspection.InspectionStatus.Name,
                    Infractions = inspection.Infractions.Select(infraction => new InfractionResponse
                    {
                        ConvictionDate = infraction.ConvictionDate,
                        Severity = infraction.Severity.Name,
                        AmountFined = infraction.AmountFined,
                        CourtOutcome = infraction.CourtOutcome
                    })
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