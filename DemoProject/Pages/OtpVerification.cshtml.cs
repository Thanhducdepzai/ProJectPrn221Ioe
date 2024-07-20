using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoProject.Pages
{
    public class OtpVerificationModel : PageModel
    {
        private readonly ILogger<OtpVerificationModel> _logger;
        private readonly IOE_Project_Clone_PRN221Context _context;

        public OtpVerificationModel(ILogger<OtpVerificationModel> logger, IOE_Project_Clone_PRN221Context context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public string OtpCode { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var otp = TempData["OtpCode"] as string;
            if (OtpCode == otp)
            {
                TempData["OtpVerified"] = true;
                return RedirectToPage("/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Otp không khớp");
                return Page();
            }
        }
    }
}
