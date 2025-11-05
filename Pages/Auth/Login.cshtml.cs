using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateDbFromScratch.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks; // Make sure this is included

namespace MyApp.Namespace
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        //property bindings for email and password
        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Just shows the login page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (User == null)
            {
                ModelState.AddModelError(string.Empty, "No user found.");
                return Page();
            }

            var Hasher = new PasswordHasher<User>();
            var result = Hasher.VerifyHashedPassword(User, User.Password, Password);

            if (result == PasswordVerificationResult.Success)
            {
                //valid credentials
                HttpContext.Session.SetString("UserName", User.Name);

                // --- THIS IS THE REQUIRED UPDATE ---
                // We save the ID to know whose data to load
                HttpContext.Session.SetInt32("UserId", User.Id);

                return RedirectToPage("/dashboard");
            }
            else
            {
                //invalid credentials
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return Page();
            }
        }
    }
}