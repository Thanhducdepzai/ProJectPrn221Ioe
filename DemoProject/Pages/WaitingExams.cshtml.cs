using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class WaitingExamsModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public WaitingExamsModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        public IList<Round> Rounds { get; set; }
        public Admin Admin { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AdminId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchRoundName { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? MinRoundCreateDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? MaxRoundCreateDate { get; set; }

        [BindProperty]
        public List<int> SelectedRounds { get; set; }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            int pageSize = 3;
            Admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == AdminId);

            if (Admin != null)
            {
                IQueryable<Round> query = _context.Rounds
                                                  .Where(r => r.adminId == AdminId && r.isPublic == "False");
                if (!string.IsNullOrEmpty(SearchRoundName))
                {
                    query = query.Where(r => r.RoundName.Contains(SearchRoundName));
                }

                if (MinRoundCreateDate.HasValue)
                {
                    query = query.Where(r => r.RoundCreateDate >= MinRoundCreateDate);
                }

                if (MaxRoundCreateDate.HasValue)
                {
                    query = query.Where(r => r.RoundCreateDate <= MaxRoundCreateDate);
                }

                TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);

                Rounds = await query
                              .OrderByDescending(r => r.RoundCreateDate)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

                CurrentPage = pageNumber;
            }
            else
            {
                Rounds = new List<Round>();
                TotalPages = 0;
                CurrentPage = 1;
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostMakePublicAsync()
        {
            if (SelectedRounds == null || !SelectedRounds.Any())
            {
                return Page();
            }

            var roundsToUpdate = await _context.Rounds
                                               .Where(r => SelectedRounds.Contains(r.RoundId))
                                               .ToListAsync();

            foreach (var round in roundsToUpdate)
            {
                round.isPublic = "True";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage(new
            {
                AdminId = AdminId,
                SearchRoundName = SearchRoundName,
                MinRoundCreateDate = MinRoundCreateDate,
                MaxRoundCreateDate = MaxRoundCreateDate
            });
        }

        [HttpPost]
        public async Task<IActionResult> OnPostDeleteRoundAsync(int roundId, int adminId)
        {
            // Find the round to delete
            var round = await _context.Rounds.FirstOrDefaultAsync(r => r.RoundId == roundId);

            // Check if the round exists and if it belongs to the admin
            if (round == null || round.adminId != adminId)
            {
                return NotFound();
            }

            // Get all parts associated with the roundId
            var parts = await _context.Parts.Where(p => p.RoundId == roundId).ToListAsync();

            // Get all PartIds from the parts
            var partIds = parts.Select(p => p.PartId).ToList();

            // Get all questions associated with the parts
            var questions = await _context.Questions.Where(q => partIds.Contains(q.PartId)).ToListAsync();

            // Remove associated questions first
            _context.Questions.RemoveRange(questions);

            // Remove associated parts next
            _context.Parts.RemoveRange(parts);

            // Finally, remove the round itself
            _context.Rounds.Remove(round);

            // Save changes to remove questions, parts, and the round
            await _context.SaveChangesAsync();

            // Reload the Admin object to ensure it is not null
            Admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == adminId);

            // Redirect to the page with updated query parameters
            return RedirectToPage(new { AdminId = adminId, SearchRoundName = SearchRoundName, MinRoundCreateDate = MinRoundCreateDate, MaxRoundCreateDate = MaxRoundCreateDate });
        }
    }
}
