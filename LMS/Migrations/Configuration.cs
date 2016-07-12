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
            var noCourse = new Course();

            noCourse.Name = "None";
            noCourse.Description = "None";
            noCourse.StartDate = new DateTime(2000, 1, 1);
            noCourse.EndDate = new DateTime(2001, 12, 31);

            context.Courses.AddOrUpdate(noCourse);

            var courses = new List<Course>();

            courses.Add(new Course
            {
                Name = "NETASP1701",
                Description = ".NET ASP.NET 2017 Spring",
                StartDate = new DateTime(2017, 1, 1),
                EndDate = new DateTime(2017, 6, 30),
            });

            courses.Add(new Course
            {
                Name = "JAVAEE1701",
                Description = "Java EE 2017 Spring",
                StartDate = new DateTime(2017, 1, 1),
                EndDate = new DateTime(2017, 6, 30),
            });

            courses.Add(new Course
            {
                Name = "NETASP1707",
                Description = ".NET ASP.NET 2017 Fall",
                StartDate = new DateTime(2017, 7, 1),
                EndDate = new DateTime(2017, 12, 30),
            });

            courses.Add(new Course
            {
                Name = "JAVAEE1707",
                Description = "Java EE 2017 Fall",
                StartDate = new DateTime(2017, 7, 1),
                EndDate = new DateTime(2017, 12, 30),
            });
            
            context.Courses.AddOrUpdate(c => c.Id, courses.ToArray());
            context.SaveChanges();

            foreach (var course in courses.Where(c => c.Name != "None").ToList())
            {
                var mods = new List<Module>();

                mods.Add(new Module
                {
                    Name = course.Name + ": Introduction",
                    Description = "Welcome to the course!",
                    StartDate = course.StartDate,
                    EndDate = course.StartDate.AddDays(25),
                    Course = course,
                });

                mods.Add(new Module
                {
                    Name = course.Name + ": History",
                    Description = "Study the past.",
                    StartDate = course.StartDate.AddMonths(1),
                    EndDate = course.StartDate.AddMonths(1).AddDays(25),
                    Course = course,
                });

                mods.Add(new Module
                {
                    Name = course.Name + ": Theory",
                    Description = "Explore the basic concepts.",
                    StartDate = course.StartDate.AddMonths(2),
                    EndDate = course.StartDate.AddMonths(2).AddDays(25),
                    Course = course,
                });

                mods.Add(new Module
                {
                    Name = course.Name + ": Midterm exam",
                    Description = "Evaluate students' understanding.",
                    StartDate = course.StartDate.AddMonths(3),
                    EndDate = course.StartDate.AddMonths(3).AddDays(25),
                    Course = course,
                });

                mods.Add(new Module
                {
                    Name = course.Name + ": Current state",
                    Description = "Present recent progress and new trends.",
                    StartDate = course.StartDate.AddMonths(4),
                    EndDate = course.StartDate.AddMonths(4).AddDays(25),
                    Course = course,
                });

                mods.Add(new Module
                {
                    Name = course.Name + ": Final exam",
                    Description = "Determines your grade.",
                    StartDate = course.StartDate.AddMonths(5),
                    EndDate = course.EndDate,
                    Course = course,
                });

                context.Modules.AddOrUpdate(m => m.Id, mods.ToArray());
            }

            context.SaveChanges();

            var modules = (from m in context.Modules
                           select m).ToList();

            foreach (var module in modules)
            {
                var activities = new List<Activity>();

                activities.Add(new Activity
                {
                    Name = module.Name + ": Introduction",
                    Description = "Welcome to the module!",
                    StartDate = module.StartDate,
                    EndDate = module.StartDate.AddDays(5),
                    Kind = Kinds.Other,
                    Module = module,
                });

                activities.Add(new Activity
                {
                    Name = module.Name + ": E-Learning",
                    Description = "Self-paced, interactive videos.",
                    StartDate = module.StartDate.AddDays(5),
                    EndDate = module.StartDate.AddDays(10),
                    Kind = Kinds.ELearning,
                    Module = module,
                });

                activities.Add(new Activity
                {
                    Name = module.Name + ": Lecture",
                    Description = "Teacher gives an oral presentation.",
                    StartDate = module.StartDate.AddDays(10),
                    EndDate = module.StartDate.AddDays(15),
                    Kind = Kinds.Lecture,
                    Module = module,
                });

                activities.Add(new Activity
                {
                    Name = module.Name + ": Homework",
                    Description = "Assignment to be completed.",
                    StartDate = module.StartDate.AddDays(15),
                    EndDate = module.EndDate,
                    Kind = Kinds.Homework,
                    Module = module,
                });

                activities.Add(new Activity
                {
                    Name = module.Name + ": Quiz",
                    Description = "A test of knowledge.",
                    StartDate = module.StartDate.AddDays(20),
                    EndDate = module.EndDate,
                    Kind = Kinds.Other,
                    Module = module,
                });

                context.Activities.AddOrUpdate(a => a.Id, activities.ToArray());
            }

            context.SaveChanges();
            
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            roleManager.Create(new IdentityRole { Name = "Teacher" });
            roleManager.Create(new IdentityRole { Name = "Student" });

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = new ApplicationUser { UserName = "teacher@localhost.com", FirstName = "Bob", LastName = "Bobson", Email = "teacher@localhost.com", CourseId = noCourse.Id };
            userManager.Create(user, "Pass.123");
            userManager.AddToRole(user.Id, "Teacher");

            user = new ApplicationUser { UserName = "anders.svensson@localhost.com", FirstName = "Anders", LastName = "Svensson", Email = "anders.svensson@localhost.com", CourseId = noCourse.Id };
            userManager.Create(user, "Pass.123");
            userManager.AddToRole(user.Id, "Teacher");

            SetUpUser(context, userManager, "student@localhost.com", "Jill", "Jillson", "student@localhost.com", courses.ElementAt(1));
            SetUpUser(context, userManager, "peter.andersson@localhost.com", "Peter", "Andersson", "peter.andersson@localhost.com", courses.ElementAt(3));

            var rand = new Random();

            for (int i = 0; i < 30; i++)
            {
                var firstName = Faker.Name.First();
                var lastName = Faker.Name.Last();
                var email = firstName.ToLower() + "." + lastName.ToLower() + "@localhost.com";
                var cor = courses.ElementAt(rand.Next(0, courses.Count()));

                SetUpUser(context, userManager, email, firstName, lastName, email, cor);
            }
        }

        private void SetUpUser(LMS.Models.ApplicationDbContext context, UserManager<ApplicationUser> userManager, string userName, string firstName, string lastName, string email, Course course)
        {
            if (!context.Users.Any(u => u.UserName == userName))
            {
                var user = new ApplicationUser { UserName = userName, FirstName = firstName, LastName = lastName, Email = email, CourseId = course.Id };

                userManager.Create(user, "Pass.123");
                context.SaveChanges();

                var userQuery = (from u in context.Users
                          where u.UserName == userName
                          select u).ToList();

                if (userQuery == null || userQuery.Count() == 0)
                    return;

                var us = userQuery.ElementAt(0);

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
