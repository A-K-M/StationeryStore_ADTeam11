using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Filters
{
    public class LayoutFilter : ActionFilterAttribute
    {
        private readonly string _masterName;

        public LayoutFilter(string masterName)
        {
            _masterName = masterName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;

            if (result != null)
            {
                result.MasterName = _masterName;
            }
        }
    }
}