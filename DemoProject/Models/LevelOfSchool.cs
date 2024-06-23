using System;
using System.Collections.Generic;

namespace DemoProject.Models
{
    public partial class LevelOfSchool
    {
        public LevelOfSchool()
        {
            Grades = new HashSet<Grade>();
            Schools = new HashSet<School>();
        }

        public int LevelSchoolId { get; set; }
        public string? LevelName { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<School> Schools { get; set; }
    }
}
