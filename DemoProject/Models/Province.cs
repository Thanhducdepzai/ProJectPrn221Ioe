using System;
using System.Collections.Generic;
namespace ProjectIoePrn.Models
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<District>();
        }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; } = null!;

        public virtual ICollection<District> Districts { get; set; }
    }
}
