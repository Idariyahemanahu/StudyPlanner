using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CreateDbFromScratch.Model;
namespace MyApp.Namespace
{
    public class AddWorkModel : PageModel
    {
        private readonly AppDbContext _context;
        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public int SubjectId { get; set; }

        [BindProperty]
        public Subject NewWork { get; set; } = null!;
        public AddWorkModel(AppDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Index");
        }
    }
}
