using ElectricityDefect.Domain.Services;
using ElectricityDefect.WebUI.Models;
using System;
using System.Web.Mvc;

namespace ElectricityDefect.WebUI.Controllers.Services
{
    public class TemplateServiceController : Controller
    {
        private ITemplateService service;
        public TemplateServiceController(ITemplateService _service)
        {
            service = _service;
        }

        public ActionResult Editor()
        {
            var model = new TemplateViewModel() { recipients=service.GetTemplateRecipients(), subject=service.GetTemplateSubject(),  body=service.GetTemplateBody() };

            return PartialView("Editor", model);

        }

        public ActionResult AvailableParameters()
        {

            var parameters = service.AvailableParameters(typeof(FullTestSetiViewModel));

            return PartialView(parameters);
        }

        public JsonResult SaveTemplate(TemplateViewModel view)
        {
            //Save
            try
            {

                 service.SaveTemplate(view.recipients,view.subject,view.body);
            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { message = "OK"  }, JsonRequestBehavior.AllowGet);
        }
    }
}