using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events.Calendar;
using DayPilot.Web.Mvc.Events.Month;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace LMS.Controllers
{
    public class CalendarController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var user = (from u in db.Users
                        where u.UserName == userName
                        select u).First();
            var course = (from c in db.Courses
                           where c.Id == user.CourseId
                           select c)
              .OrderBy(c => c.StartDate)
              .ThenBy(c => c.EndDate)
              .ThenBy(c => c.Name)
              .First();

            ViewBag.startDate = course.StartDate;

            return View();
        }

        public ActionResult Back(DateTime date)
        {
            ViewBag.startDate = date.AddMonths(-1);

            return View();
        }

        public ActionResult Forward(DateTime date)
        {
            ViewBag.startDate = date.AddMonths(1);

            return View();
        }

        public ActionResult Backend()
        {
            return new Dpm(db, User.Identity.GetUserName()).CallBack(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            
            base.Dispose(disposing);
        }

        class Dpm : DayPilotMonth
        {
            private ApplicationDbContext db;
            private string userName;

            public Dpm(ApplicationDbContext db, string userName)
            {
                this.db = db;
                this.userName = userName;
            }

            protected override void OnInit(DayPilot.Web.Mvc.Events.Month.InitArgs e)
            {
                var user = (from u in db.Users
                            where u.UserName == userName
                            select u).First();
                var courses = (from c in db.Courses
                               where c.Id == user.CourseId
                               select c)
                              .OrderBy(c => c.StartDate)
                              .ThenBy(c => c.EndDate)
                              .ThenBy(c => c.Name)
                              .ToList();
                var events = new List<Event>();

                foreach (var course in courses)
                {
                    events.Add(new Event {
                        Id = course.Id.ToString(),
                        Name = course.Name,
                        StartDate = course.StartDate,
                        EndDate = course.EndDate,
                    });

                    var modules = (from m in db.Modules
                                   where m.CourseId == course.Id
                                   select m)
                                  .OrderBy(m => m.StartDate)
                                  .ThenBy(m => m.EndDate)
                                  .ThenBy(m => m.Name)
                                  .ToList();

                    foreach (var module in modules)
                    {
                        events.Add(new Event
                        {
                            Id = module.Id.ToString(),
                            Name = module.Name,
                            StartDate = module.StartDate,
                            EndDate = module.EndDate,
                        });
                    }
                }

                Events = events;

                DataIdField = "Id";
                DataTextField = "Name";
                DataStartField = "StartDate";
                DataEndField = "EndDate";

                Update();
            }
        }

        class Event
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
