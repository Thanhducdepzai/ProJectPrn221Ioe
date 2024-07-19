using DemoProject.Models;
using DemoProject.Pages.EmailOtp;
using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        private readonly EmailService _emailService;

        [BindProperty]
        public bool IsAdmin { get; set; }

        public ConfirmEmailModel(IOE_Project_Clone_PRN221Context context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string email)
        {
            email = email.Trim().ToLower();
            if (!ModelState.IsValid)
            {
                ViewData["error"] = "ModelState is not valid";
                return Page();
            }

            var hasEmail = CheckEmail(email);
            if (hasEmail)
            {
                var otp = GenerateOtp();
                await _emailService.SendEmailAsync(email, "OTP cho việc đặt lại mật khẩu", $"Mã OTP của bạn là: {otp}");

                ViewData["message"] = "Link đặt lại mật khẩu đã được gửi qua mail. Vui lòng kiểm tra thông báo trong email.";
                return RedirectToPage("/ResetPassword", new { email = email, isAdmin = IsAdmin });
            }
            else
            {
                ViewData["error"] = "Email không tồn tại";
                return Page();
            }
        }

        private bool CheckEmail(string email)
        {
            HttpContext.Session.SetString("IsAdmin", IsAdmin.ToString());
            if (IsAdmin)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.AdminGmail.ToLower() == email);
                if (admin != null)
                {
                    HttpContext.Session.SetObjectAsJson("Admin", admin);
                    return true;
                }
                return false;
            }
            else
            {
                var student = _context.Students.FirstOrDefault(m => m.StudentGmail.ToLower() == email);
                if (student != null)
                {
                    HttpContext.Session.SetObjectAsJson("Student", student);
                    return true;
                }
                return false;
            }
        }

        private string GenerateOtp()
        {
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();
            HttpContext.Session.SetString("OTP", otp);
            return otp;
        }
    }
}
