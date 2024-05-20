using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectricityDefect.Domain.Entities
{
    [Table("SH", Schema = "CNT")]
    public class SH
    {
        public long N_SH { get; set; }
        public long N_OB { get; set; }
        public string TXT { get; set; }

        public virtual OBEKT  OBEKT { get; set; }
    }
}
