using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace StationeryStore_ADTeam11
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
             name: "DepartmentHeadApiRoute",
             routeTemplate: "api/department/",
             defaults: new { controller = "DepartmentHeadApi", deptId = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
