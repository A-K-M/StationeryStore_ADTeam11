using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_departmentHeadLayout")]
    [AuthenticationFilter]
    [RoleFilter("Head")]

    public class DepartmentHeadController : BaseController
    {
        DepartmentDAO departmentDAO = new DepartmentDAO();

        DelegationDAO delegationDAO = new DelegationDAO();

        CollectionPointDAO collectionPointDAO = new CollectionPointDAO();

        EmployeeDAO employeeDAO = new EmployeeDAO();

        string deptId, status = "";

        public ActionResult Index()
        {
            return View();
        }

        public string GetDeptId(string username)
        {
            string deptId = employeeDAO.GetDepartmentIdByDepartmentHeadUsername(username);

            return deptId;
        }
        public ActionResult Delegation()
        {
            deptId = GetDeptId("helen");

            List<Employee> employees = employeeDAO.GetEmployeeByDeptId(deptId);
            IEnumerable<Delegation> Delegations = null;

            List<Delegation> delegations = new List<Delegation>();
            foreach (var e in employees)
            {
                List<Delegation> deles = delegationDAO.GetDelegationsByEmpId(e.Id);
                foreach (var dele in deles)
                {
                    dele.EmployeeName = employeeDAO.GetEmployeeById(dele.EmployeeId).Name;
                    delegations.Add(dele);
                }
                Delegations = delegations.OrderBy(Delegation => Delegation.Id);
            }

            List<SelectListItem> Employees = new List<SelectListItem>();
            foreach (var emp in employees)
            {
                Employees.Add(new SelectListItem { Value = emp.Name, Text = emp.Name });
            }

            ViewData["employees"] = Employees;

            ViewData["Delegations"] = Delegations;
            return View();
        }
        [HttpPost]
        public ActionResult Delegation(Delegation delegation)
        {
            deptId = GetDeptId("helen");
            status = "ongoing";

            delegation.EmployeeId = employeeDAO.GetEmployeeByName(deptId, delegation.EmployeeName).Id;

            delegationDAO.CreateDelegation(delegation);
            departmentDAO.UpdateDepartmentDelegation(deptId, delegation.EmployeeId, status);


            return RedirectToAction("Delegation", "DepartmentHead");
        }
        public ActionResult CancelDelegation(int Id)
        {
            deptId = GetDeptId("helen");
            status = "completed";

            departmentDAO.UpdateDepartmentDelegation(deptId, delegationDAO.GetDelegationById(Id).EmployeeId, status);

            delegationDAO.CancelDelegation(Id);

            return RedirectToAction("Delegation", "DepartmentHead");
        }

        public ActionResult CollectionPoint()
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>();
            collectionPoints = collectionPointDAO.GetCollectionPoints(); //Getting all 6 collection point locations
            ViewData["collectionPoints"] = collectionPoints;

            //hardcoded DeptHeadUsername ="helen"
            deptId = GetDeptId("helen");// departmentID of current logined DepartmentHead

            List<Employee> employees = new List<Employee>();
            employees = employeeDAO.GetEmployeeByDeptId(deptId);
            List<SelectListItem> Employees = new List<SelectListItem>();
            foreach (var emp in employees)
            {
                Employees.Add(new SelectListItem { Value = emp.Name, Text = emp.Name });
            }
            ViewData["employees"] = Employees;

            Department department = departmentDAO.GetDepartmentByDeptId(deptId);

            int CollectionPoint = department.CollectionPoinId;

            string repName = employeeDAO.GetEmployeeById(department.RepId).Name;
            string collectionPointName = collectionPointDAO.GetCollectionPointById(department.CollectionPoinId).Name;

            ViewData["representativeName"] = repName;
            ViewData["collectionPoint"] = collectionPointName;

            return View();
        }
        [HttpPost]
        public ActionResult CollectionPoint(Department point)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            string deptId = employeeDAO.GetDepartmentIdByDepartmentHeadUsername("helen");
            int RepId = employeeDAO.GetEmployeeByName(deptId, point.RepName).Id;

            DepartmentDAO departmentDAO = new DepartmentDAO();

            departmentDAO.UpdateDepartmentCollectionPoint(deptId, point.CollectionPoinId);
            departmentDAO.UpdateDepartmentRepresentative(deptId, RepId);

            if (!employeeDAO.UpdateUserRole(RepId))
            {
                SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");
                return RedirectToAction("Delegation", "DepartmentHead");
            }

            SetFlash(Enums.FlashMessageType.Success, "Operation Succeeded!");


            return RedirectToAction("CollectionPoint", "DepartmentHead");
        }

        public ActionResult ReviewStationeryRequest()
        {
            RequestDAO reqlist = new RequestDAO();
            ViewData["reqlist"] = reqlist.GetRequestList();
            return View();
        }

     
       

        public ActionResult ApproveRejectRequest(string status, string reqId)
        {
            RequestDAO chngStatus = new RequestDAO();
            chngStatus.UpdateStatus(status, reqId);
            return RedirectToAction("ReviewStationeryRequest", "DepartmentHead");

        }
    }
}
        

 
