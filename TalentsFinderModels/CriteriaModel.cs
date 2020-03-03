using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models.TalentsFinderModels
{
    public class CriteriaModel
    {
        [Key]
        public int Id { get; set; }

        public int YearsOfExperienceIndex { get; set; }

        public int YearsOfExperience { get; set; }

        public virtual List<TecNamesModel> TecNamesModel { get; set; }
    }

    public class TecNamesModel
    {
        [Key]
        public int Id { get; set; }

        public string TecName { get; set; }

        public virtual CriteriaModel CriteriaModel { get; set; }
    }

}
