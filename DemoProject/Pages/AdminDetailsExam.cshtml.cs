using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class AdminDetailsExamModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        private readonly ILogger<AdminDetailsExamModel> _logger;

        public AdminDetailsExamModel(IOE_Project_Clone_PRN221Context context, ILogger<AdminDetailsExamModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public DateTime RoundCreateDate { get; set; } = DateTime.Now;
        public List<QuestionInputModel> QuestionsPart1 { get; set; } = new List<QuestionInputModel>();
        public List<QuestionInputModel> QuestionsPart2 { get; set; } = new List<QuestionInputModel>();
        public List<QuestionInputModel> QuestionsPart3 { get; set; } = new List<QuestionInputModel>();
        public int AdminId { get; set; }
        public Admin Admin { get; set; }

        public async Task<IActionResult> OnGetAsync(int? roundId, int? adminId)
        {
            if (roundId == null || adminId == null)
            {
                return NotFound();
            }

            RoundId = roundId.Value;
            AdminId = adminId.Value;

            await LoadAdminDataAsync(AdminId);
            await LoadRoundDataAsync(RoundId);

            return Page();
        }

        private async Task LoadAdminDataAsync(int adminId)
        {
            Admin = await _context.Admins.FindAsync(adminId);
            if (Admin == null)
            {
                throw new NullReferenceException("Admin not found");
            }
        }

        private async Task LoadRoundDataAsync(int roundId)
        {
            var round = await _context.Rounds
                .Include(r => r.Parts)
                .ThenInclude(p => p.Questions)
                .FirstOrDefaultAsync(r => r.RoundId == roundId);

            if (round == null)
            {
                throw new NullReferenceException("Round not found");
            }

            RoundName = round.RoundName;
            RoundCreateDate = round.RoundCreateDate;

            foreach (var part in round.Parts)
            {
                switch (part.PartOrder)
                {
                    case 1:
                        QuestionsPart1 = part.Questions.Select(q => new QuestionInputModel
                        {
                            Name = q.QuestionText,
                            Answer = q.QuestionMetadata
                        }).ToList();
                        break;
                    case 2:
                        QuestionsPart2 = part.Questions.Select(q => new QuestionInputModel
                        {
                            Name = q.QuestionText,
                            Answer = q.QuestionMetadata.Split("Correct: ")[1],
                            Choice1 = q.QuestionMetadata.Split("; ")[0].Split(": ")[1],
                            Choice2 = q.QuestionMetadata.Split("; ")[1].Split(": ")[1],
                            Choice3 = q.QuestionMetadata.Split("; ")[2].Split(": ")[1],
                            Choice4 = q.QuestionMetadata.Split("; ")[3].Split(": ")[1],
                        }).ToList();
                        break;
                    case 3:
                        QuestionsPart3 = part.Questions.Select(q => new QuestionInputModel
                        {
                            Name = q.QuestionText,
                            Answer = q.QuestionMetadata
                        }).ToList();
                        break;
                }
            }
        }

        public class QuestionInputModel
        {
            public string Name { get; set; } = string.Empty;
            public string Answer { get; set; } = string.Empty;
            public string Choice1 { get; set; } = string.Empty;
            public string Choice2 { get; set; } = string.Empty;
            public string Choice3 { get; set; } = string.Empty;
            public string Choice4 { get; set; } = string.Empty;
        }
    }
}
