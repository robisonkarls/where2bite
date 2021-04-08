using System;

namespace WhereToBite.Api.Model
{
    [Serializable]
    public class NearbyRequest
    {
        public double Radius { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}