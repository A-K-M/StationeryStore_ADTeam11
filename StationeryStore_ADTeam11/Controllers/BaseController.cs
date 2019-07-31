using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult Login()
        {
            return View();
        }
    }
}