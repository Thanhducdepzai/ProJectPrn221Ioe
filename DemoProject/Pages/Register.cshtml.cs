using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace DemoProject.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly DemoProject.Models.IOE_Project_Clone_PRN221Context _context;
        private readonly ILogger<RegisterModel> _logger;
        public RegisterModel (DemoProject.Models.IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        } 
        public void OnGet()
        {
        }
        public Student student { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {         
            if (!ModelState.IsValid|| _context.Students == null ||  student == null)
            {
                
                return Page();
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Login");
        }
    }
}
