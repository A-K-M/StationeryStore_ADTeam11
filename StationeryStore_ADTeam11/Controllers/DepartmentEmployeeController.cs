using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;
using StationeryStore_ADTeam11.View_Models;
using StationeryStore_ADTeam11.Util;

namespace StationeryStore_ADTeam11.Controllers
{

    [AuthenticationFilter]
    [RoleFilter("Employee,Representative")]
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

        public JsonResult GetItemByCategory(int id)
        {
            ItemDAO itemDAO = new ItemDAO();

            return Json(itemDAO.GetItems(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequisitionList()
        {
            RequestDAO request = new RequestDAO();

            ViewData["Requisitions"] = request.GetRequistionListByEmpId(Convert.ToInt32(Session["userid"].ToString())); //PUT LOGIN EMP_ID HERE
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

            switch (request.CancelRequest(id, Convert.ToInt32(Session["userid"].ToString()))) //PUT LOGIN EMP_ID HERE
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

        //DELETE FROM HERE IF SOMETHING WRONG!
        
        public ActionResult CreateStationeryRequest()
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            ViewData["Categories"] = categoryDAO.GetAll();
            return View();
        }

        [HttpPost]
        public JsonResult AddStationeryRequest(List<RequestStationery> itemData)
        {
            if (itemData == null)
            {
                return Json("Your item list is empty!", JsonRequestBehavior.AllowGet);
            }

            // add itemid to array
            int listLength = itemData.Count;
            string[] itemid = new string[listLength];

            for (int i = 0; i < listLength; i++)
            {
                itemid[i] = itemData[i].ItemId;
            }
            ////////////////////////////////////////////

            ///////////////get employee//////////////
            EmployeeDAO employeeDAO = new EmployeeDAO();
            Employee emp = new Employee();
            emp = employeeDAO.GetEmployeeByUsername(Session["username"].ToString());
            int empId = emp.Id;
            string deptId = emp.DepartmentId;
            ///////////////////////////////////////////
            RequestDAO req = new RequestDAO();
            List<RequestStationery> reqStationery = new List<RequestStationery>();
            reqStationery = itemData;
            if (req.CreateRequest(empId, reqStationery))
            {
                EmailDAO emailDAO = new EmailDAO();
                string mgrEmail = emailDAO.EmailReviewStationeryRequest(empId);
                Email email = new Email();
                string message = "Please check for new stationery Request. Do not reply this is system generated message.Thanks " + emp.Name;
                email.SendEmail(mgrEmail, "New Stationery Request", message);
                return Json("Successfully Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Something went wrong! Please try again later.", JsonRequestBehavior.AllowGet);
            }

        }

        //******************************************//


         
        public ActionResult ReviewStationeryRequest()
        {
            if (Session["delegation"].ToString() != "Yes")
            {
                return RedirectToAction("ShowError", "Base");
            }

            RequestDAO reqlist = new RequestDAO();
            ViewData["reqlist"] = reqlist.GetRequestList();
            return View();
        }

      
        public ActionResult ViewPendingRequestDetails(int id)
        {
            if (Session["delegation"].ToString() != "Yes")
            {
                return RedirectToAction("ShowError", "Base");
            }
            RequestDAO requestDAO = new RequestDAO();
            ViewData["PendingRequests"] = requestDAO.ViewPendingRequestDetails(id);

            return View();
        }

        
        public ActionResult ApproveRejectRequest(string status, int reqId)
        {
            if (Session["delegation"].ToString() != "Yes")
            {
                return RedirectToAction("ShowError", "Base");
            }
            RequestDAO chngStatus = new RequestDAO();
            chngStatus.UpdateStatus(status, reqId);
            EmailDAO emailDAO = new EmailDAO();
            Employee employee = emailDAO.EmailRequestStatus(reqId);
            string sender = Session["username"].ToString();
            Email email = new Email();
            email.SendEmail(employee.Email, "Requesation Status", "Dear" + employee.Name + ",   \n Please check for Requestsation status. Regards,\n" + sender);
            return RedirectToAction("ReviewStationeryRequest", "DepartmentEmployee");

        }
    }
}