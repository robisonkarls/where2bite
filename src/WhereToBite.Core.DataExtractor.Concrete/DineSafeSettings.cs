using System;

namespace WhereToBite.Core.DataExtractor.Concrete
{
    public class DineSafeSettings
    {
        public const string DineSafe = "DineSafe";
        public string MetadataUrl { get; set; }
        public Guid DineSafeId { get; set; }
        public string DineSafeLastUpdateUrl { get; set; }
    }
}