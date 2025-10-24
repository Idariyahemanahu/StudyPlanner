using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateDbFromScratch.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> OnPostAsync(string Email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No user found.");
                return Page();
            }
            var Hasher = new PasswordHasher<User>();
            var result = Hasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                //valid credentials
                return RedirectToPage("/Index");
            }
            else
            {
                //invalid credentials
                ModelState.AddModelError(string.Empty, "Invalid Credentials.");
                return Page();
            }
        }

    }
}
