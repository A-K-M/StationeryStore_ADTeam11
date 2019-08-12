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
            RequestDAO request = new RequestDAO();

            ViewData["Requisitions"] = request.GetRequistionListByEmpId(11233);
            return View();
        }

        public ActionResult CancelRequest(string id)
        {
            RequestDAO request = new RequestDAO();

            //if (request.CancelRequest(id, 11233) == false)
            //{
            //    ViewBag.Javascript = "<script>alert('Something went wrong! Please try again later!')</script>";
            //    return RedirectToAction("RequisitionList");
            //}

            //ViewBag.Javascript = "<script>alert('Your request has been cancelled successfully!')</script>";
            return RedirectToAction("RequisitionList");
        }
    }
}