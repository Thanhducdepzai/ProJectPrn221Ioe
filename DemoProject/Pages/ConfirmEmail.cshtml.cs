using ProjectIoePrn.Models;
using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Pages
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        [BindProperty]
        public bool IsAdmin { get; set; }
        public ConfirmEmailModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string email)
        {
            email = email.Trim();
            if (!ModelState.IsValid)
            {
                ViewData["error"] = "ModelSate is not valid";
                return Page();
            }
            var hasEmail = checkEmail(email);
            if (hasEmail)
            {
                ViewData["message"] = "Link đặt lại mật khẩu đã được gửi qua mail.Vui lòng kiểm tra thông báo trong email.";
                return RedirectToPage("/ResetPassword");
            }
            else
            {
                ViewData["error"] = "Email không tồn tại";
                return Page();
            }
        }

        private bool checkEmail(string email )
        {
            HttpContext.Session.SetString("IsAdmin", IsAdmin.ToString());
            if (IsAdmin)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.AdminGmail == email);
                if(admin != null)
                {
                    HttpContext.Session.SetObjectAsJson("Admin", admin);
                    return true;
                }
                
                return false;
            }
            else
            {
                var student = _context.Students.FirstOrDefault(m => m.StudentGmail == email );
                if (student != null)
                {
                    HttpContext.Session.SetObjectAsJson("Student", student);
                    return true;
                }
                return false;
            }
        }
    }
}
