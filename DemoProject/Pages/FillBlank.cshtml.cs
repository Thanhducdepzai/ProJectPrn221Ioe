﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;

namespace DemoProject.Pages
{
    public class FillBlankModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public List<Question> ListQuestions { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PartId { get; set; }
        public List<String> data;

        public FillBlankModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {
            ListQuestions = await _context.Questions
            .Where(q => q.PartId == PartId)
            .OrderBy(q => Guid.NewGuid())
            .Take(10)
            .ToListAsync();
            ViewData["list"] = ListQuestions;
        }

        public async Task<IActionResult> OnPostAsync(int finalScore, int timeSpent)
        {
            var partResultDetail = await _context.PresentPartResultDetails
                .FirstOrDefaultAsync(p => p.PartId == PartId);
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
