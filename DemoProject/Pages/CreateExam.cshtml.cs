using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;
using System.Threading.Tasks;

namespace ProjectIoePrn.Pages
{
    public class CreateExamModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;

        public CreateExamModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Admin Admin { get; set; }

        public async Task OnGetAsync(int adminId)
        {
            Admin = await _context.Admins
                                  .FirstOrDefaultAsync(a => a.AdminId == adminId);

            if (Admin == null)
            {
                // Handle the case when admin is not found
                NotFound();
            }
        }
    }
}
