using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIoePrn.Models;
using System;
using System.Linq;

namespace ProjectIoePrn.Pages
{
    public class AdminCreateExamModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public AdminCreateExamModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Admin Admin { get; set; }

        public IActionResult OnGet(int? AdminId)
        {
            if (AdminId == null)
            {
                return NotFound();
            }

            Admin = _context.Admins.FirstOrDefault(a => a.AdminId == AdminId);

            if (Admin == null)
            {
                return NotFound();
            }

            return Page();
        }

        public JsonResult OnPostCreateExam(string roundName, string examDate, string isPublic, string questionsPart1, string questionsPart2, string questionsPart3)
        {
            try
            {
                // Step 1: Insert new round
                int maxRoundId = _context.Rounds.Max(r => r.RoundId);
                Round newRound = new Round
                {
                    RoundId = maxRoundId + 1,
                    RoundName = roundName,
                    RoundCreateDate = DateTime.Parse(examDate),
                    RoundUpdateDate = isPublic == "False" ? DateTime.Parse(examDate) : DateTime.MinValue,
                    GradeId = 3,
                    isPublic = isPublic
                };
                _context.Rounds.Add(newRound);
                _context.SaveChanges();

                // Step 2: Insert parts for the new round
                int newRoundId = newRound.RoundId;
                InsertParts(newRoundId, examDate);

                // Step 3: Insert questions
                InsertQuestions(newRoundId, questionsPart1, questionsPart2, questionsPart3, examDate);

                return new JsonResult(new { success = true, message = "Exam created successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }


        private void InsertParts(int roundId, string examDate)
        {
            DateTime createDate = DateTime.Parse(examDate);

            var parts = new[]
            {
                new Part
                {
                    PartId = _context.Parts.Max(p => p.PartId) + 1,
                    PartName = "Part 1",
                    PartOrder = 1,
                    PartCreateDate = createDate,
                    PartUpdateDate = createDate,
                    TypeOfQuestionId = 1,
                    RoundId = roundId
                },
                new Part
                {
                    PartId = _context.Parts.Max(p => p.PartId) + 2,
                    PartName = "Part 2",
                    PartOrder = 2,
                    PartCreateDate = createDate,
                    PartUpdateDate = createDate,
                    TypeOfQuestionId = 2,
                    RoundId = roundId
                },
                new Part
                {
                    PartId = _context.Parts.Max(p => p.PartId) + 3,
                    PartName = "Part 3",
                    PartOrder = 3,
                    PartCreateDate = createDate,
                    PartUpdateDate = createDate,
                    TypeOfQuestionId = 3,
                    RoundId = roundId
                }
            };

            _context.Parts.AddRange(parts);
            _context.SaveChanges();
        }

        private void InsertQuestions(int roundId, string questionsPart1Json, string questionsPart2Json, string questionsPart3Json, string examDate)
        {
            var part1 = _context.Parts.First(p => p.RoundId == roundId && p.PartOrder == 1);
            var part2 = _context.Parts.First(p => p.RoundId == roundId && p.PartOrder == 2);
            var part3 = _context.Parts.First(p => p.RoundId == roundId && p.PartOrder == 3);

            int questionId = _context.Questions.Max(q => q.QuestionId);

            // Insert Part 1 Questions
            var questionsPart1 = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, QuestionModel>>(questionsPart1Json);
            foreach (var question in questionsPart1.Values)
            {
                _context.Questions.Add(new Question
                {
                    QuestionId = ++questionId,
                    QuestionText = question.name,
                    QuestionMetadata = question.answer,
                    PartId = part1.PartId
                });
            }

            // Insert Part 2 Questions
            var questionsPart2 = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, QuestionPart2Model>>(questionsPart2Json);
            foreach (var question in questionsPart2.Values)
            {
                string metadata = $"1: {question["1"]}; 2: {question["2"]}; 3: {question["3"]}; 4: {question["4"]}; Correct: {question.correct}";
                _context.Questions.Add(new Question
                {
                    QuestionId = ++questionId,
                    QuestionText = question.name,
                    QuestionMetadata = metadata,
                    PartId = part2.PartId
                });
            }

            // Insert Part 3 Questions
            var questionsPart3 = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, QuestionPart3Model>>(questionsPart3Json);
            foreach (var question in questionsPart3.Values)
            {
                _context.Questions.Add(new Question
                {
                    QuestionId = ++questionId,
                    QuestionText = question.vietnamese,
                    QuestionMetadata = question.english,
                    PartId = part3.PartId
                });
            }

            _context.SaveChanges();
        }
    }

    public class QuestionModel
    {
        public string name { get; set; }
        public string answer { get; set; }
    }

    public class QuestionPart2Model : QuestionModel
    {
        public string this[string key]
        {
            get { return (string)this.GetType().GetProperty(key).GetValue(this); }
            set { this.GetType().GetProperty(key).SetValue(this, value); }
        }

        public string correct { get; set; }
    }

    public class QuestionPart3Model
    {
        public string vietnamese { get; set; }
        public string english { get; set; }
    }
}
