using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
namespace MyApp.Namespace
{
    public class TodoModel : PageModel
    {
        private readonly AppDbContext _context;

        public TodoModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Subject> Subjects { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Load all subjects with their related works
            Subjects = await _context.Subjects
                .Include(s => s.Works)
                .ToListAsync();
            return Page();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
