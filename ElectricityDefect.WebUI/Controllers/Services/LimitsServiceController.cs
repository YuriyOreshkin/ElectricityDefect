using System;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Mvc;
using ElectricityDefect.Domain.Services;
using ElectricityDefect.WebUI.Models;

namespace ElectricityDefect.WebUI.Controllers.Services
{
    
    public class LimitsServiceController : Controller
    {
        private LimitsService repos;
     
        public LimitsServiceController(IRepository _repos, IACRepository _ac)
        {
            repos = new LimitsService(_repos,_ac) ;
      
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

        public ActionResult GetSHLimits(long n_sh)
        {
            var view = repos.GetSHLimits(n_sh);
            if (view == null)
            {
                view = new LimitsSetiViewModel { n_sh = n_sh };
            }
            return Json(view);
        }


        //Create
        [HttpPost]
        public ActionResult ModifyForGrid([DataSourceRequest] DataSourceRequest request, LimitsSetiViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    repos.Modify(view, ModelState);
                    
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("error", "Не удалось внести ограничения. " +ex.Message);
                }
            }

            return Json(new[] { view }.ToDataSourceResult(request, ModelState));

        }




     
        //Delete
        [HttpPost]
     
        public ActionResult DestroyForGrid([DataSourceRequest] DataSourceRequest request, LimitsSetiViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repos.Delete(view, ModelState);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", "Не удалось удалить ограничения. " + ex.Message);
                }
            }

            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }




    }
}