using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateDbFromScratch.Model;
namespace MyApp.Namespace
{
    public class LoginModel : PageModel
    {
        private readonly UserContext _context;
        
       
        public LoginModel(UserContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost(string Email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.Password == password);
            if (user != null)
            {
                // Authentication successful
                return RedirectToPage("/Index");
            }
            else
            {
                // Authentication failed
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

    }
}
