using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


using Microsoft.AspNetCore.SignalR;



using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public RegisterModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        // OnGet method to initialize data
        public void OnGet()
        {
            ViewData["ModelStateIsValid"] = "N/A";
            ViewData["Students"] = "N/A";
            ViewData["student"] = "N/A";
            ViewData["LevelOfSchool_Selected"] = "N/A";
            ViewData["Province_Selected"] = "N/A";
            ViewData["LevelOfSchool"] = new SelectList(_context.LevelOfSchools, "LevelSchoolId", "LevelName");
            ViewData["Grade"] = new SelectList(_context.Grades, "GradeId", "GradeName");
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
        [BindProperty]
        public Grade grade { get; set; } = default!;

        // OnPost method for form submission
        public async Task<IActionResult> OnPostAsync()
        {  
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

        public async Task<JsonResult> OnGetSchoolsAsync(int levelOfSchoolId, int districtId)
        {
            var schools = await _context.Schools
                .Include(a => a.District)
                .Where(s => s.LevelSchoolId == levelOfSchoolId && s.DistrictId == districtId)
                .Select(s => new { s.SchoolId, s.SchoolName })
                .ToListAsync();
            var grades = await _context.Grades
                .Include(s => s.Students)
                .Where(s => s.LevelSchoolId == levelOfSchoolId)
                .Select(s => new { s.GradeId, s.GradeName })
                .ToListAsync();
            var result = new
            {
                Schools = schools,
                Grades = grades
            };

            return new JsonResult(result);
        }
    }
}