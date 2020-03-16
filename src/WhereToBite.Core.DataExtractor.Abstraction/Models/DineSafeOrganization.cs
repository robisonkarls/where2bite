using System;
using System.Text.Json.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    public sealed class DineSafeOrganization
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
        [JsonPropertyName("created")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
        [JsonPropertyName("is_organization")]
        public bool IsOrganization { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; } = default!;
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = default!;
        [JsonPropertyName("revision_id")]
        public Guid RevisionId { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("approval_status")]
        public string ApprovalStatus { get; set; } = default!;
    }
}