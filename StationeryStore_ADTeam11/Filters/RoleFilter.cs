
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StationeryStore_ADTeam11.Filters
{
    public class RoleFilter : ActionFilterAttribute
    {
        private readonly string _role;

        public RoleFilter (string role)
        {
            if (role == "Employee,Representative")
            {
                _role = "er";
            }
            else if(role == "Representative")
            {
                _role = "r";
            }
            else
            {
                _role = role;

            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string userRole = HttpContext.Current.Session["role"].ToString();

            if (userRole == "Representative" && _role == "er")
            {
                userRole = "er";
            }else if (userRole == "Employee" && _role == "er")
            {
                userRole = "er";
            }else if (userRole == "Representative" && _role == "r")
            {
                userRole = "r";
            }


            if (userRole != _role)
            {

                filterContext.Result = new RedirectToRouteResult(

                                           new RouteValueDictionary
                                           {
                                                {"controller", "base"},
                                                { "action", "ShowError"}
                                           });
            }
        }


    }
}