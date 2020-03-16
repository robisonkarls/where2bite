using System;
using System.Text.Json.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    public sealed class DineSafeResource
    {
        [JsonPropertyName("cache_last_updated")]
        public object CacheLastUpdated { get; set; } = default!;
        [JsonPropertyName("package_id")]
        public Guid PackageId { get; set; }
        [JsonPropertyName("datastore_active")]
        public bool DataStoreActive { get; set; }
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("size")]
        public long? Size { get; set; }
        [JsonPropertyName("format")]
        public string Format { get; set; } = default!;
        [JsonPropertyName("state")]
        public string State { get; set; } = default!;
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = default!;
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
        [JsonPropertyName("is_preview")]
        public bool IsPreview { get; set; }
        [JsonPropertyName("last_modified")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("url_type")]
        public string UrlType { get; set; } = default!;
        [JsonPropertyName("mimetype")]
        public string Mimetype { get; set; } = default!;
        [JsonPropertyName("cache_url")]
        public string CacheUrl { get; set; } = default!;
        [JsonPropertyName("extract_job")]
        public string ExtractJob { get; set; } = default!;
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
        [JsonPropertyName("created")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = default!;
        [JsonPropertyName("mimetype_inner")]
        public string MimetypeInner { get; set; } = default!;
        [JsonPropertyName("position")]
        public int Position { get; set; } = default!;
        [JsonPropertyName("revision_id")]
        public string RevisionId { get; set; } = default!;
        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; } = default!;
    }
}