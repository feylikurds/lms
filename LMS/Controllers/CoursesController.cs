using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            return View(db.Courses.OrderBy(c => c.StartDate).ThenBy(c=>c.EndDate).ThenBy(c => c.Name).ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        /// <summary>
        /// Checks is the data for a course is valid
        /// </summary>
        /// <param name="course">The course to check</param>
        /// <returns>True if it is valid</returns>
        private bool IsValidCourse(Course course)
        {
            return course.EndDate >= course.StartDate;
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid && IsValidCourse(course))
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid && IsValidCourse(course))
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Given a Course.ID, it returns a View of finished Modules for that Course
        /// </summary>
        [Authorize]
        public ActionResult FinishedModules(int courseId)
        {
            DateTime now = DateTime.Now;

            var finishedModules = from m in db.Modules
                                  where m.CourseId == courseId && m.EndDate < now
                                  select m;
            return View("Index", finishedModules.ToList());
        }

        /// <summary>
        /// Given a Course.ID, it returns a View of current Modules for that Course
        /// </summary>
        [Authorize]
        public ActionResult CurrentModules(int courseId)
        {
            DateTime now = DateTime.Now;

            var currentModules = from m in db.Modules
                                 where m.CourseId == courseId && m.StartDate <= now && m.EndDate >= now
                                 select m;
            return View("Index", currentModules.ToList());

        }

        /// <summary>
        /// Given a Course.ID, it returns a View of all Modules for that Course
        /// </summary>
        [Authorize]
        public ActionResult AllModules(int courseId)
        {
            var allModules = from m in db.Modules
                             where m.CourseId == courseId
                             select m;
            return View("Index", allModules.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
