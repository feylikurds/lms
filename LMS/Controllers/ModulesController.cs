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
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules
                .OrderBy(c => c.Course.Name)
                .ThenBy(c => c.StartDate)
                .ThenBy(c=>c.EndDate)
                .ThenBy(c => c.Name).ToList());
        }

        /// <summary>
        /// Returns all modules belonging to a certain courseId
        /// </summary>
        public ActionResult ModulesByCourse(int courseId)
        {
            var modules = db.Modules.Where(m => m.CourseId == courseId);
            return View(modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");

            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            var validRange = module.StartDate <= module.EndDate;
            var notTooOld = DateTime.Now <= module.StartDate;

            var courseStartDate = db.Courses.Where(c => c.Id == module.CourseId).FirstOrDefault().StartDate;
            var courseEndDate = db.Courses.Where(c => c.Id == module.CourseId).FirstOrDefault().EndDate;

            var inCourseRange = courseStartDate <= module.StartDate
                && courseEndDate >= module.EndDate;

            if (!validRange)
            {
                ModelState.AddModelError("", "Invalid date range.");
            }
            else if (!notTooOld)
            {
                ModelState.AddModelError("", "Date too old.");
            }
            else if (ModelState.IsValid && inCourseRange)
            {
                db.Modules.Add(module);
                db.SaveChanges();

                LMS.Shared.Database.UpdateUsers(db);

                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var module = db.Modules.Find(id);

            if (module == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);

            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            var validRange = module.StartDate <= module.EndDate;
            var notTooOld = DateTime.Now <= module.StartDate;

            var courseStartDate = db.Courses.Where(c => c.Id == module.CourseId).FirstOrDefault().StartDate;
            var courseEndDate = db.Courses.Where(c => c.Id == module.CourseId).FirstOrDefault().EndDate;

            var inCourseRange = courseStartDate <= module.StartDate
                && courseEndDate >= module.EndDate;

            if (!validRange)
            {
                ModelState.AddModelError("", "Invalid date range.");
            }
            else if (!notTooOld)
            {
                ModelState.AddModelError("", "Date too old.");
            }
            else if (!inCourseRange)
            {
                ModelState.AddModelError("", "Date not within the coure's dates");
            }
            else if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                LMS.Shared.Database.UpdateUsers(db);

                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);

            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();

            LMS.Shared.Database.UpdateUsers(db);

            return RedirectToAction("Index");
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
