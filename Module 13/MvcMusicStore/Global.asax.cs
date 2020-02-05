using MvcMusicStore.Infrastructure;
using MvcMusicStore.Logging;
using PerformanceCounterHelper;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IUserLogger logger;
        public MvcApplication()
        {
            if (ConfigurationManager.AppSettings["Logging"].AsBool())
                logger = new Logger();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger?.Info("Application started");

            using (var counterHelper = PerformanceHelper.CreateCounterHelper<PerfomanceCounters>("Mvc project"))
            {
                counterHelper.RawValue(PerfomanceCounters.VisitHomePage, 0);
                counterHelper.RawValue(PerfomanceCounters.LogIn, 0);
                counterHelper.RawValue(PerfomanceCounters.LogOff, 0);
            }
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            logger.Error(exception.ToString());
        }
    }
}
