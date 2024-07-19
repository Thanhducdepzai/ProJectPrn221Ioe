using System;
using System.Collections.Generic;

namespace DemoProject.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Rounds = new HashSet<Round>();
        }

        public int AdminId { get; set; }
        public string AdminName { get; set; } = null!;
        public string AdminGmail { get; set; } = null!;
        public DateTime AdminDob { get; set; }
        public string AdminUsername { get; set; } = null!;
        public string AdminPassword { get; set; } = null!;

        public virtual ICollection<Round> Rounds { get; set; }
    }
}
