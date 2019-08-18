using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.Util;

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

        public string GetDeptId()
        {
            string deptId = employeeDAO.GetDepartmentIdByDepartmentHeadUsername(Session["username"].ToString());

            return deptId;
        }
        public ActionResult Delegation()
        {
            deptId = GetDeptId();

            List<Employee> employees = employeeDAO.GetEmployeeByDeptId(deptId);
            IEnumerable<Delegation> Delegations = null;

            List<Delegation> delegations = new List<Delegation>();
            foreach (var e in employees)
            {
                List<Delegation> deles = delegationDAO.GetDelegationsByEmpId(e.Id,deptId);
                if(deles!=null)
                {
                    foreach (var dele in deles)
                    {
                        dele.EmployeeName = employeeDAO.GetEmployeeById(dele.EmployeeId).Name;
                        delegations.Add(dele);
                    }
                    
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
            deptId = GetDeptId();
            status = "Ongoing";
            string currentStatus = departmentDAO.GetDepartmentByDeptId(deptId).DelegateStatus;
            if (currentStatus==status)
            {
                SetFlash(Enums.FlashMessageType.Error, "You've already granted authourity to someone!");

                return RedirectToAction("Delegation", "DepartmentHead");
            }
            else
            {
                delegation.EmployeeId = employeeDAO.GetEmployeeByName(deptId, delegation.EmployeeName).Id;

                if (!departmentDAO.InsertDelegation(delegation, deptId))
                {
                    SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");

                    return RedirectToAction("Delegation", "DepartmentHead");
                }
                ///// start Email /////
                EmailDAO emailDAO = new EmailDAO();
                Employee employee = emailDAO.EmailDelegation(delegation.EmployeeId);
                Email email = new Email();
                email.SendEmail(employee.Email, "Delegation", "Please Check delegation status");
                ///////////////////////
                ///
                SetFlash(Enums.FlashMessageType.Success, "Authourity successfully granted!");
                //delegationDAO.CreateDelegation(delegation);
                //departmentDAO.UpdateDepartmentDelegation(deptId, delegation.EmployeeId, status);

                return RedirectToAction("Delegation", "DepartmentHead");
            }
            
        }
        public ActionResult CancelDelegation(int Id)
        {
            deptId = GetDeptId();
            DelegationDAO delegationDAO = new DelegationDAO();
            //status = "completed";
            //departmentDAO.UpdateDepartmentDelegation(deptId, delegationDAO.GetDelegationById(Id).EmployeeId, status);
            //delegationDAO.CancelDelegation(Id);
            if (!departmentDAO.CancelDelegation(deptId, Id))
            {
                SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");

                return RedirectToAction("Delegation", "DepartmentHead");
            }
            ///// start Email /////
            Delegation delegation = delegationDAO.GetDelegationById(Id);
            EmailDAO emailDAO = new EmailDAO();
            Employee employee = emailDAO.EmailDelegation(delegation.EmployeeId);
            Email email = new Email();
            email.SendEmail(employee.Email, "Delegation", "Dear " + delegation.EmployeeName + ",Please Check delegation status");
            ///////////////////////
            SetFlash(Enums.FlashMessageType.Success, "You've cancelled Authority Delegation!");

            return RedirectToAction("Delegation", "DepartmentHead");
        }

        public ActionResult CollectionPoint()
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>();
            collectionPoints = collectionPointDAO.GetCollectionPoints(); //Getting all 6 collection point locations
            ViewData["collectionPoints"] = collectionPoints;

            deptId = GetDeptId();// departmentID of current logined DepartmentHead

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
            string deptId = employeeDAO.GetDepartmentIdByDepartmentHeadUsername(Session["username"].ToString());
            
            if (point.CollectionPoinId==0 || point.RepName == null)
            {
                SetFlash(Enums.FlashMessageType.Error, "Please select both category!");

                return RedirectToAction("CollectionPoint", "DepartmentHead");
            }
            else
            {
                int RepId = employeeDAO.GetEmployeeByName(deptId, point.RepName).Id;

                //departmentDAO.UpdateDepartmentCollectionPoint(deptId, point.CollectionPoinId);

                //if (!departmentDAO.UpdateDepartmentRepresentative(deptId, RepId))
                //{
                //    SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");

                //    return RedirectToAction("CollectionPoint", "DepartmentHead");
                //}

                if (!departmentDAO.UpdateDeptRepAndColPt(deptId, RepId, point.CollectionPoinId))
                {
                    SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");

                    return RedirectToAction("CollectionPoint", "DepartmentHead");
                }

                ///// Email start ////////
                EmailDAO emailDAO = new EmailDAO();
                Employee employee = emailDAO.EmailUpdateDepartmentRep(point.CollectionPoinId);
                Email email = new Email();
                string message = "Dear " + employee.Name + ", Please Check for Updated department Representative";
                email.SendEmail(employee.Email, "New Stationery Request", message);
                //////////////////////////
                
                SetFlash(Enums.FlashMessageType.Success, "Operation Succeeded!");

                return RedirectToAction("CollectionPoint", "DepartmentHead");
            }            
            
        }

        public ActionResult ReviewStationeryRequest()
        {
            RequestDAO reqlist = new RequestDAO();
            ViewData["reqlist"] = reqlist.GetRequestList();
            return View();
        }

        public ActionResult ViewPendingRequestDetails(int id)
        {
            RequestDAO requestDAO = new RequestDAO();
            ViewData["PendingRequests"] = requestDAO.ViewPendingRequestDetails(id);

            return View();
        }

        public ActionResult ApproveRejectRequest(string status, int reqId)
        {
            RequestDAO chngStatus = new RequestDAO();
            chngStatus.UpdateStatus(status, reqId);
            EmailDAO emailDAO = new EmailDAO();
            Employee employee =emailDAO.EmailRequestStatus(reqId);
            string sender = Session["username"].ToString();
            Email email = new Email();
            email.SendEmail(employee.Email, "Requesation Status", "Dear" + employee.Name + ",   \n Please check for Requestsation status. Regards,\n" + sender);
            return RedirectToAction("ReviewStationeryRequest", "DepartmentHead");

        }
    }
}
        

 
