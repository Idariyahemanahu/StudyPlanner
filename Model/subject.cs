using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CreateDbFromScratch.Model
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FacultyName { get; set; } 
        [Range(1, 10)]
        public int? Semester { get; set; } 

        // Foreign key to User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Work> Works { get; set; } = new List<Work>();
    }
}