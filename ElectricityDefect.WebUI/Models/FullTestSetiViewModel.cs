using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class  FullTestSetiViewModel :TestSetiViewModel
    {

        [DisplayName("№ объекта")]
        public long n_ob { get; set; }
        [DisplayName("Наименование объекта")]
        public string name_ob { get; set; }
        
        [DisplayName("Фидер")]
        public string name_sh { get; set; }
        [DisplayName("UaMin")]
        public double? uamin { get; set; }
        [DisplayName("UaMax")]
        public double? uamax { get; set; }

        [DisplayName("UbMin")]
        public double? ubmin { get; set; }
        [DisplayName("UbMax")]
        public double? ubmax { get; set; }
        [DisplayName("UcMin")]
        public double? ucmin { get; set; }
        [DisplayName("UcMax")]
        public double? ucmax { get; set; }

        public bool incident { get; set; }    

        public FullTestSetiViewModel() : base()
        {
            n_ob = 1;
            name_ob = "Тест";
            name_sh = "Ввод №1";
            uamax = ubmax = ucmax = 60.22;
            uamin = ubmin = ucmin = 50.10;


        }

        public FullTestSetiViewModel(TEST_SETI entity) :base(entity)
        {
            if (entity.SH != null)
            {
                name_sh = entity.SH.TXT;

                if(entity.SH.OBEKT != null)
                {
                    name_ob = entity.SH.OBEKT.TXT;
                    n_ob = entity.SH.OBEKT.N_OB;
                }
            }
            
        }
    }
}