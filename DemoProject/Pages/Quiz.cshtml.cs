using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoProject.Models;

namespace DemoProject.Pages
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
            ListQuestions = await _context.Questions
            .Where(q => q.PartId == PartId)
            .OrderBy(q => Guid.NewGuid())
            .Take(5)
            .ToListAsync();
            ViewData["list"] = ListQuestions;
        }

        public async Task<IActionResult> OnPostAsync(int finalScore, int timeSpent, string History,string QuestionIds)
        {
            var partResultDetail = await _context.PresentPartResultDetails
                .FirstOrDefaultAsync(p => p.PartId == PartId);

            if (partResultDetail != null)
            {
                partResultDetail.Score = finalScore;
                partResultDetail.CompleteTime = timeSpent;
                await _context.SaveChangesAsync();

                var historyExam = new HistoryExam
                {
                    PartResultDetailId = partResultDetail.PartResultDetailId,
                    ListQuestionId = QuestionIds,
                    ListUserAnswer = History
                };

                _context.HistoryExams.Add(historyExam);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/OverviewExams");
        }
    }
}
