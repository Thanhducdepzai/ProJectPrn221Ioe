﻿using Microsoft.AspNetCore.Mvc;
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
        public int CurrentRoundId { get; set; }

        public async Task OnGetAsync(int? studentId)
        {
            if (studentId == null)
            {
                return;
            }

            Student = await _context.Students
                                    .Include(s => s.School)
                                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

            IndividualStudyResults = await _context.IndividualResultDetails
                                             .Where(ir => ir.UserId == studentId)
                                             .ToListAsync();

            Parts = await _context.Parts.ToListAsync();

            Rounds = await _context.Rounds.ToListAsync();

            // Determine the current round
            var completedRounds = IndividualStudyResults.Select(ir => ir.RoundId).Distinct().ToList();
            CurrentRoundId = completedRounds.Count > 0 ? completedRounds.Max() + 1 : 1;

            // Check if all parts of the current round are completed
            var allPartsInCurrentRound = await _context.Parts
                                                       .Where(p => p.RoundId == CurrentRoundId)
                                                       .ToListAsync();

            var partResultsInCurrentRound = await _context.PresentPartResultDetails
                                                          .Include(pprd => pprd.IndividualResult)
                                                          .ThenInclude(ir => ir.Round)
                                                          .Where(pprd => pprd.IndividualResult.UserId == studentId && pprd.IndividualResult.RoundId == CurrentRoundId)
                                                          .ToListAsync();

            bool allPartsCompleted = allPartsInCurrentRound.All(part => partResultsInCurrentRound.Any(r => r.PartId == part.PartId && r.Score != -1 && r.CompleteTime != -1));

            if (allPartsCompleted)
            {
                CurrentRoundId++;
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
