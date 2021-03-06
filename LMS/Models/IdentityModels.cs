﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure.Annotations;
using System.Web.Mvc;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DisplayAttribute(Name = "First name")]
        public string FirstName { get; set; }
        [DisplayAttribute(Name = "Last name")]
        public string LastName { get; set; }
        [DisplayAttribute(Name = "Full name")]
        public string FullName { get { return FirstName + " " + LastName; } }
        [DisplayAttribute(Name = "Registration Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TimeOfRegistration { get; set; } = DateTime.Now;

        [DisplayAttribute(Name = "Course")]
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<StudentActivity> StudentActivities { get; set; }

        //[NotMapped]
        //public string AssignedRole { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Activity> Activities { get; set; }
        
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentActivity>()
                .HasKey(c => new { c.StudentId, c.ActivityId });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(c => c.StudentActivities)
                .WithRequired()
                .HasForeignKey(c => c.StudentId);

            modelBuilder.Entity<Activity>()
                .HasMany(c => c.StudentActivities)
                .WithRequired()
                .HasForeignKey(c => c.ActivityId);
        }

        public System.Data.Entity.DbSet<LMS.Models.StudentActivity> StudentActivities { get; set; }

        //public System.Data.Entity.DbSet<LMS.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<LMS.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}