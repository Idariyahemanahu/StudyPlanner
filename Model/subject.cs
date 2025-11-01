using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace CreateDbFromScratch.Model
{

    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Please enter a subject name.")]
        public string SubjectName { get; set; } = string.Empty;
        [ForeignKey("User")]
        public int Id { get; set; }

        public User? User { get; set; }
        public ICollection<Work> Works { get; set; } = new List<Work>();
    }
}
