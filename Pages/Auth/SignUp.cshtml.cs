using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateDbFromScratch.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace MyApp.Namespace
{
    public class SignUpModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public User NewUser { get; set; } = null!;
        private readonly string[] allowedDomains =
        {
            "gmail.com",
            "yahoo.com",
            "outlook.com",
            "hotmail.com",
            "live.com",
            "icloud.com",
            "me.com",
            "mac.com"
        };


        public SignUpModel(AppDbContext context)
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
            NewUser.Email = (NewUser.Email?.Trim().ToLower()) ?? string.Empty;
            //var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == NewUser.Email);
            bool existingUser = await _context.Users.AnyAsync(u => u.Email == NewUser.Email);
            if (existingUser)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return Page();
            }
            var domain = NewUser.Email.Split('@').Last();
            if (!allowedDomains.Contains(domain))
            {
                ModelState.AddModelError("NewUser.Email", $"Only {string.Join(", ", allowedDomains)} emails are allowed.");
                return Page();
            }
            var Hasher = new PasswordHasher<User>();
            var HashedPassword = Hasher.HashPassword(NewUser, NewUser.Password);

            NewUser.Password = HashedPassword;

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", NewUser.Name);
            HttpContext.Session.SetInt32("UserId", NewUser.Id);
            return RedirectToPage("/Index");
        }
    }
}
