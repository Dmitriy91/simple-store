using Assignment.Data;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using log4net.Config;
using System.IO;

namespace Assignment.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperWebConfig.RegisterMappingProfiles();
            AutofacWebConfig.Initialize(GlobalConfiguration.Configuration);
            Database.SetInitializer(new AppDbContextInitializer());
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/Configs/Log4Net.config")));
        }
    }
}
