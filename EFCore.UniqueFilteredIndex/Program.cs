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
            UpdateTwoEntitiesWithFilteredIndex(context);
            RemoveTwoEntitiesWithFilteredIndex(context);
            // CleanupDatabase(context);
            Console.ReadKey();
        }

        private static void UpdateTwoEntitiesWithFilteredIndex(JustDatabaseContext context)
        {
            Console.WriteLine("Updating entities...");
            var entitiesToUpdate = context.Users.Where(x => x.IsActive == false);
            int i = 0;
            foreach (var entity in entitiesToUpdate)
            {
                entity.Login = "NewLogin_" + i++;
            }

            SaveChangesAndWriteToConsoleException(context);
        }

        private static void RemoveTwoEntitiesWithFilteredIndex(JustDatabaseContext context)
        {
            Console.WriteLine("Deleting entities...");
            var entitiesToDelete = context.Users.Where(x => x.IsActive == false);
            context.RemoveRange(entitiesToDelete);

            SaveChangesAndWriteToConsoleException(context);
        }

        private static void CreateTwoTheSameEntitiesWhichDoNotMetFilterConditions(JustDatabaseContext context)
        {
            context.Users.Add(new Model.User { IsActive = false, Login = "Login1", Email = "Jan@Kowalski.pl" });
            context.Users.Add(new Model.User { IsActive = false, Login = "Login1", Email = "Jan@Kowalski.pl" });
            context.SaveChanges();
        }

        private static void CleanupDatabase(JustDatabaseContext context)
        {
            context.RemoveRange(context.Users);
            context.SaveChanges();
        }

        private static void SaveChangesAndWriteToConsoleException(JustDatabaseContext context)
        {
            try
            {
                // The problem occurs during saving changes
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private static void InitializeDiContainer()
        {
            var containerBuilder = new ContainerBuilder();
            InitializeDatabaseContext(containerBuilder);
            _container = containerBuilder.Build();
        }

        private static JustDatabaseContext MigrateDatabase()
        {
            var context = _container.Resolve<JustDatabaseContext>();
            context.Database.Migrate();
            return context;
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
