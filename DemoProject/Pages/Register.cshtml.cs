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
        public RegisterModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        // Properties for binding
        [BindProperty]
        public Student student { get; set; } = new Student();
        [BindProperty]
        public LevelOfSchool levelOfSchool { get; set; } = new LevelOfSchool();
        [BindProperty]
        public Province province { get; set; } = new Province();
        [BindProperty]
        public District district { get; set; } = new District();
        [BindProperty]
        public School school { get; set; } = new School();

        // List to store grades
        public List<Grade> Grades { get; set; }

        // OnGet method to initialize data
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

            // Fetch grades from database
            Grades = _context.Grades.ToList();
            ViewData["Grades"] = new SelectList(Grades, "GradeId", "GradeName");
        }

        // OnPost method for form submission
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ModelStateIsValid"] = "ModelState.IsValid is " + !ModelState.IsValid + ModelState.Values;
                ViewData["Students"] = "_context.Students is " + (_context.Students == null);
                ViewData["student"] = "student is " + (student == null);
                ViewData["LevelOfSchool_Selected"] = "Level School" + levelOfSchool.LevelSchoolId;
                ViewData["Province_Selected"] = "district" + district.DistricId;

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

        public async Task<JsonResult> OnGetSchoolsAsync(int levelOfSchoolId, int districtId)
        {
            var schools = await _context.Schools
                .Where(s => s.LevelSchoolId == levelOfSchoolId && s.DistrictId == districtId)
                .Select(s => new { s.SchoolId, s.SchoolName })
                .ToListAsync();
            return new JsonResult(schools);
        }
    }
}
