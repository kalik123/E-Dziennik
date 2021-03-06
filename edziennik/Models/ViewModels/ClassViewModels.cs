﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Models.Models;

namespace edziennik.Models.ViewModels
{
    public class ClassCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Wychowawca")]
        public string TeacherId { get; set; }

        public List<SelectListItem> Teachers { get; set; }

    }

    public class ClassListItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Wychowawca")]
        public string Teacher { get; set; }

        [Display(Name = "Id Wychowawcy")]
        public string TeacherId { get; set; }

    }

    public class ClassDetailsViewModel : ClassListItemViewModel
    {
       // public List<Student> Students { get; set; }

        [Display(Name = "Ilość uczniów")]
        public int StudentCount { get; set; }
    }

    public class ClassEditViewModel : ClassCreateViewModel
    {
        public List<Student> Students { get; set; }
    }
}