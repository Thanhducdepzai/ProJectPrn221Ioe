using System;
using System.Collections.Generic;

namespace DemoProject.Models
{
    public partial class Student
    {
        public Student()
        {
            IndividualResultDetails = new HashSet<IndividualResultDetail>();
        }

        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentGmail { get; set; } = null!;
        public DateTime? StudentDob { get; set; }
        public string? StudentUsername { get; set; } = null!;
        public string StudentPassword { get; set; } = null!;
        public string? StudentClass { get; set; } = null!;
        public int? SchoolId { get; set; }
        public int? GradeId { get; set; }

        public virtual Grade Grade { get; set; } = null!;
        public virtual School School { get; set; } = null!;
        public virtual ICollection<IndividualResultDetail> IndividualResultDetails { get; set; }
    }
}
