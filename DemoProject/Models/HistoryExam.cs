using System;
using System.Collections.Generic;

namespace DemoProject.Models
{
    public partial class HistoryExam
    {
        public int HistoryExamId { get; set; }
        public int PartResultDetailId { get; set; }
        public string ListQuestionId { get; set; } = null!;
        public string ListUserAnswer { get; set; } = null!;

        public virtual PresentPartResultDetail PartResultDetail { get; set; } = null!;
    }
}
