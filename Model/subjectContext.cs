using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{
    internal class SubjectContext : DbContext
    {
        public SubjectContext(DbContextOptions<SubjectContext> options) : base(options)
        {
        }
        public DbSet<Subject> Subjects { get; set; }
    }
}
