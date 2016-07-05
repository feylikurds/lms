using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayAttribute(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }
        [DisplayAttribute(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime EndDate { get; set; }

        [DisplayAttribute(Name = "Module")]
        public int ModuleId { get; set; }
        [DisplayAttribute(Name = "Module")]
        public virtual Module Module { get; set; }

        public virtual ICollection<StudentActivity> StudentActivities { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}