using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace CreateDbFromScratch.Model
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
