using ElectricityDefect.Domain.Entities;
using ElectricityDefect.Domain.Services;
using ElectricityDefect.WebUI.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;

namespace ElectricityDefect.WebUI.Controllers
{
    public class ParametersSetiServiceController : Controller
    {
        private ParametersSetiService repo;
        private IAlert alert;
        public ParametersSetiServiceController(IACRepository ac_repo, IRepository _repo,IAlert _alert)
        {
            repo = new ParametersSetiService(ac_repo, _repo);
            alert = _alert;
        }

        public ActionResult GridRead([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "points")] IEnumerable<long> points, DateTime datebegin, DateTime dateend,bool ofr)
        {
            var result = Enumerable.Empty<TestSetiViewModel>();
            
            if(points != null)
            {
                points = points.Where(w => w > 0);
                if (ofr)
                {
                    result = repo.GetTestsOutOfRange(points.ToArray(), datebegin, dateend);

                }
                else
                {
                    result = repo.GetTestByPeriod(points.ToArray(), datebegin, dateend);
                }
            }
          

            return Json(result.OrderByDescending(o => o.testtime).ToDataSourceResult(request));
        }

        public ActionResult TooltipIncident(long n_sh,string testtime)
        {
           
            var time = DateTime.Parse(testtime.Substring(0, 10)+" " +testtime.Substring(10,8)); 
            var incident = repo.GetIncident(n_sh,time);
            if(incident == null)
            {
                incident = new IncidentViewModel() { id = 0, n_sh = n_sh, begin = time, end = time };
            }
            return PartialView("~/Views/ParametersSeti/ToolTipIncident.cshtml", incident);
        }

        public ActionResult TreeRead(int? id)
        {
            var result = repo.GetTreePoints(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckAndAlert()
        {
            try
            {
                repo.GetDefects().ToList().ForEach(d => alert.Alert(d));

                return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { message = "Ошибка: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult DropDownListReadObjekts(string text)
        {
            var objekts = repo.GetOBEKTs().Where(o=>o.name.ToLower().Contains(text.ToLower()) || o.id.ToString().Contains(text));
            return Json(objekts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DropDownListReadSHs(long? n_ob, string text)
        {
            var shs = Enumerable.Empty<SHViewModel>();
            if (n_ob.HasValue)
            {
                shs = repo.GetSHs(new long[] { (long)n_ob }).Where(w => String.IsNullOrEmpty(text) ? true : w.id.ToString().ToLower().Contains(text.ToLower()) || w.name.ToLower().Contains(text.ToLower())) ;
            }
            return Json(shs, JsonRequestBehavior.AllowGet);
        }

       
    }
}