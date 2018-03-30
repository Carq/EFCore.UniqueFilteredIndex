using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore.UniqueFilteredIndex.Context
{
    public class DbInitalizer : IDesignTimeDbContextFactory<JustDatabaseContext>
    {
        public JustDatabaseContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<JustDatabaseContext>()
                                .UseSqlServer("Server=.\\SQLEXPRESS;Database=JustDatabase;Trusted_Connection=True")
                                .Options;

            return new JustDatabaseContext(options);
        }
    }
}
