using System;

namespace WhereToBite.Api.Model
{
    [Serializable]
    public class InfractionResponse
    {
        public string Severity { get; set; }
        public DateTime ConvictionDate { get; set; }
        public string CourtOutcome { get; set; }
        public decimal AmountFined { get; set; }
        public string Action { get; set; }
    }
}