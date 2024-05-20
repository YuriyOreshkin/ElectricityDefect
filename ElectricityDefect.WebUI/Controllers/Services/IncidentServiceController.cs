using System;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Mvc;
using ElectricityDefect.Domain.Services;
using ElectricityDefect.WebUI.Models;

namespace ElectricityDefect.WebUI.Controllers.Services
{
    
    public class IncidentServiceController : Controller
    {
        private IncidentService repos;
     
        public IncidentServiceController(IRepository _repos, IACRepository _ac)
        {
            repos = new IncidentService(_repos,_ac) ;
      
        }

        protected override void Dispose(bool disposing)
        {
            repos.Dispose();

            base.Dispose(disposing);
        }

        //Read
        public ActionResult ReadForGrid([DataSourceRequest] DataSourceRequest request)
        {

            return Json(repos.GetAll().ToDataSourceResult(request));
        }

        public ActionResult ShowToAddIncident(IncidentViewModel view)
        {
            var incident = repos.ConvertToViewModel(new Domain.Entities.INCIDENT { id =view.id,BEGIN=view.begin, N_SH= view.n_sh, END = view.end }, view);
            return PartialView("~/Views/Journal/Edit.cshtml", incident);
        }

        [HttpPost]
        public ActionResult CreateIncident(IncidentViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    repos.Create(view, ModelState);
                }
                catch
                {
                    ModelState.AddModelError("error", "Не удалось внести событие.");
                }
            }

            return Json(new { Errors = ModelState.IsValid ? null : ModelState.SerializeErrors() });
        }


        //Create
        [HttpPost]
        public ActionResult CreateForGrid([DataSourceRequest] DataSourceRequest request, IncidentViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    repos.Create(view, ModelState);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", "Не удалось внести событие!. " + ex.Message);
                }
            }

            return Json(new[] { view }.ToDataSourceResult(request, ModelState));

        }


        //Create
        [HttpPost]
        public ActionResult UpdateForGrid([DataSourceRequest] DataSourceRequest request, IncidentViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    repos.Update(view, ModelState);
                    
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("error", "Не удалось внести событие!. " +ex.Message);
                }
            }

            return Json(new[] { view }.ToDataSourceResult(request, ModelState));

        }

     
        //Delete
        [HttpPost]
     
        public ActionResult DestroyForGrid([DataSourceRequest] DataSourceRequest request, IncidentViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repos.Delete(view, ModelState);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", "Не удалось удалить событие. " + ex.Message);
                }
            }

            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }




    }
}