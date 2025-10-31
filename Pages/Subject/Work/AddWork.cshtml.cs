using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using CreateDbFromScratch.Model;
namespace MyApp.Namespace
{
    public class AddWorkModel : PageModel
    {
        private readonly AppDbContext _context;


        [BindProperty]
        public Work NewWork { get; set; } = null!;
        public AddWorkModel(AppDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                ModelState.AddModelError("", "Session expired or user not logged in.");
                return Page();
            }
            bool Exists = await _context.Works.AnyAsync(w => w.WorkName == NewWork.WorkName && w.Id == UserId);
            if (Exists)
            {
                ModelState.AddModelError(string.Empty, "Work  already exists.");
                return Page();
            }
            _context.Works.Add(NewWork);
            await _context.SaveChangesAsync();
            return RedirectToPage("/test/Test");
        }
    }
}
