using ElectricityDefect.Domain.Entities;
using ElectricityDefect.Domain.Services;
using ElectricityDefect.WebUI.Models ;    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectricityDefect.WebUI.Controllers.Services
{
  
    public class MailSettingsServiceController : Controller
    {
        private IMailServiceConfig mailconfig;
        private IAlert alert;
        public MailSettingsServiceController(IMailServiceConfig _mailconfig, IAlert _alert)
        {
            mailconfig = _mailconfig;
            alert = _alert;
        }

        public ActionResult Settings()
        {
            try
            {
                var settings = mailconfig.ReadSettings();

                MailSettingsViewModel view = new MailSettingsViewModel
                {
                    host = settings.HOST,
                    port = settings.PORT,
                    user = settings.USER,
                    password = settings.PASSWORD
                };

                return PartialView("~/Views/Mail/Settings.cshtml", view);
            }
            catch (Exception ex)
            {
                return Content("Ошибка: "+ ex.Message);
            }
        }
        public JsonResult TestSettings(MailSettingsViewModel viewsettings)
        {
            MAILSERVICESETTINGS settings = viewsettings.ToEntity(new MAILSERVICESETTINGS());

            //Save
            try
            {
                
                alert.Alert(new FullTestSetiViewModel(),settings);

            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { message = "OK", result = viewsettings }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSettings(MailSettingsViewModel viewsettings)
        {
            MAILSERVICESETTINGS settings = viewsettings.ToEntity(new MAILSERVICESETTINGS());

            //Save
            try
            {
                mailconfig.SaveSettings(settings);

            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { message = "OK", result = viewsettings }, JsonRequestBehavior.AllowGet);
        }

    }
}