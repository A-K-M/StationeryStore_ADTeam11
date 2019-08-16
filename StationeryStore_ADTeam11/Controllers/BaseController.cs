using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Filters;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.Enums;
using StationeryStore_ADTeam11.MobileModels;

namespace StationeryStore_ADTeam11.Controllers
{
    public class BaseController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (ViewData["error"] == null)
            {
                string error = null;
                ViewData["error"] = error;
            }
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
       public ActionResult Login(string username, string password)
        {
            string error = null;
            EmployeeDAO employeeDAO = new EmployeeDAO();
            Employee employee = employeeDAO.GetEmployeeByUsername(username);

            if(employee == null || employee.Role == null)
            {
                error = "No Employee with this name.";
                ViewData["error"] = error;
                return View();
            }
            if (employee.Password != password)
            {
                error = "Wrong password.";
                ViewData["error"] = error;
                return View(); 
            }
            string sessionID = Guid.NewGuid().ToString();
            Session["sessionID"] = sessionID;
            Session["username"] = employee.UserName;
            Session["userid"] = employee.Id;
            Session["role"] = employee.Role;
            Session["deptID"] = employee.DepartmentId;
            ViewData["error"] = error;
            switch (employee.Role)
            {
                case Constant.ROLE_HEAD:
                    return RedirectToAction("Index", "DepartmentHead");

                case Constant.ROLE_EMPLOYEE:
                    return RedirectToAction("Index", "DepartmentEmployee");

                case Constant.ROLE_REPRESENTATIVE:
                    return RedirectToAction("Index", "DepartmentEmployee");

                case Constant.ROLE_MANAGER:
                    return RedirectToAction("Index", "StoreManager");

                case Constant.ROLE_SUPERVISOR:
                    return RedirectToAction("Index", "StoreSupervisor");

                case Constant.ROLE_CLERK:
                    return RedirectToAction("Index", "StoreClerk");

                default:
                    break;
            }
            return RedirectToAction("ShowError","Base");

        }

        [AuthenticationFilter]
        public ActionResult LogOut()
        {
            Session["sessionID"] = null;
            Session["username"] = null;
            Session["role"] = null;
            return RedirectToAction("Login", "Base");
        }

        [AuthenticationFilter]
        public ActionResult RedirectBack()
        {
            return Redirect(Request.UrlReferrer.ToString());
        }


        [AllowAnonymous]
        public ActionResult ShowError()
        {
            string error = "You dont have permission to access this page.";
            ViewData["error"] = error;
            return View();
        }

        public void SetFlash(FlashMessageType type, string text)
        {
            TempData["FlashMessage.Type"] = type;
            TempData["FlashMessage.Text"] = text;
        }
    }
}