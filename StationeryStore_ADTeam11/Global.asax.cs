using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StationeryStore_ADTeam11
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                            "Default",                                              // Route name
                            "{controller}/{action}/{id}",                           // URL with parameters
                            new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
                        );

            routes.MapRoute(
                  "MarkDelivered",
                  "{controller}/{action}/{pid}/{id}",
                  new { controller = "StoreClerk", action = "", id = ""}
                );
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
