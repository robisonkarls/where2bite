using System;
using System.Text.Json.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    public sealed class DineSafeMetadata
    {
        [JsonPropertyName("help")] 
        public string Help { get; set; } = default!;

        [JsonPropertyName("success")] 
        public bool Success { get; set; } = default!;

        [JsonPropertyName("result")] 
        public DineSafeResult Result { get; set; } = default!;
    }
}