using Microsoft.EntityFrameworkCore;

namespace CreateDbFromScratch.Model
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Work> Works { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subjects)
                .HasForeignKey(s => s.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Work>()
                .HasOne(w => w.Subject)
                .WithMany(s => s.Works)
                .HasForeignKey(w => w.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}