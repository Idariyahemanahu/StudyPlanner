
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{
    public enum DateType
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

        public DateType DateType { get; set; }

        public WorkStatus WorkStatus { get; set; } = WorkStatus.Pending;
        // Foreign key to Subject

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public Subject? Subject { get; set; }

        // Foreign key to User

        [ForeignKey("User")]
        public int Id { get; set; }
        public User? User { get; set; }
    }
}


