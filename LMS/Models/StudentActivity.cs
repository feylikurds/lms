using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class StudentActivity
    {
        public string StudentId { get; set; }
        public int ActivityId { get; set; }

        public Statuses Status { get; set; }
        public Grades Grade { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }

    public enum Statuses
    {
        Waiting,
        Done,
        Error,
    }

    public enum Grades
    {
        Pass,
        Fail,
    }
}