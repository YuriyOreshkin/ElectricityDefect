using ElectricityDefect.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectricityDefect.WebUI.Controllers
{
    public class ParametersSetiController : Controller
    {
 
        public ParametersSetiController()
        {
           
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid()
        {
            return PartialView();
        }
    }
}