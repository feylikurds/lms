using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace LMS.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Student
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            //var course = (from u in db.Users
            //             where u.Id == userId
            //             select u.Course).First();
            var course = db.Users.Where(u => u.Id == userId).Select(u => u.Course).First();
            return View(course);
        }
    }
}