using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class StudentActivityViewModel
    {
        public int ActivityId { get; set; }
        [DisplayAttribute(Name = "Activity Name")]
        public string ActivityName { get; set; }
        public string StudentId { get; set; }
        [DisplayAttribute(Name = "Student Name")]
        public string StudentName { get; set; }
        public Statuses Status { get; set; }
        public Grades Grade { get; set; }
    }
}