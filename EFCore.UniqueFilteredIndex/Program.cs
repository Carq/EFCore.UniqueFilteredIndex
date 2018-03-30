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
            Console.WriteLine("Hello World!");
            InitializeDiContainer();

            var context = _container.Resolve<JustDatabaseContext>();
            context.Database.Migrate();

           

            context.Users.Add(new Model.User { IsActive = false, Login = "2", Email = "John@wp.pl" });
            context.Users.Add(new Model.User { IsActive = false, Login = "2", Email = "John@wp.pl" });
            
            context.SaveChanges();

            context.Users.Add(new Model.User { IsActive = true, Login = "2", Email = "John@wp.pl" });
            context.SaveChanges();

            var test = context.Users.Where(x => x.IsActive == false);

            context.RemoveRange(test);
            context.SaveChanges();

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

        //migrationBuilder.CreateIndex(
        //     name: "IX_Users_Login_Email",
        //       table: "Users",
        //       columns: new[] { "Login", "Email" },
        //       unique: true,
        //       filter: "IsActive = 1");
    }
}
