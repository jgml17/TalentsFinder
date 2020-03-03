using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Core.Models.TalentsFinderModels
{
    public partial class TalentModel
    {
        [Key]
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("age")]
        public long Age { get; set; }

        [JsonPropertyName("name")]
        public virtual Name Name { get; set; }

        [JsonPropertyName("picture")]
        public Uri Picture { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("fullResume")]
        public string FullResume { get; set; }

        [JsonPropertyName("technologies")]
        public virtual List<Technology> Technologies { get; set; }
    }

    public partial class Name
    {
        [Key]
        public string Id { get; set; }

        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }

        public string TalentModelId { get; set; }

        [NotMapped]
        public virtual TalentModel TalentModel { get; set; }
    }

    public partial class Technology
    {
        [Key]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("experianceYears")]
        public long ExperianceYears { get; set; }

        [NotMapped]
        public virtual TalentModel Talents { get; set; }
    }
}
