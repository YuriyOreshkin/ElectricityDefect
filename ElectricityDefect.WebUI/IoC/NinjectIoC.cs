using ElectricityDefect.Domain.Services;
using Ninject;
using Ninject.Web.WebApi.Filter;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ElectricityDefect.WebUI.IoC
{
    public static class NinjectIoC
    {
        public static IKernel Initialize()
        {
            IKernel kernel = new StandardKernel();
            
            AddBindings(kernel);
            return kernel;
        }

        private static IKernel AddBindings(IKernel ninjectKernel)
        {
            ninjectKernel.Bind<DefaultFilterProviders>().ToConstant(new DefaultFilterProviders(new[] { new NinjectFilterProvider(ninjectKernel) }.AsEnumerable()));
            ninjectKernel.Bind<DefaultModelValidatorProviders>().ToConstant(new DefaultModelValidatorProviders(GlobalConfiguration.Configuration.Services.GetModelValidatorProviders()));
            
            //Repositories
            ninjectKernel.Bind<IRepository>().To<EFRepository>();
            ninjectKernel.Bind<IACRepository>().To<OracleClientACRepository>().InSingletonScope().WithConstructorArgument("_connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["ACEntities"].ConnectionString);

            ninjectKernel.Bind<IMailServiceConfig>().To<XMLMailServiceConfig>().InSingletonScope().WithConstructorArgument("_filename", HttpContext.Current.Server.MapPath("~/App_Data/MailSettings.xml"));
            ninjectKernel.Bind<ICryptoService>().To<CryptoService>().InSingletonScope();
            ninjectKernel.Bind<ITemplateService>().To<FileTemplateService>().InSingletonScope().WithConstructorArgument("_anchor","alert").WithConstructorArgument("_filename", HttpContext.Current.Server.MapPath("~/App_Data/MailTemplate.html"));

            ninjectKernel.Bind<ISender>().To<EMailSender>().InSingletonScope();
            ninjectKernel.Bind<IAlert>().To<EMailAlert>().InSingletonScope();
            return ninjectKernel;
        }
    }
}