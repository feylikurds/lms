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
    public class StudentActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentActivities
        public ActionResult Index()
        {
            var courses = new List<Course>();

            if (User.IsInRole("Teacher"))
            {
                courses = db.Courses
                          .Where(c => c.Name != "None")
                          .ToList();
            }
            else
            {
                var user = (from u in db.Users
                            where u.UserName == User.Identity.Name
                            select u).FirstOrDefault();
                courses = (from c in db.Courses
                          where c.Id == user.CourseId
                          select c).ToList(); 
            }

            return View(courses);
        }

        // GET: StudentActivities/ListModules/5
        public ActionResult ListModules(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var modules = from m in db.Modules
                          where m.CourseId == id
                          select m;

            if (modules == null)
            {
                return HttpNotFound();
            }

            return View(modules);
        }

        // GET: StudentActivities/ListActivities/5
        public ActionResult StudentListActivities()
        {
            var user = (from u in db.Users
                          where u.UserName == User.Identity.Name
                          select u).FirstOrDefault();

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var activities = (from a in db.StudentActivities
                              where a.StudentId == user.Id
                              select a).ToList();

            if (activities == null)
            {
                return HttpNotFound();
            }

            var studentActivities = new List<StudentActivityViewModel>();

            foreach (var activity in activities)
            {
                var sa = new StudentActivityViewModel();
                var a = (from ac in db.Activities
                         where ac.Id == activity.ActivityId
                         select ac).First();

                sa.ActivityName = a.Name;
                sa.Status = activity.Status;
                sa.Grade = activity.Grade;
                sa.Documents = a.Documents.ToList();

                studentActivities.Add(sa);
            }

            ViewBag.Id = user.Id;

            return View(studentActivities);
        }

        // GET: StudentActivities/ListActivities/5
        public ActionResult ListActivities(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var activities = (from a in db.Activities
                                where a.ModuleId == id
                                select a).ToList();
            
            if (activities == null)
            {
                return HttpNotFound();
            }

            return View(activities);
        }

        // GET: StudentActivities/HandleClass/5
        public ActionResult HandleClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentActivities = (from sa in db.StudentActivities
                                     where sa.ActivityId == id
                                     select sa).ToList();
            var students = new List<StudentActivityViewModel>();

            foreach (var sa in studentActivities)
            {
                var activityName = (from a in db.Activities
                                    where a.Id == sa.ActivityId
                                    select a.Name).First();
                var studentName = (from s in db.Users
                                    where s.Id == sa.StudentId
                                    select s).First().FullName;

                students.Add(new StudentActivityViewModel{ ActivityId = sa.ActivityId, ActivityName = activityName,
                                                           StudentId = sa.StudentId, StudentName = studentName,
                                                           Status = sa.Status, Grade = sa.Grade });                
            }

            ViewBag.Id = id;
            ViewBag.Documents = (from a in db.Activities
                                 where a.Id == id
                                 select a.Documents).First().ToList();

            return View(students);
        }

        // GET: StudentActivities/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentActivity studentActivity = db.StudentActivities.Find(id);
            if (studentActivity == null)
            {
                return HttpNotFound();
            }
            return View(studentActivity);
        }

        // GET: StudentActivities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,ActivityId,Status,Grade")] StudentActivity studentActivity)
        {
            if (ModelState.IsValid)
            {
                db.StudentActivities.Add(studentActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentActivity);
        }

        // GET: StudentActivities/EditStudent/5
        public ActionResult EditStudent(int activityId, string studentId)
        {
            if (activityId == 0 || string.IsNullOrEmpty(studentId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = (from u in db.Users
                        where u.Id == studentId
                        select u).FirstOrDefault();

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var studentActivity = (from a in db.StudentActivities
                                   where a.StudentId == user.Id && a.ActivityId == activityId
                                   select a).First();

            if (studentActivity == null)
            {
                return HttpNotFound();
            }

            var activityName = (from a in db.Activities
                                where a.Id == activityId
                                select a.Name).First();

            var viewModel = new StudentActivityViewModel { ActivityId = activityId, ActivityName = activityName,
                                                           StudentId = user.Id, StudentName = user.FullName,
                                                           Status = studentActivity.Status, Grade = studentActivity.Grade };

            return View(viewModel);
        }

        // POST: StudentActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(StudentActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var studentActivity = (from sa in db.StudentActivities
                                       where sa.StudentId == viewModel.StudentId && sa.ActivityId == viewModel.ActivityId
                                       select sa).First();

                studentActivity.Status = viewModel.Status;
                studentActivity.Grade = viewModel.Grade;

                db.Entry(studentActivity).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("HandleClass", new { Id = studentActivity.ActivityId});
            }

            return View(viewModel);
        }

        // GET: StudentActivities/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentActivity studentActivity = db.StudentActivities.Find(id);
            if (studentActivity == null)
            {
                return HttpNotFound();
            }
            return View(studentActivity);
        }

        // POST: StudentActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,ActivityId,Status,Grade")] StudentActivity studentActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentActivity);
        }

        // GET: StudentActivities/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentActivity studentActivity = db.StudentActivities.Find(id);
            if (studentActivity == null)
            {
                return HttpNotFound();
            }
            return View(studentActivity);
        }

        // POST: StudentActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StudentActivity studentActivity = db.StudentActivities.Find(id);
            db.StudentActivities.Remove(studentActivity);
            db.SaveChanges();
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
