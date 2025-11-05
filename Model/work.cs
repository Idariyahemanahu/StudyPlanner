using System.ComponentModel.DataAnnotations;
using System;

namespace CreateDbFromScratch.Model
{
    public enum WorkType
    {
        Assignment,
        Quiz,
        Practical,
        Exam,
        Other
    }

    public enum WorkStatus
    {
        [Display(Name = "Not Started")]
        NotStarted,
        [Display(Name = "In Progress")]
        InProgress,
        Completed
    }

    public class Work
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public WorkType Type { get; set; }

        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [Required]
        public WorkStatus Status { get; set; }

        // Foreign key to Subject
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }
}