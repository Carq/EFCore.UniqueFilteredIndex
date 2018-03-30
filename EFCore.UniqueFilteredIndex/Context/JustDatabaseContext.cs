using EFCore.UniqueFilteredIndex.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCore.UniqueFilteredIndex.Context
{
    public class JustDatabaseContext : DbContext
    {
        public JustDatabaseContext(DbContextOptions<JustDatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// Workaround
            /// Removing this code after migration creation will solve the problem
            modelBuilder
              .Entity<User>()
              .HasIndex("Email", "Login")
              .HasFilter("IsActive = 1")
              .IsUnique();
        }
    }
}
