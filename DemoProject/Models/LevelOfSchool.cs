using System;
using System.Collections.Generic;

<<<<<<< HEAD
namespace ProjectIoePrn.Models
=======
namespace DemoProject.Models
>>>>>>> Register
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
