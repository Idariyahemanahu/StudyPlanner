using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.EntityFrameworkCore;
using CreateDbFromScratch.Model;
namespace MyApp.Namespace
{
    public class AddSubjectModel : PageModel
    {

        private readonly AppDbContext _context;


        [BindProperty]
        public Subject NewSubject { get; set; } = null!;

        public AddSubjectModel(AppDbContext context)
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
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                ModelState.AddModelError("", "Session expired or user not logged in.");
                return Page();
            }

            // use userId
            bool Exists = await _context.Subjects.AnyAsync(s => s.SubjectName == NewSubject.SubjectName && s.Id == UserId);
            if (Exists)
            {
                ModelState.AddModelError(string.Empty, "Subject already exists.");
                return Page();
            }
            NewSubject.Id = UserId.Value; // Set the user ID for the new subject
            _context.Subjects.Add(NewSubject);
            await _context.SaveChangesAsync();
            return RedirectToPage("/test/Test");
        }
    }
}
