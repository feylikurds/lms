namespace LMS.Migrations
{
    using FizzWare.NBuilder;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
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

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!context.Roles.Any(r => r.Name == "Teacher"))
            {
                var role = new IdentityRole { Name = "Teacher" };

                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Student"))
            {
                var role = new IdentityRole { Name = "Student" };

                roleManager.Create(role);
            }

            var rand = new Random();
            var noCourse = new Course();

            noCourse.Name = "None";
            noCourse.Description = "None";
            noCourse.StartDate = new DateTime(2000, 1, 1);
            noCourse.EndDate = new DateTime(2001, 12, 12);

            context.Courses.AddOrUpdate(noCourse);

            var courses1 = Builder<Course>.CreateListOfSize(5).All()
                .With(c => c.Name = Faker.Company.Name())
                .With(c => c.Description = Faker.Company.CatchPhrase())
                .With(c => c.StartDate = new DateTime(2017, 1, 1))
                .With(c => c.EndDate = new DateTime(2017, 6, 30))
                .Build();

            context.Courses.AddOrUpdate(c => c.Id, courses1.ToArray());

            var courses2 = Builder<Course>.CreateListOfSize(5).All()
                .With(c => c.Name = Faker.Company.Name())
                .With(c => c.Description = Faker.Company.CatchPhrase())
                .With(c => c.StartDate = new DateTime(2017, 7, 1))
                .With(c => c.EndDate = new DateTime(2017, 12, 30))
                .Build();

            context.Courses.AddOrUpdate(c => c.Id, courses2.ToArray());

            var modules1 = Builder<LMS.Models.Module>.CreateListOfSize(10).All()
                .With(m => m.Name = Faker.Company.Name())
                .With(m => m.Description = Faker.Company.CatchPhrase())
                .With(m => m.StartDate = new DateTime(2017, 1, 1))
                .With(m => m.EndDate = new DateTime(2017, 6, 30))
                .With(m => m.Course = courses1.ElementAt(rand.Next(0, courses1.Count())))
                .Build();

            context.Modules.AddOrUpdate(m => m.Id, modules1.ToArray());

            var modules2 = Builder<LMS.Models.Module>.CreateListOfSize(10).All()
                .With(m => m.Name = Faker.Company.Name())
                .With(m => m.Description = Faker.Company.CatchPhrase())
                .With(m => m.StartDate = new DateTime(2017, 7, 1))
                .With(m => m.EndDate = new DateTime(2017, 12, 30))
                .With(m => m.Course = courses2.ElementAt(rand.Next(0, courses2.Count())))
                .Build();

            context.Modules.AddOrUpdate(m => m.Id, modules2.ToArray());

            

            //Activities
            var activities1 = Builder<Activity>.CreateListOfSize(20).All()
                .With(a => a.Name = Faker.Company.Name())
                .With(a => a.Description = Faker.Company.CatchPhrase())
                .With(a => a.StartDate = new DateTime(2017, 1, 1))
                .With(a => a.EndDate = new DateTime(2017, 6, 30))
                .With(a => a.Module = modules1.ElementAt(rand.Next(0, modules1.Count())))
                .Build();

            context.Activities.AddOrUpdate(a => a.Id, activities1.ToArray());

            var activities2 = Builder<Activity>.CreateListOfSize(20).All()
                .With(a => a.Name = Faker.Company.Name())
                .With(a => a.Description = Faker.Company.CatchPhrase())
                .With(a => a.StartDate = new DateTime(2017, 7, 1))
                .With(a => a.EndDate = new DateTime(2017, 12, 30))
                .With(a => a.Module = modules2.ElementAt(rand.Next(0, modules2.Count())))
                .Build();

            context.Activities.AddOrUpdate(a => a.Id, activities2.ToArray());


            


            context.SaveChanges();

            //Add an module to courses that doesn't have one
            var modulelessCourses = context.Courses.Where(c => c.Modules.Count() == 0).ToList();
            foreach (var item in modulelessCourses)
            {
                var module = Builder<Module>.CreateNew()
                .With(m => m.Name = Faker.Company.Name())
                .With(m => m.Description = Faker.Company.CatchPhrase())
                .With(m => m.StartDate = new DateTime(2017, 1, 1))
                .With(m => m.EndDate = new DateTime(2017, 6, 30))
                .With(m => m.Course = item)
                .Build();
                context.Modules.AddOrUpdate(c => c.Id, module);
            }
            context.SaveChanges();

            //Add an activity to a module that did not have any
            var activitylessModules = context.Modules.Where(c => c.Activities.Count() == 0).ToList();
            foreach (var item in activitylessModules)
            {
                var activity = Builder<Activity>.CreateNew()
                .With(m => m.Name = Faker.Company.Name())
                .With(m => m.Description = Faker.Company.CatchPhrase())
                .With(m => m.StartDate = new DateTime(2017, 1, 1))
                .With(m => m.EndDate = new DateTime(2017, 6, 30))
                .With(m => m.Module = item)
                .Build();
                context.Activities.AddOrUpdate(c => c.Id, activity);
            }
            context.SaveChanges();

            //Users
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (!context.Users.Any(u => u.UserName == "teacher@localhost.com"))
            {
                var user = new ApplicationUser { UserName = "teacher@localhost.com", FirstName = "Bob", LastName = "Bobson", Email = "teacher@localhost.com", CourseId = noCourse.Id };

                userManager.Create(user, "Pass.123");
                userManager.AddToRole(user.Id, "Teacher");
            }

            var courses = (from c in context.Courses
                           where c.Name != "None" && c.Modules.Count() > 0
                           select c).ToList();

            SetUpUser(context, userManager, courses, rand, "student@localhost.com", "Jill", "Jillson", "student@localhost.com", courses.FirstOrDefault().Id);

            context.SaveChanges();

            //Some additional users
            for (int i = 0; i < courses.Count*3; i++)
            {
                var firstName = Faker.Name.First();
                var lastName = Faker.Name.Last();
                var email = firstName.ToLower() + "." + lastName.ToLower() + "@localhost.com";

                SetUpUser(context, userManager, courses, rand, email, firstName, lastName, email, courses.ElementAt(i % courses.Count).Id);
            }
        }

        private void SetUpUser(LMS.Models.ApplicationDbContext context, UserManager<ApplicationUser> userManager, List<Course>courses, Random rand, string userName, string firstName, string lastName, string email, int courseId)
        {
            if (!context.Users.Any(u => u.UserName == userName))
            {
                var course = courses.Where(c => c.Id == courseId).First();
                var user = new ApplicationUser { UserName = userName, FirstName = firstName, LastName = lastName, Email = email, CourseId = course.Id };

                userManager.Create(user, "Pass.123");
                context.SaveChanges();

                var us = (from u in context.Users
                          where u.UserName == userName
                          select u).FirstOrDefault();

                userManager.AddToRole(us.Id, "Student");

                var activities = (from m in context.Modules
                                  where m.CourseId == course.Id
                                  from a in context.Activities
                                  where a.ModuleId == m.Id
                                  select a).ToList();
                var studentActivities = new List<StudentActivity>();

                foreach (var a in activities)
                {
                    var sa = new StudentActivity();

                    sa.StudentId = us.Id;
                    sa.ActivityId = a.Id;

                    studentActivities.Add(sa);
                }

                context.StudentActivities.AddOrUpdate(a => a.StudentId, studentActivities.ToArray());
                context.SaveChanges();
            }
        }
    }
}
