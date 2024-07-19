using DemoProject.Models;
using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace DemoProject.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public ResetPasswordModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public Admin adminAccount { get; set; }
        public Student studentAccount { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string newPassword, string confirmPassword)
        {
            
            if (!ModelState.IsValid)
            {
                ViewData["error"] = "ModelSate is not valid";
                return Page();
            }
            if(newPassword != confirmPassword)
            {
                ViewData["error"] = "Mật khẩu xác nhận không trùng khớp. Vui lòng thử lại.";
                return Page();
            }
            else
            {
                var isAdmin = HttpContext.Session.GetString("IsAdmin");
                if (isAdmin != null && isAdmin.ToLower() == "true")
                {
                    adminAccount = HttpContext.Session.GetObjectFromJson<Admin>("Admin");
                    if (adminAccount != null)
                    {
                        adminAccount.AdminPassword = newPassword; 
                        _context.Admins.Update(adminAccount);
                        await _context.SaveChangesAsync();
                        ViewData["error"] = "Reset mật khẩu của admin thành công.";
                    }
                    else
                    {
                        ViewData["error"] = "Không tìm thấy tài khoản admin.";
                    }
                }
                else
                {
                    studentAccount = HttpContext.Session.GetObjectFromJson<Student>("Student");
                    if (studentAccount != null)
                    {
                        studentAccount.StudentPassword = newPassword; // Edit password here
                        _context.Students.Update(studentAccount);
                        await _context.SaveChangesAsync();
                        ViewData["error"] = "Reset mật khẩu của student thành công.";
                    }
                    else
                    {
                        ViewData["error"] = "Không tìm thấy tài khoản student.";
                    }
                }
                return Page();
                //return RedirectToPage("/Login");
            }
        }
    }
}
