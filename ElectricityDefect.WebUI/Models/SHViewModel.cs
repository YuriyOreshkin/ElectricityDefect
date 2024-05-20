using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class SHViewModel
    {
        public long id { get; set; }
        [Display(Name = "Наименование")]
        public string name { get; set; }

        public long obid { get; set; }

        public SHViewModel(SH sh) 
        {
            id = sh.N_SH;
            name = sh.TXT;
            obid = sh.N_OB;
        }

    }
}