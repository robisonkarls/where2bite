using System;

namespace WhereToBite.Api.Model
{
    [Serializable]
    public class EstablishmentResponse
    {
        public int DineSafeId { get; init; }
        public string Name { get; init; }
        public string Address { get; init; }
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public string Status { get; init; }
        public string Type { get; init; }
        public int NumberOfInspections { get; init; }
        public LastInspection LastInspection { get; init; }
    }

    [Serializable]
    public class LastInspection
    {
        public DateTime Date { get; init; }
        public int NumberOfInfractions { get; init; }
        public string Status { get; init; }

        public static LastInspection Empty => new()
        {
            Status = "Empty"
        };
    }
}