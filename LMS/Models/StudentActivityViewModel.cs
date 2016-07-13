using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class StudentActivityViewModel
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        [DisplayAttribute(Name = "Activity name")]
        public string ActivityName { get; set; }
        public string StudentId { get; set; }
        [DisplayAttribute(Name = "Student name")]
        public string StudentName { get; set; }
        public Statuses Status { get; set; }
        public Grades Grade { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Document> Homeworks { get; set; }
        public string Comments { get; set; }
    }
}