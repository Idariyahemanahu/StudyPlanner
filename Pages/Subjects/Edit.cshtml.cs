using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public SubjectInput Input { get; set; } = new();

        // This Input class is now updated
        public class SubjectInput
        {
            public int Id { get; set; }
            [Required]
            [StringLength(100)]
            public string Name { get; set; } = null!;

            [StringLength(100)]
            public string? FacultyName { get; set; }

            [Range(1, 10)]
            public int? Semester { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId.Value);

            if (subject == null)
            {
                return NotFound();
            }

            // Load the correct fields from the database
            Input = new SubjectInput
            {
                Id = subject.Id,
                Name = subject.Name,
                FacultyName = subject.FacultyName,
                Semester = subject.Semester
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

            var subjectToUpdate = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Id == Input.Id && s.UserId == userId.Value);

            if (subjectToUpdate == null)
            {
                return NotFound();
            }

            // Save the correct fields to the database
            subjectToUpdate.Name = Input.Name;
            subjectToUpdate.FacultyName = Input.FacultyName;
            subjectToUpdate.Semester = Input.Semester;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}