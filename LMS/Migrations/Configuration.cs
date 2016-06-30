namespace LMS.Migrations
{
    using FizzWare.NBuilder;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext context)
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

            if (!context.Roles.Any(r => r.Name == "Teacher"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Teacher" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Student"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Student" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "teacher@localhost.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { AssignedRole = "Teacher", UserName = "teacher@localhost.com", FirstName = "Bob", LastName = "Bobson" };

                manager.Create(user, "Pass.123");
                manager.AddToRole(user.Id, "Teacher");
            }

            if (!context.Users.Any(u => u.UserName == "student@localhost.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { AssignedRole = "Student", UserName = "student@localhost.com", FirstName = "Jill", LastName = "Jillson" };

                manager.Create(user, "Pass.123");
                manager.AddToRole(user.Id, "Student");
            }

            var rand = new Random();

            var courses1 = Builder<Course>.CreateListOfSize(5).All()
                .With(c => c.Name = Faker.Company.Name())
                .With(c => c.Description = Faker.Company.CatchPhrase())
                .With(c => c.StartDate = new DateTime(2016, 1, 1))
                .With(c => c.EndDate = new DateTime(2016, 6, 30))
                .Build();

            context.Courses.AddOrUpdate(c => c.Id, courses1.ToArray());

            var courses2 = Builder<Course>.CreateListOfSize(5).All()
                .With(c => c.Name = Faker.Company.Name())
                .With(c => c.Description = Faker.Company.CatchPhrase())
                .With(c => c.StartDate = new DateTime(2016, 7, 1))
                .With(c => c.EndDate = new DateTime(2016, 12, 30))
                .Build();

            context.Courses.AddOrUpdate(c => c.Id, courses2.ToArray());

            var modules1 = Builder<Module>.CreateListOfSize(10).All()
                .With(m => m.Name = Faker.Company.Name())
                .With(m => m.Description = Faker.Company.CatchPhrase())
                .With(m => m.StartDate = new DateTime(2016, 1, 1))
                .With(m => m.EndDate = new DateTime(2016, 6, 30))
                .With(m => m.Course = courses1.ElementAt(rand.Next(0, courses1.Count())))
                .Build();

            context.Modules.AddOrUpdate(m => m.Id, modules1.ToArray());

            var modules2 = Builder<Module>.CreateListOfSize(10).All()
                .With(m => m.Name = Faker.Company.Name())
                .With(m => m.Description = Faker.Company.CatchPhrase())
                .With(m => m.StartDate = new DateTime(2016, 7, 1))
                .With(m => m.EndDate = new DateTime(2016, 12, 30))
                .With(m => m.Course = courses2.ElementAt(rand.Next(0, courses2.Count())))
                .Build();

            context.Modules.AddOrUpdate(m => m.Id, modules2.ToArray());

            var activities1 = Builder<Activity>.CreateListOfSize(20).All()
                .With(a => a.Name = Faker.Company.Name())
                .With(a => a.Description = Faker.Company.CatchPhrase())
                .With(a => a.StartDate = new DateTime(2016, 1, 1))
                .With(a => a.EndDate = new DateTime(2016, 6, 30))
                .With(a => a.Module = modules1.ElementAt(rand.Next(0, modules1.Count())))
                .Build();

            context.Activities.AddOrUpdate(a => a.Id, activities1.ToArray());

            var activities2 = Builder<Activity>.CreateListOfSize(20).All()
                .With(a => a.Name = Faker.Company.Name())
                .With(a => a.Description = Faker.Company.CatchPhrase())
                .With(a => a.StartDate = new DateTime(2016, 7, 1))
                .With(a => a.EndDate = new DateTime(2016, 12, 30))
                .With(a => a.Module = modules2.ElementAt(rand.Next(0, modules2.Count())))
                .Build();

            context.Activities.AddOrUpdate(a => a.Id, activities2.ToArray());
        }
    }
}
