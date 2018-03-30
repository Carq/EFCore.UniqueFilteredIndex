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
            modelBuilder
              .Entity<User>()
              .HasIndex("Email", "Login")
              .HasFilter("IsActive = 1")
              .IsUnique();
        }
    }
}
