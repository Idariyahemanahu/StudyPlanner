using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear the session variable to log the user out
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId"); // <-- Clear both

            // Redirect the user to the Index page
            return RedirectToPage("/Index");
        }
    }
}
