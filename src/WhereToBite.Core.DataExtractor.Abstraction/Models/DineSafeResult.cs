using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhereToBite.Core.DataExtractor.Abstraction.Models
{
    [Serializable]
    public class DineSafeResult
    {
        [JsonPropertyName("license_title")]
        public string LicenseTitle { get; set; } = default!;
        [JsonPropertyName("owner_unit")]
        public string OwnerUnit { get; set; } = default!;
        [JsonPropertyName("topics")]
        public string Topics { get; set; } = default!;
        [JsonPropertyName("owner_email")]
        public string OwnerEmail { get; set; } = default!;
        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; } = default!;
        [JsonPropertyName("private")]
        public bool @Private { get; set; }
        [JsonPropertyName("owner_division")]
        public string OwnerDivision { get; set; } = default!;
        [JsonPropertyName("num_tags")]
        public int TagsCount { get; set; }
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("metadata_created")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("refresh_rate")]
        public string RefreshRate { get; set; } = default!;
        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;
        [JsonPropertyName("license_url")]
        public string LicenseUrl { get; set; } = default!;
        [JsonPropertyName("state")]
        public string State { get; set; } = default!;
        [JsonPropertyName("information_url")]
        public string InformationUrl { get; set; } = default!;
        [JsonPropertyName("license_id")]
        public string LicenseId { get; set; } = default!;
        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;
        [JsonPropertyName("resources")]
        public IReadOnlyList<DineSafeResource> Resources { get; set; } = default!;
        [JsonPropertyName("limitations")]
        public string Limitations { get; set; } = default!;
        [JsonPropertyName("num_resources")]
        public int ResourcesCount { get; set; }
        [JsonPropertyName("collection_method")]
        public string CollectionMethod { get; set; } = default!;
        [JsonPropertyName("tags")]
        public IReadOnlyList<DineSafeTag> Tags { get; set; } = default!;
        [JsonPropertyName("is_retired")]
        public bool IsRetired { get; set; } 
        [JsonPropertyName("groups")]
        public IReadOnlyList<string> Groups { get; set; } = default!;
        [JsonPropertyName("creator_user_id")]
        public string CreatorUserId { get; set; } = default!;
        [JsonPropertyName("dataset_category")]
        public string DatasetCategory { get; set; } = default!;
        [JsonPropertyName("relationships_as_subject")]
        public IReadOnlyList<string> RelationshipsAsSubject { get; set; } = default!;
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
        [JsonPropertyName("metadata_modified")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("isopen")]
        public bool IsOpen { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = default!;
        [JsonPropertyName("notes")]
        public string Notes { get; set; } = default!;
        [JsonPropertyName("owner_org")]
        public string OrganizationOwner { get; set; } = default!;
        [JsonPropertyName("last_refreshed")]
        public DateTime? LastRefreshed { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = default!;
        [JsonPropertyName("formats")]
        public string Formats { get; set; } = default!;
        [JsonPropertyName("owner_section")]
        public string OwnerSection { get; set; } = default!;
        [JsonPropertyName("organization")]
        public DineSafeOrganization Organization { get; set; } = default!;
        [JsonPropertyName("revision_id")]
        public string RevisionId { get; set; } = default!;
        [JsonPropertyName("civic_issues")]
        public string CivicIssues { get; set; } = default!;
    }
}