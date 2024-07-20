using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class AdminCreateExamModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        private readonly ILogger<AdminCreateExamModel> _logger;

        public AdminCreateExamModel(IOE_Project_Clone_PRN221Context context, ILogger<AdminCreateExamModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public string RoundName { get; set; } = string.Empty;

        [BindProperty]
        public DateTime RoundCreateDate { get; set; } = DateTime.Now;

        [BindProperty]
        public int GradeChoosingId { get; set; }

        public List<Grade> Grades { get; set; } = new List<Grade>();

        [BindProperty]
        public List<QuestionInputModel> QuestionsPart1 { get; set; } = new List<QuestionInputModel>();

        [BindProperty]
        public List<QuestionInputModel> QuestionsPart2 { get; set; } = new List<QuestionInputModel>();

        [BindProperty]
        public List<QuestionInputModel> QuestionsPart3 { get; set; } = new List<QuestionInputModel>();

        [BindProperty]
        public int AdminId { get; set; }

        public Admin Admin { get; set; }

        public async Task<IActionResult> OnGetAsync(int? adminId)
        {
            if (adminId == null)
            {
                return NotFound();
            }

            AdminId = adminId.Value;
            await LoadAdminDataAsync(AdminId);

            // Load Grades
            Grades = await _context.Grades.ToListAsync();

            // Initialize the question lists with the desired number of empty questions
            for (int i = 0; i < 10; i++)
            {
                QuestionsPart1.Add(new QuestionInputModel());
                QuestionsPart2.Add(new QuestionInputModel());
            }

            for (int i = 0; i < 8; i++)
            {
                QuestionsPart3.Add(new QuestionInputModel());
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertMessage"] = "Thêm mới vòng thi không thành công!";
                await LoadAdminDataAsync(AdminId);
                return Page();
            }

            try
            {
                var round = new Round
                {
                    RoundName = RoundName,
                    RoundCreateDate = RoundCreateDate,
                    RoundUpdateDate = DateTime.Now,
                    GradeId = GradeChoosingId,
                    isPublic = "False", // Assuming default isPublic value
                    adminId = AdminId
                };
                _logger.LogInformation("Inserting Round: {@Round}", round);

                _context.Rounds.Add(round);
                await _context.SaveChangesAsync();

                var part1 = new Part
                {
                    PartName = "Phần 1: Điền vào chỗ trống",
                    PartOrder = 1,
                    PartCreateDate = DateTime.Now,
                    PartUpdateDate = DateTime.Now,
                    TypeOfQuestionId = 1, // Assuming type of question ID
                    RoundId = round.RoundId
                };

                _logger.LogInformation("Inserting Part 1: {@Part}", part1);

                _context.Parts.Add(part1);
                await _context.SaveChangesAsync();

                foreach (var question in QuestionsPart1)
                {
                    var newQuestion = new Question
                    {
                        QuestionText = question.Name,
                        QuestionMetadata = question.Answer,
                        PartId = part1.PartId
                    };

                    _logger.LogInformation("Inserting Question for Part 1: {@Question}", newQuestion);

                    _context.Questions.Add(newQuestion);
                }

                var part2 = new Part
                {
                    PartName = "Phần 2: Trắc nghiệm",
                    PartOrder = 2,
                    PartCreateDate = DateTime.Now,
                    PartUpdateDate = DateTime.Now,
                    TypeOfQuestionId = 2, // Assuming type of question ID
                    RoundId = round.RoundId
                };

                _logger.LogInformation("Inserting Part 2: {@Part}", part2);

                _context.Parts.Add(part2);
                await _context.SaveChangesAsync();

                foreach (var question in QuestionsPart2)
                {
                    var newQuestion = new Question
                    {
                        QuestionText = question.Name,
                        QuestionMetadata = $"1: {question.Choice1}; 2: {question.Choice2}; 3: {question.Choice3}; 4: {question.Choice4}; Correct: {question.Answer}",
                        PartId = part2.PartId
                    };

                    _logger.LogInformation("Inserting Question for Part 2: {@Question}", newQuestion);
                    _context.Questions.Add(newQuestion);
                }

                var part3 = new Part
                {
                    PartName = "Phần 3: Nối từ",
                    PartOrder = 3,
                    PartCreateDate = DateTime.Now,
                    PartUpdateDate = DateTime.Now,
                    TypeOfQuestionId = 3, // Assuming type of question ID
                    RoundId = round.RoundId
                };

                _logger.LogInformation("Inserting Part 3: {@Part}", part3);

                _context.Parts.Add(part3);
                await _context.SaveChangesAsync();

                foreach (var question in QuestionsPart3)
                {
                    var newQuestion = new Question
                    {
                        QuestionText = question.Name,
                        QuestionMetadata = question.Answer,
                        PartId = part3.PartId
                    };

                    _logger.LogInformation("Inserting Question for Part 3: {@Question}", newQuestion);

                    _context.Questions.Add(newQuestion);
                }

                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Thêm vòng thi mới thành công!";
                return RedirectToPage("./CreateExam", new { AdminId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the exam");
                TempData["AlertMessage"] = "Thêm mới vòng thi không thành công!";
                await LoadAdminDataAsync(AdminId);
                return Page();
            }
        }

        private async Task LoadAdminDataAsync(int adminId)
        {
            Admin = await _context.Admins.FindAsync(adminId);
            if (Admin == null)
            {
                throw new NullReferenceException("Admin not found");
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
