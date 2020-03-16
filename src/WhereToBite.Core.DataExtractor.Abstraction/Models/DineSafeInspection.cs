using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    [XmlRoot(ElementName="INSPECTION")]
    public sealed class DineSafeInspection
    {
        [XmlElement(ElementName = "STATUS")] 
        public string Status { get; set; } = default!;
        [XmlElement(ElementName="DATE")]
        public DateTime Date { get; set; } = default!;
        [XmlElement(ElementName="INFRACTION")]
        public DineSafeInfraction[]? Infractions { get; set; }
    }
}