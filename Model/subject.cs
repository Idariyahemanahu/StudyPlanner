using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{

    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Please enter a subject name.")]
        public string SubjectName { get; set; } = string.Empty;

        public int Id { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Work> Works { get; set; } = new List<Work>();
    }
}
