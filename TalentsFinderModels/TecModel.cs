using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Core.Models.TalentsFinderModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class TecModel
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("votes")]
        public long Votes { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("fans")]
        public long Fans { get; set; }

        [JsonPropertyName("logo")]
        public Uri Logo { get; set; }

        [JsonPropertyName("stacks")]
        public long Stacks { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public string LogoString { get => Logo.AbsoluteUri; }

        public int YearsOfExperienceIndex { get; set; }

        public bool ItemSelected { get; set; }

        public int YearsOfExperience { 

            get {

                int value = 1; // Default 1 year

                switch (YearsOfExperienceIndex)
                {
                    case 1:
                        // 5 years
                        value = 5;
                        break;

                    case 2:
                        // 10 years
                        value = 10;
                        break;
                }

                return value;
            } 
        }
    }
}
