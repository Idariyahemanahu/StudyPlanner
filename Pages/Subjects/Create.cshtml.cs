using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MyApp.Namespace
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public SubjectInput Input { get; set; } = new();

        // This Input class is now updated to match your Subject.cs
        public class SubjectInput
        {
            [Required]
            [StringLength(100)]
            public string Name { get; set; } = null!;

            [StringLength(100)]
            public string? FacultyName { get; set; }

            [Range(1, 10)]
            public int? Semester { get; set; }
        }

        public void OnGet() { }

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

            // This now saves the correct fields
            var subject = new Subject
            {
                Name = Input.Name,
                FacultyName = Input.FacultyName,
                Semester = Input.Semester,
                UserId = userId.Value
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}