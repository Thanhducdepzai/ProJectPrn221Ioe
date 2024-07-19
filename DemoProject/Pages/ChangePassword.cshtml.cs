using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using DemoProject.Models;

namespace DemoProject.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public Student stu;
        public Student stuTest;
        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        public ChangePasswordModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public void OnGet()
        {
            var studentJson2 = HttpContext.Session.GetObjectFromJson<Student>("Student");
            if (studentJson2!=null)
            {
                stu = studentJson2;

            }
        }


        public IActionResult OnPost()
        {
            var studentJson2 = HttpContext.Session.GetObjectFromJson<Student>("Student");
            if (studentJson2 != null)
            {
                stu = studentJson2;

            }

            ViewData["a"] = stu.StudentPassword;

            ViewData["b"] =OldPassword;

            if ( !OldPassword.Trim().Equals(stu.StudentPassword.Trim()) )
            {
                ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "New passwords do not match.");
                return Page();
            }

            var studentToUpdate = _context.Students.FirstOrDefault(s => s.StudentId == stu.StudentId);
            if (studentToUpdate != null)
            {
                studentToUpdate.StudentPassword = NewPassword;
                _context.SaveChanges();
                HttpContext.Session.SetObjectAsJson("Student", studentToUpdate);
            }
            ViewData["SuccessMessage"] = "Password changed successfully.";
            return Page();
        }

    }
}
