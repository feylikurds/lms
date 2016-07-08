using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LMS.Shared
{
    public static class Database
    {
        public static async void UpdateUsers(ApplicationDbContext db)
        {
            var users = (from u in db.Users
                         select u).ToList();
            var userIds = (from u in db.Users
                           select u.Id).ToList();
            var deletedStudents = (from sa in db.StudentActivities
                                   where !userIds.Contains(sa.StudentId)
                                   select sa).ToList();

            db.StudentActivities.RemoveRange(deletedStudents);

            await db.SaveChangesAsync();

            foreach (var u in users)
            {
                if (u.Course.Name == "None")
                    continue;

                var moduleIds = db.Modules
                                .Where(m => m.CourseId == u.CourseId)
                                .Select(m => m.Id)
                                .ToList();
                var allActivtyIds = db.Activities
                                   .Where(a => moduleIds.Contains(a.ModuleId))
                                   .Select(a => a.Id)
                                   .ToList();
                var currentActivityIds = db.StudentActivities
                                               .Where(sa => sa.StudentId == u.Id)
                                         .Select(sa => sa.ActivityId)
                                               .ToList();
                var deletedActivityIds = currentActivityIds
                                         .Where(csa => !allActivtyIds.Contains(csa))
                                               .ToList();
                var deletedStudentActivities = (from sa in db.StudentActivities
                                                where sa.StudentId == u.Id && deletedActivityIds.Contains(sa.ActivityId)
                                                select sa).ToList();

                db.StudentActivities.RemoveRange(deletedStudentActivities);

                await db.SaveChangesAsync();

                var newActivtyIds = allActivtyIds
                                   .Where(aa => !currentActivityIds.Contains(aa))
                                   .ToList();

                foreach (var na in newActivtyIds)
                    db.StudentActivities.Add(new StudentActivity { StudentId = u.Id, ActivityId = na });

                await db.SaveChangesAsync();
            }
        }
    }
}