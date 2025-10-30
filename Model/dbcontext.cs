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
            // Subject → User (many-to-one)
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subjects)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Work → Subject (many-to-one)
            modelBuilder.Entity<Work>()
                .HasOne(w => w.Subject)
                .WithMany(s => s.Works)
                .HasForeignKey(w => w.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Work → User (many-to-one)
            modelBuilder.Entity<Work>()
                .HasOne(w => w.User)
                .WithMany(u => u.Works)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enum conversions
            modelBuilder.Entity<Work>()
                .Property(w => w.DateMeaning)
                .HasConversion<string>();

            modelBuilder.Entity<Work>()
                .Property(w => w.WorkStatus)
                .HasConversion<string>();
        }
    }
}
