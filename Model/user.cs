using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        // --- Username Validation ---
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be 2–20 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$",
            ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens.")]
        public string Name { get; set; } = string.Empty;
        //Email Validation
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
        // --- Password Validation ---
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}


