using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ElectricityDefect.Domain.Entities
{
    public class INCIDENT
    {
        [Key]
        public long id { get; set; }

        public long N_SH { get; set; }

        public DateTime BEGIN { get; set; }

        public DateTime END { get; set; }

        public string COMMENT { get; set; }
    }
}
