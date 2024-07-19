using Microsoft.AspNetCore.Mvc.RazorPages;
using DemoProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DemoProject.Pages
{
    public class Index1Model : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public Student stu { get; set; }
        public School school { get; set; }

        public Index1Model(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId != null)
            {
                stu = _context.Students
                              .Include(s => s.Grade) // Include the Grade property
                              .FirstOrDefault(s => s.StudentId == studentId);
                if (stu != null)
                {
                    school = _context.Schools
                                     .Include(s => s.District)
                                     .ThenInclude(d => d.Province)
                                     .FirstOrDefault(sc => sc.SchoolId == stu.SchoolId);
                }
            }
        }
    }
}
