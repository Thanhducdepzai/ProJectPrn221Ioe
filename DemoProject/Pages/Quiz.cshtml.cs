using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;

namespace ProjectIoePrn.Pages
{
    public class QuizModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public List<Question> ListQuestions { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PartId { get; set; }

        public QuizModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {

            //ListQuestions = _context.Questions.Where(q => q.QuestionId >= 25 && q.QuestionId <= 26).ToList();
            //foreach (Question q in ListQuestions)
            //{
            //    data.Add(q.QuestionMetadata);
            //    data.Add(q.QuestionText);
            //}

            // ViewData["data"] = data;

            ListQuestions = await _context.Questions
                .Where(q => q.PartId == PartId)
                .OrderBy(q => EF.Functions.Random())
                .Take(10)
                .ToListAsync();
            ViewData["list"] = ListQuestions;
        }

        public async Task<IActionResult> OnPostAsync(int finalScore, int timeSpent)
        {
            
            

            var partResultDetail = await _context.PresentPartResultDetails
                .FirstOrDefaultAsync(p => p.PartId == PartId );

            if (partResultDetail != null)
            {
                partResultDetail.Score = finalScore;
                partResultDetail.CompleteTime = timeSpent;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/OverviewExams");
        }
    }
}
