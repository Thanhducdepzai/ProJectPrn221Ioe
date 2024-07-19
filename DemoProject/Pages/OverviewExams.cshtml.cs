using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class OverviewExamsModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public OverviewExamsModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? SelectedRoundId { get; set; }

        public IList<IndividualResultDetail> IndividualStudyResults { get; set; }
        public IList<PresentPartResultDetail> PartResults { get; set; }
        public IList<Part> Parts { get; set; }
        public Student Student { get; set; }
        public IList<Round> Rounds { get; set; }
        public int CurrentRoundId { get; set; }

        public async Task OnGetAsync()
        {
            var numberRound = await _context.Rounds.Where(a => a.isPublic == "True").ToListAsync();

            var studentSession = HttpContext.Session.GetObjectFromJson<Student>("Student");
            Student = _context.Students.Include(a => a.School).Where(a => a.StudentId == studentSession.StudentId).FirstOrDefault();
            IndividualStudyResults = await _context.IndividualResultDetails
                                             .Where(ir => ir.UserId == Student.StudentId && ir.RoundScore >= 0)
                                             .ToListAsync();

            Parts = await _context.Parts.ToListAsync();

            Rounds = await _context.Rounds.ToListAsync();
            // Determine the current round
            var completedRounds = IndividualStudyResults.Select(ir => ir.RoundId).Distinct().ToList();
            CurrentRoundId = completedRounds.Count > 0 ? completedRounds.Max() + 1 : 1;
            var individualResultDetailCurrentRound = await _context.IndividualResultDetails
                                             .Where(ir => ir.UserId == Student.StudentId && ir.RoundId == CurrentRoundId)
                                             .FirstOrDefaultAsync();
            if (individualResultDetailCurrentRound == null)
            {
                IndividualResultDetail individualResultDetail = new IndividualResultDetail
                {
                    RoundScore = -1,
                    CompleteTime = -1,
                    UserId = Student.StudentId,
                    RoundId = CurrentRoundId
                };
                _context.IndividualResultDetails.Add(individualResultDetail);
                await _context.SaveChangesAsync();
                var PartOfCurrentRound = _context.Parts.Include(a => a.Round).Where(a => a.RoundId == CurrentRoundId).ToList();
                foreach (var part in PartOfCurrentRound)
                {
                    PresentPartResultDetail presentPartResultDetail = new PresentPartResultDetail
                    {
                        Score = -1,
                        CompleteTime = -1,
                        PartId = part.PartId,
                        IndividualResultId = individualResultDetail.IndividualResultId
                    };
                    _context.PresentPartResultDetails.Add(presentPartResultDetail);
                    await _context.SaveChangesAsync();
                }
            }
            // Check if all parts of the current round are completed
            var allPartsInCurrentRound = await _context.Parts
                                                       .Where(p => p.RoundId == CurrentRoundId)
                                                       .ToListAsync();

            var partResultsInCurrentRound = await _context.PresentPartResultDetails
                                                          .Include(pprd => pprd.IndividualResult)
                                                          .ThenInclude(ir => ir.Round)
                                                          .Where(pprd => pprd.IndividualResult.UserId == Student.StudentId && pprd.IndividualResult.RoundId == CurrentRoundId)
                                                          .ToListAsync();

            bool allPartsCompleted = allPartsInCurrentRound.All(part => partResultsInCurrentRound.Any(r => r.PartId == part.PartId && r.Score != -1 && r.CompleteTime != -1));

            if (allPartsCompleted)
            {
                var totalScore = partResultsInCurrentRound.Sum(pr => pr.Score);
                var totalTime = partResultsInCurrentRound.Sum(pr => pr.CompleteTime);

                individualResultDetailCurrentRound.RoundScore = totalScore;
                individualResultDetailCurrentRound.CompleteTime = totalTime;

                _context.IndividualResultDetails.Update(individualResultDetailCurrentRound);
                await _context.SaveChangesAsync();
                CurrentRoundId++;
                IndividualStudyResults = await _context.IndividualResultDetails
                                             .Where(ir => ir.UserId == Student.StudentId && ir.RoundScore >= 0)
                                             .ToListAsync();
                IndividualResultDetail individualResultDetail = new IndividualResultDetail
                {
                    RoundScore = -1,
                    CompleteTime = -1,
                    UserId = Student.StudentId,
                    RoundId = CurrentRoundId
                };
                _context.IndividualResultDetails.Add(individualResultDetail);
                await _context.SaveChangesAsync();
                var PartOfCurrentRound = _context.Parts.Include(a => a.Round).Where(a => a.RoundId == CurrentRoundId).ToList();
                foreach (var part in PartOfCurrentRound)
                {
                    PresentPartResultDetail presentPartResultDetail = new PresentPartResultDetail
                    {
                        Score = -1,
                        CompleteTime = -1,
                        PartId = part.PartId,
                        IndividualResultId = individualResultDetail.IndividualResultId
                    };
                    _context.PresentPartResultDetails.Add(presentPartResultDetail);
                    await _context.SaveChangesAsync();
                }
            }

            // Get results for the current round
            var allPartsInCurrentRoundWithResults = await _context.Parts
                                                       .Where(p => p.RoundId == CurrentRoundId)
                                                       .ToListAsync();

            PartResults = allPartsInCurrentRoundWithResults
                          .Select(part => new PresentPartResultDetail
                          {
                              Part = part,
                              Score = partResultsInCurrentRound.FirstOrDefault(r => r.PartId == part.PartId)?.Score ?? -1,
                              CompleteTime = partResultsInCurrentRound.FirstOrDefault(r => r.PartId == part.PartId)?.CompleteTime ?? -1,
                              IndividualResultId = partResultsInCurrentRound.FirstOrDefault(r => r.PartId == part.PartId)?.IndividualResultId ?? -1
                          }).ToList();
        }
    }
}