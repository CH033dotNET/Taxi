namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.MainContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Roles.Add(
                new Model.Role { 
                    Id = 1,
                    Name = "SuperAdmin",
                    Description = "SomeTXT"
            });
            context.SaveChanges();

            context.Users.Add(
                new Model.User {
                    Id = 1,
                    Password = "pass",
                    Email = "ema",
                    RoleId = 1,
                    UserName = "testt"
                }
                );
            context.SaveChanges();
        }
    }
}
