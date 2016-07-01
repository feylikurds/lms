using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class StudentActivityViewModel
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public Statuses Status { get; set; }
        public Grades Grade { get; set; }
    }
}