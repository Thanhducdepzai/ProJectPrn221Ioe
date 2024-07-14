using System;
using System.Collections.Generic;

<<<<<<< HEAD
namespace ProjectIoePrn.Models
=======
namespace DemoProject.Models
>>>>>>> Register
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
