﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Services
{
    public interface ICryptoService
    {
        string DecryptPassword(string ProxyPassword);
        string EncryptPassword(string ProxyPassword);
    }
}
