using DemoProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class HistoryExamModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public HistoryExamModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        public List<Part> Exams { get; set; }
        public List<HistoryExam> HistoryExams { get; set; }
        public List<Question> Questions { get; set; }

        public int RoundId { get; set; }

        public async Task OnGetAsync(int roundId)
        {
            RoundId = roundId;

            // L?y danh sách các ph?n bài thi c?a vòng thi
            Exams = await _context.Parts
                .Where(x => x.RoundId == RoundId)
                .ToListAsync();

            // L?y l?ch s? bài thi liên k?t v?i PartResultDetail và Part
            HistoryExams = await _context.HistoryExams
                .Include(h => h.PartResultDetail)
                .ThenInclude(prd => prd.Part)
                .Where(h => h.PartResultDetail.Part.RoundId == RoundId)
                .ToListAsync();

            // L?y danh sách câu h?i t? database
            Questions = await _context.Questions.ToListAsync();
        }
    }
}
