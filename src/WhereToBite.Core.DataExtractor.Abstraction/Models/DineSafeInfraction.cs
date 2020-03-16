using System;
using System.Xml.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    [XmlRoot(ElementName="INFRACTION")]
    public sealed class DineSafeInfraction
    {
        [XmlElement(ElementName="SEVERITY")]
        public string Severity { get; set; } = default!;
        [XmlElement(ElementName="ACTION")]
        public string Action { get; set; } = default!;
        [XmlElement(ElementName="CONVICTION_DATE")]
        public DateTime ConvictionDate { get; set; }
        [XmlElement(ElementName="COURT_OUTCOME")]
        public string CourtOutcome { get; set; } = default!;
        [XmlElement(ElementName="AMOUNT_FINED")]
        public string AmountFined { get; set; } = default!;
    }
}