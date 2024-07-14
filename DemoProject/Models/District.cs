using System;
using System.Collections.Generic;

namespace ProjectIoePrn.Models
{
    public partial class District
    {
        public District()
        {
            Schools = new HashSet<School>();
        }

        public int DistricId { get; set; }
        public string DistricName { get; set; } = null!;
        public int ProvinceId { get; set; }

        public virtual Province Province { get; set; } = null!;
        public virtual ICollection<School> Schools { get; set; }
    }
}
