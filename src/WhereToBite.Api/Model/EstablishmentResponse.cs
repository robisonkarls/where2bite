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
        public string Status { get; set; }
        public string Type { get; set; }
        public int NumberOfInspection { get; set; }
        public LastInspection LastInspection { get; set; }
    }

    [Serializable]
    public class LastInspection
    {
        public DateTime Date { get; set; }
        public int NumberOfInfractions { get; set; }
        public string Status { get; set; }
    }
}