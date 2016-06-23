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
        public DateTime UploadTime { get; set; }
        public int UploaderId { get; set; }
        public DateTime Deadline { get; set; }
        public string DocumentName { get; set; }
        public DocumentTypes DocumentType { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public int ActivityId { get; set; }

        public enum DocumentTypes
        {
            Course,
            Module,
            Activity,
            Homework,
        }
    }
}