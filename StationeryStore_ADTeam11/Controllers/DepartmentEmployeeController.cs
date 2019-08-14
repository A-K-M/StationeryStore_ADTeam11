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
    public class DepartmentEmployeeController : BaseController
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

        public ActionResult RequestDetail(int id)
        {
            RequestDAO requestDAO = new RequestDAO();
            ViewData["RequestDetail"] = requestDAO.GetRequestDetail(id);

            return View();
        }

        public ActionResult CancelRequest(string id)
        {
            RequestDAO request = new RequestDAO();

            switch (request.CancelRequest(id, 11233))
            {
                case ("success"):
                    {
                        SetFlash(Enums.FlashMessageType.Success, "Successfully Cancelled!");
                        return RedirectToAction("RequisitionList");                        
                    }
                case ("failed"):
                    {
                        SetFlash(Enums.FlashMessageType.Error, "Something went wrong! Please try again later or contact your webmaster.");
                        return RedirectToAction("RequisitionList");
                    }
                case ("unauthorized"):
                    {
                        SetFlash(Enums.FlashMessageType.Warning, "You cannot cancel requests which are not yours!");
                        return RedirectToAction("RequisitionList");
                    }
                case ("reviewed"):
                    {
                        SetFlash(Enums.FlashMessageType.Warning, "You cannot cancel requests which are already reviewed!");
                        return RedirectToAction("RequisitionList");
                    }
            }

            return RedirectToAction("RequisitionList");
        }
    }
}