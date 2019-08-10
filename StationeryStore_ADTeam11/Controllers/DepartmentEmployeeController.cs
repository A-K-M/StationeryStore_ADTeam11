using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_deptartmentEmployeeLayout")]
    public class DepartmentEmployeeController : Controller
    {
        // GET: DepartmentEmployee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewDelegationStatus()
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            Employee emp = new Employee();
            emp = employeeDAO.GetEmployeeByUsername(Session["username"].ToString());
            int empId = emp.Id;
            //int empId = 11237;
            // String today = DateTime.Now.ToString("yyyy/MM/dd");
            DateTime today = DateTime.Today;
            DelegationDAO dele = new DelegationDAO();
            ViewData["delegation"] = dele.GetDelegationStatus(empId);
            ViewData["today"] = today;
            return View();
        }

        public ActionResult RequisitionList()
        {
            return View();
        }
    }
}