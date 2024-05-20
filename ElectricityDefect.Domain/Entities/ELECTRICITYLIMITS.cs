using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Entities
{
    public class ELECTRICITYLIMITS
    {
        [Key]
        public long id { get; set; }
        public long N_SH { get; set; }
        //1-A;2-B;3-C
        public short FASA { get; set; }

        public double? UMIN { get; set; }

        public double? UMAX { get; set; }
    }
}
