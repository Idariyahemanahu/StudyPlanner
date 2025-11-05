using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace MyApp.Namespace
{
    public class WorkeditModel : PageModel
    {
        private readonly AppDbContext _context;
        public WorkeditModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public WorkInput Input { get; set; } = new();

        public class WorkInput
        {
            public int Id { get; set; }
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
            public WorkStatus Status { get; set; }

            [Required]
            public DateTime DueDate { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            var work = await _context.Works
                .Include(w => w.Subject)
                .FirstOrDefaultAsync(w => w.Id == id && w.Subject.UserId == userId.Value);

            if (work == null)
            {
                return NotFound();
            }

            Input = new WorkInput
            {
                Id = work.Id,
                SubjectId = work.SubjectId,
                Title = work.Title,
                Description = work.Description, // <-- This line now works
                Type = work.Type,
                Status = work.Status,
                DueDate = work.DueDate
            };
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
                return Page();
            }
            var workToUpdate = await _context.Works
                .Include(w => w.Subject)
                .FirstOrDefaultAsync(w => w.Id == Input.Id && w.Subject.UserId == userId.Value);
            if (workToUpdate == null)
            {
                return NotFound();
            }
            workToUpdate.Title = Input.Title;
            workToUpdate.Description = Input.Description; // <-- This line now works
            workToUpdate.Type = Input.Type;
            workToUpdate.Status = Input.Status;
            workToUpdate.DueDate = Input.DueDate;
            await _context.SaveChangesAsync();

            return RedirectToPage("./WorkIndex", new { subjectId = workToUpdate.SubjectId });
        }
    }
}