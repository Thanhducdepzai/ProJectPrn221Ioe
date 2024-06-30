using System;
using System.Collections.Generic;

namespace ProjectIoePrn.Models
{
    public partial class IndividualResultDetail
    {
        public IndividualResultDetail()
        {
            PresentPartResultDetails = new HashSet<PresentPartResultDetail>();
        }

        public int IndividualResultId { get; set; }
        public int RoundScore { get; set; }
        public int CompleteTime { get; set; }
        public int UserId { get; set; }
        public int RoundId { get; set; }

        public virtual Round Round { get; set; } = null!;
        public virtual Student User { get; set; } = null!;
        public virtual ICollection<PresentPartResultDetail> PresentPartResultDetails { get; set; }
    }
}
