using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Namespace
{
    public class WorkIndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public WorkIndexModel(AppDbContext context) { _context = context; }

        public Subject? Subject { get; set; }
        public List<Work> WorkItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int subjectId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            Subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Id == subjectId && s.UserId == userId.Value);

            if (Subject == null)
            {
                return NotFound();
            }

            WorkItems = await _context.Works
                .Where(w => w.SubjectId == subjectId)
                .OrderBy(w => w.DueDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, int subjectId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            var work = await _context.Works
                .Include(w => w.Subject)
                .FirstOrDefaultAsync(w => w.Id == id && w.Subject.UserId == userId.Value);

            if (work != null)
            {
                _context.Works.Remove(work);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { subjectId = subjectId });
        }
    }
}