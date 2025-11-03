using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ViewSubjectModel : PageModel
    {
        private readonly AppDbContext _context;
        public ViewSubjectModel(AppDbContext context)
        {
            _context = context;
        }
        
        public List<Subject> Subjects { get; set; } = new();
        
        public async Task<IActionResult> OnGetAsync()
        {
            // Load all subjects with their related works
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                ModelState.AddModelError("", "Session expired or user not logged in.");
                return Page();
            }

            Subjects = await _context.Subjects
                .Where(s => s.Id == UserId)
                .ToListAsync();
            return Page();
        }

        
    }
}
