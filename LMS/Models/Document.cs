using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayAttribute(Name = "Upload Time")]
        public DateTime UploadTime { get; set; }
        [DisplayAttribute(Name = "Uploader")]
        public int UploaderId { get; set; }
        public DateTime Deadline { get; set; }
        public string DocumentName { get; set; }
        public DocumentTypes DocumentType { get; set; }

        public Course Course { get; set; }
        public Module Module { get; set; }
        public Activity Activity { get; set; }

        public enum DocumentTypes
        {
            Course,
            Module,
            Activity,
            Homework,
        }
    }
}