using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models.TalentsFinderModels
{
    public class StatusModel
    {
        [Key]
        public int Id { get; set; }

        public string TalentId { get; set; }

        public bool Seen { get; set; }

        public bool Accepted { get; set; }
    }
}
