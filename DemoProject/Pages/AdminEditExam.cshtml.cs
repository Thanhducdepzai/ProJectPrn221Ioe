using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;

namespace ProjectIoePrn.Pages
{
    public class AdminEditExamModel : PageModel
    {
        private readonly ProjectIoePrn.Models.IOE_Project_Clone_PRN221Context _context;

        public AdminEditExamModel(ProjectIoePrn.Models.IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public string RoundName { get; set; } = string.Empty;

        [BindProperty]
        public DateTime RoundCreateDate { get; set; }

        [BindProperty]
        public List<QuestionViewModel> QuestionsPart1 { get; set; } = new List<QuestionViewModel>();

        [BindProperty]
        public List<QuestionViewModel> QuestionsPart2 { get; set; } = new List<QuestionViewModel>();

        [BindProperty]
        public List<QuestionViewModel> QuestionsPart3 { get; set; } = new List<QuestionViewModel>();

        public Admin Admin { get; set; }
        public int RoundId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _context.Rounds
                .Include(r => r.Parts)
                .ThenInclude(p => p.Questions)
                .FirstOrDefaultAsync(r => r.RoundId == id);

            if (round == null)
            {
                return NotFound();
            }

            RoundId = round.RoundId;
            RoundName = round.RoundName;
            RoundCreateDate = round.RoundCreateDate;

            foreach (var part in round.Parts)
            {
                switch (part.PartOrder)
                {
                    case 1:
                        QuestionsPart1 = part.Questions.Select(q => new QuestionViewModel(q)).ToList();
                        break;
                    case 2:
                        QuestionsPart2 = part.Questions.Select(q => new QuestionViewModel(q)).ToList();
                        break;
                    case 3:
                        QuestionsPart3 = part.Questions.Select(q => new QuestionViewModel(q)).ToList();
                        break;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var round = await _context.Rounds
                .Include(r => r.Parts)
                .ThenInclude(p => p.Questions)
                .FirstOrDefaultAsync(r => r.RoundId == id);

            if (round == null)
            {
                return NotFound();
            }

            round.RoundName = RoundName;
            round.RoundCreateDate = RoundCreateDate;

            UpdateQuestions(round.Parts, QuestionsPart1, 1);
            UpdateQuestions(round.Parts, QuestionsPart2, 2);
            UpdateQuestions(round.Parts, QuestionsPart3, 3);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoundExists(round.RoundId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private void UpdateQuestions(IEnumerable<Part> parts, List<QuestionViewModel> questions, int partOrder)
        {
            var part = parts.FirstOrDefault(p => p.PartOrder == partOrder);
            if (part != null)
            {
                foreach (var question in questions)
                {
                    var existingQuestion = part.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
                    if (existingQuestion != null)
                    {
                        existingQuestion.QuestionText = question.Name;
                        if (partOrder == 2)
                        {
                            existingQuestion.QuestionMetadata = $"1: {question.Choice1}; 2: {question.Choice2}; 3: {question.Choice3}; 4: {question.Choice4}; Correct: {question.Answer}";
                        }
                        else
                        {
                            existingQuestion.QuestionMetadata = question.Answer;
                        }
                    }
                }
            }
        }

        private bool RoundExists(int id)
        {
            return _context.Rounds.Any(e => e.RoundId == id);
        }
    }

    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public string Choice1 { get; set; } = string.Empty;
        public string Choice2 { get; set; } = string.Empty;
        public string Choice3 { get; set; } = string.Empty;
        public string Choice4 { get; set; } = string.Empty;

        public QuestionViewModel() { }

        public QuestionViewModel(Question question)
        {
            QuestionId = question.QuestionId;
            Name = question.QuestionText;

            if (question.QuestionMetadata.Contains("Correct:"))
            {
                var metadataParts = question.QuestionMetadata.Split(';');
                if (metadataParts.Length >= 5)
                {
                    Choice1 = metadataParts[0].Split(':')[1].Trim();
                    Choice2 = metadataParts[1].Split(':')[1].Trim();
                    Choice3 = metadataParts[2].Split(':')[1].Trim();
                    Choice4 = metadataParts[3].Split(':')[1].Trim();
                    Answer = metadataParts[4].Split(':')[1].Trim();
                }
            }
            else
            {
                Answer = question.QuestionMetadata;
            }
        }
    }
}
