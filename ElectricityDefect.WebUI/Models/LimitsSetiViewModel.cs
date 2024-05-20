using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class  LimitsSetiViewModel
    {
       
        [Display(Name = "№ объекта")]
        public long n_ob { get; set; }
        [Display(Name = "Наименование")]
        public string name_ob { get; set; }
        [Required]
        [Display(Name = "№ счетчика")]
        public long n_sh { get; set; }
        [Display(Name = "Наименование")]
        public string name_sh { get; set; }
        [Display(Name = "Min")]
        public double? uamin { get; set; }
        [Display(Name = "Max")]
        public double? uamax { get; set; }

        [Display(Name = "Min")]
        public double? ubmin { get; set; }
        [Display(Name = "Max")]
        public double? ubmax { get; set; }
        [Display(Name = "Min")]
        public double? ucmin { get; set; }
        [Display(Name = "Max")]
        public double? ucmax { get; set; }

       
    }
}