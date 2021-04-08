using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    [XmlRoot("DINESAFE_DATA", IsNullable = false)]
    public sealed class DineSafeData
    {
        [XmlElement(ElementName="ESTABLISHMENT", IsNullable = false)]
        public DineSafeEstablishment[]? Establishments { get; set; }
    }
}