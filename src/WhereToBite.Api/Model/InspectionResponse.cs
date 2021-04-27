using System;
using System.Collections.Generic;

namespace WhereToBite.Api.Model
{
    [Serializable]
    public class InspectionResponse
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<InfractionResponse> Infractions { get; set; }
    }
}