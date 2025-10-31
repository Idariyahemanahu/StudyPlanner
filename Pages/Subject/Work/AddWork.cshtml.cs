using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace MyApp.Namespace
{
    public class AddWorkModel : PageModel
    {
        private readonly AppDbContext _context;


        [BindProperty]
        public Work NewWork { get; set; } = null!;
        public SelectList WorkStatusList { get; set; } = default!;
        public SelectList DateTypeList { get; set; } = default!;
        public SelectList SubjectList { get; set; } = default!;
        public AddWorkModel(AppDbContext context)
        {
            _context = context;
            WorkStatusList = new SelectList(Enum.GetValues(typeof(WorkStatus)));
            DateTypeList = new SelectList(Enum.GetValues(typeof(DateType)));
        }
        public void OnGet()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                ModelState.AddModelError("", "Session expired or user not logged in.");
                return;
            }
            var Subjects = _context.Subjects
             .Where(s => s.Id == UserId)
             .Select(s => new { s.SubjectId, s.SubjectName })
             .ToList();

            // Convert to SelectList for dropdown
            SubjectList = new SelectList(Subjects, "SubjectId", "SubjectName");
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
            bool Exists = await _context.Works.AnyAsync(w => w.WorkName == NewWork.WorkName && w.Id == UserId);
            if (Exists)
            {
                ModelState.AddModelError(string.Empty, "Work  already exists.");
                return Page();
            }
            NewWork.Id = UserId.Value; // Set the user ID for the new work
            _context.Works.Add(NewWork);
            await _context.SaveChangesAsync();
            return RedirectToPage("/test/Test");
        }
    }
}
