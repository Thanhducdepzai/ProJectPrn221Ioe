
using ProjectIoePrn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public RegisterModel (IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        } 
        public void OnGet()
        {
            ViewData["ModelStateIsValid"] = "N/A";
            ViewData["Students"] = "N/A";
            ViewData["student"] = "N/A";
            ViewData["LevelOfSchool_Selected"] = "N/A";
            ViewData["Province_Selected"] = "N/A";
            ViewData["LevelOfSchool"] = new SelectList(_context.LevelOfSchools, "LevelSchoolId", "LevelName");
            ViewData["Province"] = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
            ViewData["District"] = new SelectList(_context.Districts, "DistricId", "DistricName");
            ViewData["School"] = new SelectList(_context.Schools, "SchoolId", "SchoolName");
        }
        [BindProperty]
        public Student student { get; set; } = default!;
        [BindProperty]
        public LevelOfSchool levelOfSchool { get; set; } = default!;
        [BindProperty]
        public Province province { get; set; } = default!;
        [BindProperty]
        public District district { get; set; } = default!;
        [BindProperty]
        public School school { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {         
            if (!ModelState.IsValid|| _context.Students == null ||  student == null)
            {
                ViewData["ModelStateIsValid"] ="ModelState.IsValid is "+ !ModelState.IsValid + ModelState.Values;
                ViewData["Students"] = "_context.Students is " + (_context.Students==null);
                ViewData["student"] = "student is " + (student == null);
                ViewData["LevelOfSchool_Selected"] = "Level School"+ levelOfSchool.LevelSchoolId;
                ViewData["Province_Selected"] ="district" +  district.DistricId;

                return Page();
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Login");
        }
        public async Task<JsonResult> OnGetDistrictsAsync(int provinceId)
        {
            var districts = await _context.Districts
                .Where(d => d.ProvinceId == provinceId)
                .Select(d => new { d.DistricId, d.DistricName })
                .ToListAsync();

            return new JsonResult(districts);
        }
        public async Task<JsonResult> OnGetSchoolsAsync(int levelOfSchoolId)
        {
            var schools = await _context.Schools
                .Where(d => d.LevelSchoolId == levelOfSchoolId)
                .ToListAsync();

            return new JsonResult(schools);
        }

    }
}

