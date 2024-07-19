using Microsoft.AspNetCore.Mvc.RazorPages;
using DemoProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GoldBracelet_HE172196_HoangThuPhuong;

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
            // Lấy đối tượng sinh viên từ session
            var student = HttpContext.Session.GetObjectFromJson<Student>("Student");
            if (student != null)
            {
                stu = _context.Students
                              .Include(s => s.Grade) // Bao gồm thuộc tính Grade
                              .FirstOrDefault(s => s.StudentName == student.StudentName);
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
