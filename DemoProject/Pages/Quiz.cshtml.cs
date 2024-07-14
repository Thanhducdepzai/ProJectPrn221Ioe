using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIoePrn.Models;

namespace ProjectIoePrn.Pages
{
    public class QuizModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public List<Question> ListQuestions { get; set; }
        

        public QuizModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ListQuestions = _context.Questions.Where(q => q.QuestionId >= 25 && q.QuestionId <= 26).ToList();
            //foreach (Question q in ListQuestions)
            //{
            //    data.Add(q.QuestionMetadata);
            //    data.Add(q.QuestionText);
            //}

            // ViewData["data"] = data;
            ViewData["list"] = ListQuestions;
        }

        public IActionResult OnPost(int FinalScore, int TimeSpent)
        {
            

            return Page();
        }
    }
}
