using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.DAOs;

namespace StationeryStore_ADTeam11.Controllers
{
    public class DepartmentEmployeeController : Controller
    {
        // GET: DepartmentEmployee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewDelegationStatus()
        {
            int empId = 11237;
            // String today = DateTime.Now.ToString("yyyy/MM/dd");
            DateTime today = DateTime.Today;
            DelegationDAO dele = new DelegationDAO();
            ViewData["delegation"] = dele.GetDelegationStatus(empId);
            ViewData["today"] = today;
            return View();
        }
    }
}