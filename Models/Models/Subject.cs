﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Id nauczyciela")]
        public int TeacherId { get; set; }

        [Display(Name = "Id klasy")]
        public int ClasssId { get; set; }

        [Display(Name = "Id sali")]
        public int ClassroomId { get; set; }

        [Display(Name = "Data zajęć")]
        public LessonHour Date { get; set; }
    }
}