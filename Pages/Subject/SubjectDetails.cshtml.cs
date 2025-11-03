using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateDbFromScratch.Model;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Namespace
{
    public class SubjectDetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public SubjectDetailsModel(AppDbContext context)
        {
            _context = context;
        }
        public Subject? Subject { get; set; }
        public List<Work> Works { get; set; } = new();
        
        public async Task<IActionResult> OnGetAsync(int SubjectId)
        {
            Subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.SubjectId == SubjectId);

            if (Subject == null)
                return NotFound();
            // Additional logic to fetch and display subject details can be added here
            Works = await _context.Works
                .Where(w => w.SubjectId == SubjectId)
                .ToListAsync();
            return Page();
        }

    }
}
