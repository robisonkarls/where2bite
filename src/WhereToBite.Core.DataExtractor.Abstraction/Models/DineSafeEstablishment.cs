using System.Collections.Generic;
using System.Xml.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [XmlRoot(ElementName="ESTABLISHMENT")]
    public sealed class DineSafeEstablishment
    {
        [XmlElement(ElementName="ID")]
        public int Id { get; set; }
        [XmlElement(ElementName="NAME")]
        public string Name { get; set; } = default!;
        [XmlElement(ElementName="TYPE")]
        public string Type { get; set; } = default!;
        [XmlElement(ElementName="ADDRESS")]
        public string Address { get; set; } = default!;
        [XmlElement(ElementName="LATITUDE")]
        public string Latitude { get; set; } = default!;
        [XmlElement(ElementName="LONGITUDE")]
        public string Longitude { get; set; } = default!;
        [XmlElement(ElementName="STATUS")]
        public string Status { get; set; } = default!;
        [XmlElement(ElementName="INSPECTION")]
        public IReadOnlyList<DineSafeInspection>? Inspections { get; set; }
    }
}