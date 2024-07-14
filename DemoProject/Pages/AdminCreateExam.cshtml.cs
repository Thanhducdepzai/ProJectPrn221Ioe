using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIoePrn.Models;

namespace ProjectIoePrn.Pages
{
    public class AdminCreateExamModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public AdminCreateExamModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Admin Admin { get; set; }

        public IActionResult OnGet(int? AdminId)
        {
            if (AdminId == null)
            {
                return NotFound();
            }

            Admin = _context.Admins.FirstOrDefault(a => a.AdminId == AdminId);

            if (Admin == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
