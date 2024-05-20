using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class TreePointViewModel
    {
        public long id { get; set; }
        [Display(Name = "Наименование")]
        public string name { get; set; }
        public bool hasChildren { get; set; }


    }
}