using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class StudentActivity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string StudentId { get; set; }
        public int ActivityId { get; set; }

        public Statuses Status { get; set; }
        public Grades Grade { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

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
        Waiting,
        Pass,
        Fail,
        Error,
    }
}