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
        [XmlElement(ElementName = "CONVICTION_DATE")]
        // ReSharper disable once InconsistentNaming
        public string _ConvictionDate { get; set; } = default!;

        [XmlElement(ElementName="COURT_OUTCOME")]
        public string CourtOutcome { get; set; } = default!;
        [XmlElement(ElementName="AMOUNT_FINED")]
        public string AmountFined { get; set; } = default!;

        [XmlIgnore]
        private DateTime? ConvictionDate
        {
            get
            {
                if (!string.IsNullOrEmpty(_ConvictionDate))
                {
                    return DateTime.Parse(_ConvictionDate);
                }

                return null;
            }
        }
    }
}