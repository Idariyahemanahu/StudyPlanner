using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{

    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a subject name.")]
        public string SubjectName { get; set; } = string.Empty;
        public int AssignmentCount { get; set; } = 0;
        public int Total_Practicals { get; set; } = 0;
        public int Remaining_Practicals { get; set; } = 0;
    }
}
