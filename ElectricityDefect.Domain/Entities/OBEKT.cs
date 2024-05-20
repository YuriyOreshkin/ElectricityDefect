using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectricityDefect.Domain.Entities
{
    [Table("OBEKT", Schema ="CNT")]
    public class OBEKT
    {
       
        [Key]
        public long N_OB { get; set; }
        public string TXT { get; set; }

        public int SHS { get; set; }
       
    }
}
