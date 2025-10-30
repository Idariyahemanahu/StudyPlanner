
using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{
    public class Work
    {
        [Key]
        public int WorkId { get; set; }
        [Required(ErrorMessage = "Name is required.")]

        public string WorkName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]

        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; } = DateTime.Now;

        // Foreign key to Subject
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }
}


