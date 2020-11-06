using System;
using System.Text.Json.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction
{
    [Serializable]
    public class DineSafeLastUpdate
    {
        [JsonPropertyName("lastUpdate")] 
        public string? LastUpdate { get; set; }
    }
}