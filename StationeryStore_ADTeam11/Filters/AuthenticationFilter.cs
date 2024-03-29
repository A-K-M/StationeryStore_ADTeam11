﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace StationeryStore_ADTeam11.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string sessionId = null;

            try
            {
                sessionId = HttpContext.Current.Session["sessionID"].ToString();
            }
            catch (Exception e)
            {
                if (sessionId == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                        { "controller", "base" },
                        { "action", "Login" }
                        }
                    );
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(

                                            new RouteValueDictionary
                                            {
                                                {"controller", "base"},
                                                { "action", "Login"}
                                            });
            }
        }
    }

}