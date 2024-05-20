using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class  TestSetiViewModel
    {
         
        [DisplayName("№ счетчика")]
        public long n_sh { get; set; }
        [DisplayName("Дата/Время")]
        public DateTime testtime { get; set; }
        [DisplayName("Ua(B)")]
        public double? ua { get; set; }
        [DisplayName("Ub(B)")]
        public double? ub { get; set; }
        [DisplayName("Uc(B)")]
        public double? uc { get; set; }


        public TestSetiViewModel()
        {
            n_sh = 123456789;
            testtime = DateTime.Now;
            ua = 10.1;
            ub = 50.02;
            uc = 103;

        }
        public TestSetiViewModel(TEST_SETI entity) 
        {
            n_sh = entity.N_SH;
            testtime = entity.TESTTIME;
            ua = entity.UA;
            ub = entity.UB;
            uc = entity.UC;
                
        }
    }
}