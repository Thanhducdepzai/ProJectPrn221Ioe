using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectIoePrn.Pages
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

        public async Task OnGetAsync(int? studentId)
        {
            if (studentId == null)
            {
                return;
            }

            Student = await _context.Students
                                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

            IndividualStudyResults = await _context.IndividualResultDetails
                                             .Where(ir => ir.UserId == studentId)
                                             .ToListAsync();

            Parts = await _context.Parts.ToListAsync();

            Rounds = await _context.Rounds.ToListAsync();

            if (SelectedRoundId.HasValue)
            {
                // Get all parts of the selected round
                var allPartsInRound = await _context.Parts
                                                    .Where(p => p.RoundId == SelectedRoundId)
                                                    .ToListAsync();

                // Get results for the selected round
                var partResultsInRound = await _context.PresentPartResultDetails
                                                       .Include(pprd => pprd.IndividualResult)
                                                       .ThenInclude(ir => ir.Round)
                                                       .Where(pprd => pprd.IndividualResult.UserId == studentId && pprd.IndividualResult.RoundId == SelectedRoundId)
                                                       .ToListAsync();

                // Combine parts and results
                PartResults = allPartsInRound
                              .Select(part => new PresentPartResultDetail
                              {
                                  Part = part,
                                  Score = partResultsInRound.FirstOrDefault(r => r.PartId == part.PartId)?.Score ?? -1,
                                  CompleteTime = partResultsInRound.FirstOrDefault(r => r.PartId == part.PartId)?.CompleteTime ?? -1,
                                  IndividualResultId = partResultsInRound.FirstOrDefault(r => r.PartId == part.PartId)?.IndividualResultId ?? -1
                              }).ToList();
            }
            else
            {
                PartResults = await _context.PresentPartResultDetails
                                            .Include(pprd => pprd.IndividualResult)
                                            .ThenInclude(ir => ir.Round)
                                            .Where(pprd => pprd.IndividualResult.UserId == studentId)
                                            .ToListAsync();
            }
        }
    }
}
