using System.Web.Mvc;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using ElectricityDefect.WebUI.IoC;
using System.Data.Entity;
using ElectricityDefect.Domain.Services;
using ElectricityDefect.Domain.Migrations;

namespace ElectricityDefect.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // dependency injection
            var kernel = NinjectIoC.Initialize();
            // Web Api
            GlobalConfiguration.Configuration.DependencyResolver = new Ninject.Web.WebApi.NinjectDependencyResolver(kernel);
            // MVC 
            System.Web.Mvc.DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBEntities, Configuration>());

          
        }

        
    }
}
