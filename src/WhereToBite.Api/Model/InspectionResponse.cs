using System;
using System.Collections.Generic;

namespace WhereToBite.Api.Model
{
    [Serializable]
    public class InspectionResponse
    {
        public string InspectionStatus { get; set; }
        public DateTime InspectionDate { get; set; }
        public IEnumerable<InfractionResponse> Infractions { get; set; }
    }
}