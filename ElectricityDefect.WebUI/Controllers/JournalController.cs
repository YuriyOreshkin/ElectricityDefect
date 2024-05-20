using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectricityDefect.WebUI.Controllers
{
    public class JournalController : Controller
    {
        // GET: Limits
        public ActionResult Index()
        {
            ViewData["fasa"] = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "A" },
                new SelectListItem { Value = "2", Text = "B" },
                new SelectListItem { Value = "3", Text = "C" },

            };
            return View();
        }
    }
}