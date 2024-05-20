using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricityDefect.Domain.Entities
{
    public class MAILSERVICESETTINGS
    {
        public string HOST { get; set; }
        public int PORT { get; set; }
        public string USER { get; set; }
        public string PASSWORD { get; set; }

    }
}
