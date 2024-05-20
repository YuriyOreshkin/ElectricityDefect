using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Services
{
    public interface ISender
    {
        void SendEMail(MAILSERVICESETTINGS config, string to, string subject, string body, string attachment = null);

        
    }
}
