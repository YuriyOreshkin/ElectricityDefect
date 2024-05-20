using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ElectricityDefect.Domain.Entities;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class MailSettingsViewModel
    {
        [Required]
        [DisplayName("SMTP-сервер")]
        public string host { get; set; }
        [Required]
        [DisplayName("Порт")]
        public int port { get; set; }
        [Required]
        [DisplayName("Пользователь")]
        [EmailAddress]
        public string user { get; set; }
        [Required]
        [DisplayName("Пароль")]
        public string password { get; set; }

        public MAILSERVICESETTINGS ToEntity(MAILSERVICESETTINGS settings)
        {

            settings.HOST = host;
            settings.PORT = port;
            settings.USER = user;
            settings.PASSWORD = password;

            return settings;
        }

    }
}