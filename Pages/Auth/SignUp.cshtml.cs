using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateDbFromScratch.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
namespace MyApp.Namespace
{
    public class SignUpModel : PageModel
    {
        private readonly UserContext _context;

        [BindProperty]
        public User NewUser { get; set; } = null!;

        public SignUpModel(UserContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid form submission.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == NewUser.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return Page();
            }
            var Hasher = new PasswordHasher<User>();
 
            var HashedPassword = Hasher.HashPassword(NewUser, NewUser.Password);

            NewUser.Password = HashedPassword;

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", NewUser.Name);
            return RedirectToPage("/Index");
        }
    }
}
