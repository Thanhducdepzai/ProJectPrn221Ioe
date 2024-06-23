using System;
using System.Collections.Generic;

namespace DemoProject.Models
{
    public partial class TypeOfQuestion
    {
        public TypeOfQuestion()
        {
            Parts = new HashSet<Part>();
        }

        public int TypeOfQuestion1 { get; set; }
        public string TypeOfQuestionName { get; set; } = null!;

        public virtual ICollection<Part> Parts { get; set; }
    }
}
