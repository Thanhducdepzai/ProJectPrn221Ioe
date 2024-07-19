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
    public class ListExamsModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public ListExamsModel(IOE_Project_Clone_PRN221Context context)
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

        public async Task OnGetAsync(int pageNumber = 1)
        {
            int pageSize = 3;
            Admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == AdminId);

            if (Admin != null)
            {
                IQueryable<Round> query = _context.Rounds
                                                  .Where(r => r.AdminId == AdminId && r.IsPublic == "True");
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
    }
}
