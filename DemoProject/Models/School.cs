using System;
using System.Collections.Generic;

namespace DemoProject.Models
{
    public partial class School
    {
        public School()
        {
            Students = new HashSet<Student>();
        }

        public int SchoolId { get; set; }
        public string SchoolName { get; set; } = null!;
        public int LevelSchoolId { get; set; }
        public int DistrictId { get; set; }

        public virtual District District { get; set; } = null!;
        public virtual LevelOfSchool LevelSchool { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; }
    }
}
