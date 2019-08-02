using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_departmentHeadLayout")]
    [AuthenticationFilter]
    [RoleFilter("Head")]

    public class DepartmentHeadController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReviewStationeryRequest()
        {
            RequestDAO reqlist = new RequestDAO();
            ViewData["reqlist"] = reqlist.GetRequestList();
            return View();
        }

        public ActionResult ViewPendingRequestDetails(string reqId)
        {
            RequestDAO penReq = new RequestDAO();
            ViewData["penReq"] = penReq.ViewPendingRequestDetails(reqId);
            ViewData["reqId"] = reqId;
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