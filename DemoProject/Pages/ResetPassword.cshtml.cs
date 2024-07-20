using DemoProject.Models;
using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public ResetPasswordModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Otp { get; set; }

        public Admin AdminAccount { get; set; }
        public Student StudentAccount { get; set; }

        public void OnGet(string email, bool isAdmin)
        {
            Email = email;
            HttpContext.Session.SetString("IsAdmin", isAdmin.ToString());
        }

        public async Task<IActionResult> OnPostAsync(string newPassword, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                ViewData["error"] = "ModelState is not valid";
                return Page();
            }

            if (newPassword != confirmPassword)
            {
                ViewData["error"] = "Mật khẩu xác nhận không trùng khớp. Vui lòng thử lại.";
                return Page();
            }

            var sessionOtp = HttpContext.Session.GetString("OTP");
            if (Otp != sessionOtp)
            {
                ViewData["error"] = "OTP không hợp lệ.";
                return Page();
            }

            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != null && isAdmin.ToLower() == "true")
            {
                AdminAccount = HttpContext.Session.GetObjectFromJson<Admin>("Admin");
                if (AdminAccount != null)
                {
                    AdminAccount.AdminPassword = newPassword;
                    _context.Admins.Update(AdminAccount);
                    await _context.SaveChangesAsync();
                    ViewData["message"] = "Reset mật khẩu của admin thành công.";
                }
                else
                {
                    ViewData["error"] = "Không tìm thấy tài khoản admin.";
                }
            }
            else
            {
                StudentAccount = HttpContext.Session.GetObjectFromJson<Student>("Student");
                if (StudentAccount != null)
                {
                    StudentAccount.StudentPassword = newPassword;
                    _context.Students.Update(StudentAccount);
                    await _context.SaveChangesAsync();
                    ViewData["message"] = "Reset mật khẩu của student thành công.";
                }
                else
                {
                    ViewData["error"] = "Không tìm thấy tài khoản student.";
                }
            }

            return Page();
        }

        public IActionResult OnPostGoToLogin()
        {
            return RedirectToPage("/Login");
        }
    }
}
