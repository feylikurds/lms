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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; } = DateTime.Now;
        public int ObjectId { get; set; }
        [DisplayAttribute(Name = "Document Type")]
        public DocumentTypes DocumentType { get; set; } = DocumentTypes.Other;

        [DisplayAttribute(Name = "Uploader")]
        public string UploaderId { get; set; }
        public virtual ApplicationUser Uploader { get; set; }
        [DisplayAttribute(Name = "Upload Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UploadTime { get; set; } = DateTime.Now;

        public string Comments { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }

        public Course Course { get; set; }
        public Module Module { get; set; }
        public Activity Activity { get; set; }
        public StudentActivity StudentActivity { get; set; }

        public enum DocumentTypes
        {
            Course,
            Module,
            Activity,
            Homework,
            Other,
        }
    }
}