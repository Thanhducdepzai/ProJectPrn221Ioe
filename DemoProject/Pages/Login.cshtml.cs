using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;
using Newtonsoft.Json;
using GoldBracelet_HE172196_HoangThuPhuong;

namespace DemoProject.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        [BindProperty]
        public bool IsAdmin { get; set; }

        public LoginModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {
        }
        public async Task<IActionResult> OnPostAsync(string studentUsername, string studentPassword)
        {
            HttpContext.Session.SetString("IsAdmin", IsAdmin.ToString());
            if (!ModelState.IsValid)
            {
                ViewData["message"] = "ModelSate is not valid";
                return Page();
            }
            if (!IsAdmin)
            {
                var student = GetAccount(studentUsername, studentPassword);
                if (student != null)
                {
                    HttpContext.Session.SetObjectAsJson("Student", student);                   
                    return RedirectToPage("/HomeStudent");
                }
                else
                {
                    ViewData["error"] = "Tên người dùng hoặc mật khẩu không chính xác.Nếu bạn là admin thì tich vào ô 'Đăng nhập với tài khoản là admin'";
                    return Page();
                }
            }
            else
            {
                var admin = GetAccountAdmin(studentUsername, studentPassword);
                if (admin != null)
                {
                    HttpContext.Session.SetObjectAsJson("Admin", admin);
                    return RedirectToPage("/Admin2");
                }
                else
                {
                    ViewData["error"] = "Tài khoản của bạn không phải là của admin.";
                    return Page();
                }
            }
        }
        private Student GetAccount(string studentUsername, string studentPassword)
        {
            var student = _context.Students.FirstOrDefault(m => m.StudentPassword == studentPassword && m.StudentUsername == studentUsername);
            return student;
        }
        private Admin GetAccountAdmin(string studentUsername, string studentPassword)
        {
            var admin = _context.Admins.FirstOrDefault(m => m.AdminUsername == studentUsername && m.AdminPassword == studentPassword);
            return admin;
        }
    }
}
