using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricityDefect.Domain.Services
{
    public interface IMailServiceConfig
    {
        MAILSERVICESETTINGS ReadSettings();
        void SaveSettings(MAILSERVICESETTINGS settings);
     
    }
}
