using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    public class WorkCreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public WorkCreateModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public WorkInput Input { get; set; } = new();
        public string SubjectName { get; set; } = string.Empty;

        public class WorkInput
        {
            [Required]
            public int SubjectId { get; set; }
            [Required]
            [StringLength(150)]
            public string Title { get; set; } = null!;

            // --- THIS PROPERTY WAS MISSING ---
            [StringLength(1000)]
            public string? Description { get; set; }

            [Required]
            public WorkType Type { get; set; }
            [Required]
            public DateTime DueDate { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int subjectId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Id == subjectId && s.UserId == userId.Value);

            if (subject == null)
            {
                return NotFound();
            }

            SubjectName = subject.Name;
            Input.SubjectId = subject.Id;
            Input.DueDate = DateTime.Now.AddDays(7);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            if (!ModelState.IsValid)
            {
                var subject = await _context.Subjects.FindAsync(Input.SubjectId);
                SubjectName = subject?.Name ?? "Work";
                return Page();
            }

            var subjectCheck = await _context.Subjects
                .AnyAsync(s => s.Id == Input.SubjectId && s.UserId == userId.Value);

            if (!subjectCheck)
            {
                return Forbid();
            }

            var newWork = new Work
            {
                Title = Input.Title,
                Description = Input.Description, // <-- This line now works
                Type = Input.Type,
                DueDate = Input.DueDate,
                SubjectId = Input.SubjectId,
                Status = WorkStatus.NotStarted
            };

            _context.Works.Add(newWork);
            await _context.SaveChangesAsync();

            return RedirectToPage("./WorkIndex", new { subjectId = Input.SubjectId });
        }
    }
}