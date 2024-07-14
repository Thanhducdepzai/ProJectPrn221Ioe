using System;
using System.Collections.Generic;

namespace ProjectIoePrn.Models
{
    public partial class Round
    {
        public Round()
        {
            IndividualResultDetails = new HashSet<IndividualResultDetail>();
            Parts = new HashSet<Part>();
        }

        public int RoundId { get; set; }
        public string RoundName { get; set; } = null!;
        public DateTime RoundCreateDate { get; set; }
        public DateTime RoundUpdateDate { get; set; }
        public int GradeId { get; set; }
        public string isPublic { get; set; }

        public virtual Grade Grade { get; set; } = null!;
        public virtual ICollection<IndividualResultDetail> IndividualResultDetails { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
    }
}
