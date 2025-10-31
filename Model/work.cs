
using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{
    public enum DateMeaning
    {
        [Display(Name = "Exam")]
        Exam,
        [Display(Name = "Due Date")]
        DueDate,
        [Display(Name = "Important Date")]
        ImportantDate,
    }
    public enum WorkStatus
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Overdue")]
        Overdue
    }
    public class Work
    {
        [Key]
        public int WorkId { get; set; }
        [Required(ErrorMessage = "Work Name is required.")]

        public string WorkName { get; set; } = string.Empty;

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;

        public DateMeaning DateMeaning { get; set; }

        public WorkStatus WorkStatus { get; set; } = WorkStatus.Pending;
        // Foreign key to Subject
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        // Foreign key to User
        public int Id { get; set; }
        public User User { get; set; } = null!;
    }
}


