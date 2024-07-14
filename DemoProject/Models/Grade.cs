using System;
using System.Collections.Generic;

<<<<<<< HEAD
namespace ProjectIoePrn.Models
=======
namespace DemoProject.Models
>>>>>>> Register
{
    public partial class Grade
    {
        public Grade()
        {
            Rounds = new HashSet<Round>();
            Students = new HashSet<Student>();
        }

        public int GradeId { get; set; }
        public string GradeName { get; set; } = null!;
        public int LevelSchoolId { get; set; }

        public virtual LevelOfSchool LevelSchool { get; set; } = null!;
        public virtual ICollection<Round> Rounds { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
