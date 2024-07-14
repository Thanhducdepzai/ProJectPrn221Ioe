using System;
using System.Collections.Generic;

<<<<<<< HEAD
namespace ProjectIoePrn.Models
=======
namespace DemoProject.Models
>>>>>>> Register
{
    public partial class Part
    {
        public Part()
        {
            PresentPartResultDetails = new HashSet<PresentPartResultDetail>();
            Questions = new HashSet<Question>();
        }

        public int PartId { get; set; }
        public string PartName { get; set; } = null!;
        public int PartOrder { get; set; }
        public DateTime PartCreateDate { get; set; }
        public DateTime PartUpdateDate { get; set; }
        public int TypeOfQuestionId { get; set; }
        public int RoundId { get; set; }

        public virtual Round Round { get; set; } = null!;
        public virtual TypeOfQuestion TypeOfQuestion { get; set; } = null!;
        public virtual ICollection<PresentPartResultDetail> PresentPartResultDetails { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
