using System;
using System.Collections.Generic;

namespace ProjectIoePrn.Models
{
    public partial class PresentPartResultDetail
    {
        public int PartResultDetailId { get; set; }
        public int Score { get; set; }
        public int CompleteTime { get; set; }
        public int PartId { get; set; }
        public int IndividualResultId { get; set; }

        public virtual IndividualResultDetail IndividualResult { get; set; } = null!;
        public virtual Part Part { get; set; } = null!;
    }
}
