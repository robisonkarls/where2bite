using System;
using System.Text.Json.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    public sealed class DineSafeTag
    {
        [JsonPropertyName("vocabulary_id")]
        public Guid? VocabularyId { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; } = default!;
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = default!;
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
    }
}