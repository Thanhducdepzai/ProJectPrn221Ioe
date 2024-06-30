using System;
using System.Collections.Generic;

namespace ProjectIoePrn.Models
{
    public partial class Answer
    {
        public int AnwserId { get; set; }
        public string AnswerText { get; set; } = null!;
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
