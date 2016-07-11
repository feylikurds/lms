using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace LMS.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult Index()
        {
            return View(db.Documents.ToList());
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,UploadTime,UploaderId,Deadline,DocumentName,DocumentType")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,UploadTime,UploaderId,Deadline,DocumentName,DocumentType")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }
        // GET: Documents/Remove/5
        public ActionResult Remove(int? id, int objectId, string returnAction, string returnController)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = db.Documents.Find(id);

            if (document == null)
            {
                return HttpNotFound();
            }

            db.Documents.Remove(document);
            db.SaveChanges();

            return RedirectToAction(returnAction, returnController, new { id = objectId });
        }
        
        // GET: Documents/Download/5
        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);

            if (document == null)
            {
                return HttpNotFound();
            }

            byte[] fileBytes = document.Content;

            return File(fileBytes, document.ContentType, document.FileName);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Documents/Upload/5
        public ActionResult Upload(Document.DocumentTypes documentType, int objectId, string returnAction, string returnController)
        {
            var userId = User.Identity.GetUserId();
            var user = (from u in db.Users
                        where u.Id == userId
                        select u).First();

            if (user == null)
            {
                return HttpNotFound();
            }

            var document = new Document();

            document.DocumentType = documentType;
            document.Uploader = user;
            document.UploaderId = user.Id;

            ViewBag.ObjectId = objectId;
            ViewBag.ReturnAction = returnAction;
            ViewBag.ReturnController = returnController;

            return View(document);
        }

        // POST: Documents/Upload/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Upload(Document document, HttpPostedFileBase upload, int objectId, string returnAction, string returnController)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    document.FileName = System.IO.Path.GetFileName(upload.FileName);
                    document.ContentType = upload.ContentType;

                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        document.Content = reader.ReadBytes(upload.ContentLength);
                    }

                    switch (document.DocumentType)
                    {
                        case Document.DocumentTypes.Course:
                            document.Course = (from c in db.Courses
                                               where c.Id == objectId
                                               select c).First();
                            document.Course.Documents.Add(document);

                            break;

                        case Document.DocumentTypes.Module:
                            document.Module = (from m in db.Modules
                                               where m.Id == objectId
                                               select m).First();
                            document.Module.Documents.Add(document);

                            break;

                        case Document.DocumentTypes.Activity:
                            document.Activity = (from a in db.Activities
                                                 where a.Id == objectId
                                                 select a).First();
                            document.Activity.Documents.Add(document);

                            break;

                        case Document.DocumentTypes.Homework:
                            document.StudentActivity = (from sa in db.StudentActivities
                                                 where sa.Id == objectId
                                                 select sa).First();
                            document.StudentActivity.Documents.Add(document);

                            break;

                        default:
                            break;
                    }

                    document.Uploader = (from u in db.Users
                                         where u.Id == document.UploaderId
                                         select u).First();

                    db.Documents.Add(document);
                    db.SaveChanges();

                    return RedirectToAction(returnAction, returnController, new { id = objectId });
                }
            }

            ViewBag.ObjectId = objectId;
            ViewBag.ReturnAction = returnAction;
            ViewBag.ReturnController = returnController;

            return View(document);
        }

        [HttpPost]
        public ActionResult Comments(int id, string comments, int returnId)
        {
            var document = (from d in db.Documents
                            where d.Id == id
                            select d).First();

            document.Comments = comments;

            db.SaveChanges();

            return RedirectToAction("HandleClass", "StudentActivities", new { id = returnId });
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
