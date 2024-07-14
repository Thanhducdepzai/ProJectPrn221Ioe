using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIoePrn.Models;

namespace ProjectIoePrn.Pages
{
    public class SynomymModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public List<Question> ListQuestions { get; set; }
        public List<String> data ;

        public SynomymModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ListQuestions =  _context.Questions.Where(q => q.QuestionId >= 1 && q.QuestionId <= 8).ToList();
            //foreach (Question q in ListQuestions)
            //{
            //    data.Add(q.QuestionMetadata);
            //    data.Add(q.QuestionText);
            //}

            ViewData["data"] = data;
            ViewData["list"] = ListQuestions;
        }
    }
}
