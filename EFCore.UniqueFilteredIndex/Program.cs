using Autofac;
using EFCore.UniqueFilteredIndex.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCore.UniqueFilteredIndex
{
    class Program
    {
        private static IContainer _container;

        static void Main(string[] args)
        {
            InitializeDiContainer();
            JustDatabaseContext context = MigrateDatabase();

            CreateTwoTheSameEntitiesWhichDoNotMetFilterConditions(context);
            RemoveTwoEntitiesWithFilteredIndex(context);
            CleanupDatabase(context);
        }

        private static void RemoveTwoEntitiesWithFilteredIndex(JustDatabaseContext context)
        {
            var entitiesToDelete = context.Users.Where(x => x.IsActive == false);
            context.RemoveRange(entitiesToDelete);

            // The problem occurs during saving changes
            context.SaveChanges();
        }

        private static void CreateTwoTheSameEntitiesWhichDoNotMetFilterConditions(JustDatabaseContext context)
        {
            context.Users.Add(new Model.User { IsActive = false, Login = "2", Email = "Jan@Kowalski.pl" });
            context.Users.Add(new Model.User { IsActive = false, Login = "2", Email = "Jan@Kowalski.pl" });
            context.SaveChanges();
        }

        private static JustDatabaseContext MigrateDatabase()
        {
            var context = _container.Resolve<JustDatabaseContext>();
            context.Database.Migrate();
            return context;
        }

        private static void CleanupDatabase(JustDatabaseContext context)
        {
            context.RemoveRange(context.Users);
            context.SaveChanges();
        }

        private static void InitializeDiContainer()
        {
            var containerBuilder = new ContainerBuilder();
            InitializeDatabaseContext(containerBuilder);
            _container = containerBuilder.Build();
        }

        private static void InitializeDatabaseContext(ContainerBuilder builder)
        {
            var options = new DbContextOptionsBuilder<JustDatabaseContext>()
                                .UseSqlServer("Server=.\\SQLEXPRESS;Database=JustDatabase;Trusted_Connection=True")
                                .Options;

            builder.Register(c => options).As<DbContextOptions<JustDatabaseContext>>();
            builder.RegisterType<JustDatabaseContext>();
            builder.RegisterType<DbInitalizer>();
        }
    }
}
