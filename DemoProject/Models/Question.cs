using System;
using System.Collections.Generic;
namespace ProjectIoePrn.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionMetadata { get; set; } = null!;
        public int PartId { get; set; }

        public virtual Part Part { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
