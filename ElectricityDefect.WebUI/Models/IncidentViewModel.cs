using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class  IncidentViewModel
    {
        
        public long id { get; set; }

        [Display(Name = "№ объекта")]
        public long n_ob { get; set; }
        [Display(Name = "Наименование")]
        public string name_ob { get; set; }
        [Required]
        [Display(Name = "№ счетчика")]
        public long n_sh { get; set; }
        [Display(Name = "Фидер")]
        public string name_sh { get; set; }

        
        [Display(Name = "Начало")]
        public DateTime begin { get; set; }
        [Display(Name = "Окончание")]
        public DateTime end { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string comment { get; set; }
        public INCIDENT ToEntity(INCIDENT incident)
        {

            incident.id = id;
            incident.N_SH = n_sh;
            incident.BEGIN = begin;
            incident.END = end;
            incident.COMMENT = comment;
           

            return incident;
        }

    }
}