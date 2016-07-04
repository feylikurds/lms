using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles ="Teacher")]
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Teacher
        public ActionResult Index()
        {
            return View(db.Courses.OrderBy(c => c.StartDate).ThenBy(c=>c.EndDate).ThenBy(c => c.Name).ToList());
        }
    }
}