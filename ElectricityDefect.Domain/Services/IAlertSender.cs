using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Services
{
    public interface IAlert
    {
        
        void Alert(object alert,MAILSERVICESETTINGS config=null);
    }
}
