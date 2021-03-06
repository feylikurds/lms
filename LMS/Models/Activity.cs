﻿using System;
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

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayAttribute(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayAttribute(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public Kinds Kind { get; set; } = Kinds.Other;

        [DisplayAttribute(Name = "Module")]
        public int ModuleId { get; set; }
        [DisplayAttribute(Name = "Module")]
        public virtual Module Module { get; set; }

        public virtual ICollection<StudentActivity> StudentActivities { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }


    public enum Kinds
    {
        ELearning,
        Lecture,
        Homework,
        Other
    }
}