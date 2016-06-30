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
    [Authorize(Roles = "Teacher")]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        // GET: Activities
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Activities.ToList());
        }

        /// <summary>
        /// Returns all activities belonging to a certain moduleId
        /// </summary>
        [Authorize]
        public ActionResult ActivitiesByModule(int moduleId)
        {
            var activities = db.Activities.Where(m => m.ModuleId == moduleId);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            ViewBag.Modules = new SelectList(db.Modules.ToList(), "Id", "Name");

            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,ModuleId")] Activity activity)
        {
            var validRange = activity.StartDate <= activity.EndDate;
            var notTooOld = DateTime.Now <= activity.StartDate;

            if (!validRange)
            {
                ModelState.AddModelError("", "Invalid date range.");
            }
            else if (!notTooOld)
            {
                ModelState.AddModelError("", "Date too old.");
            }
            else if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Modules = new SelectList(db.Modules.ToList(), "Id", "Name");

            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var activity = db.Activities.Find(id);

            if (activity == null)
            {
                return HttpNotFound();
            }

            ViewBag.Modules = new SelectList(db.Modules.ToList(), "Id", "Name", activity.ModuleId);

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,ModuleId")] Activity activity)
        {
            var validRange = activity.StartDate <= activity.EndDate;
            var notTooOld = DateTime.Now <= activity.StartDate;

            if (!validRange)
            {
                ModelState.AddModelError("", "Invalid date range.");
            }
            else if (!notTooOld)
            {
                ModelState.AddModelError("", "Date too old.");
            }
            else if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Modules = new SelectList(db.Modules.ToList(), "Id", "Name", activity.ModuleId);

            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Given a Module.ID, it returns a View of finished activities for that module
        /// </summary>
        [Authorize]
        public ActionResult FinishedActivities(int moduleId)
        {
            DateTime now = DateTime.Now;

            var finishedActivities = from a in db.Activities
                                     where a.ModuleId == moduleId && a.EndDate < now
                                     select a;
            return View("Index", finishedActivities.ToList());
        }

        /// <summary>
        /// Given a Module.ID, it returns a View of current activities for that module
        /// </summary>
        [Authorize]
        public ActionResult CurrentActivities(int moduleId)
        {
            DateTime now = DateTime.Now;

            var currentActivities = from a in db.Activities
                                    where a.ModuleId == moduleId && a.StartDate <= now && a.EndDate >= now
                                    select a;
            return View("Index", currentActivities.ToList());

        }

        /// <summary>
        /// Given a Module.ID, it returns a View of all activities for that module
        /// </summary>
        [Authorize]
        public ActionResult AllActivities(int moduleId)
        {
            var allActivities = from a in db.Activities
                                where a.ModuleId == moduleId
                                select a;
            return View("Index", allActivities.ToList());
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
