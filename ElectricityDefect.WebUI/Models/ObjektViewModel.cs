using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class ObjektViewModel
    {
        public long id { get; set; }
        [Display(Name = "Наименование")]
        public string name { get; set; }

        public ObjektViewModel(OBEKT obekt ) 
        {
            id = obekt.N_OB;
            name = obekt.TXT;

        }

    }
}