namespace ProjectIoePrn.Models
{
    public class RoundViewModel
    {
        public List<QuestionPart1ViewModel> QuestionsPart1 { get; set; } = new List<QuestionPart1ViewModel>();
        public List<QuestionPart2ViewModel> QuestionsPart2 { get; set; } = new List<QuestionPart2ViewModel>();
        public List<QuestionPart3ViewModel> QuestionsPart3 { get; set; } = new List<QuestionPart3ViewModel>();
        public bool IsPublic { get; set; }
    }

    public class QuestionPart1ViewModel
    {
        public string Name { get; set; }
        public string Answer { get; set; }
    }

    public class QuestionPart2ViewModel
    {
        public string Name { get; set; }
        public string Choice1 { get; set; }
        public string Choice2 { get; set; }
        public string Choice3 { get; set; }
        public string Choice4 { get; set; }
        public string Correct { get; set; }
    }

    public class QuestionPart3ViewModel
    {
        public string Vietnamese { get; set; }
        public string English { get; set; }
    }

}