using ADProjectTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_departmentHeadLayout")]
    [AuthenticationFilter]
    public class DepartmentHeadController : Controller
    {
        public ActionResult HeadIndex()
        {
            return View();
        }
      
    }
}