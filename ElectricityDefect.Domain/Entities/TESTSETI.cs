using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectricityDefect.Domain.Entities
{
    [Table("TEST_SETI", Schema ="CNT")]
    public class TEST_SETI
    {
        public long N_SH { get; set; }
        public DateTime TESTTIME { get; set; }
        public double? UA { get; set; }
        public double? UB { get; set; }
        public double? UC { get; set; }

        public virtual SH SH { get; set; }
    }
}
