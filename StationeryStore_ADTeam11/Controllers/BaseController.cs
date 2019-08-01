using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Filters;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.Controllers
{
    public class BaseController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            string error = null;
            ViewData["error"] = error;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
       public ActionResult Login(string username, string password)
        {
            string error = null;
            EmployeeDAO employeeDAO = new EmployeeDAO();
            Employee employee = employeeDAO.GetEmployeeByUsername(username);

            if(employee == null)
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
            Session["role"] = employee.Role;
            ViewData["error"] = error;
            return RedirectToAction( "HeadIndex", "DepartmentHead");
           // return View();
        }

        [AuthenticationFilter]
        public ActionResult LogOut()
        {
            Session["username"] = null;
            Session["role"] = null;
            return RedirectToAction("Base", "Login");
        }

        [AuthenticationFilter]
        public ActionResult RedirectBack()
        {

            string error = "You dont have permission to access this page.";
            ViewData["error"] = error;
            return Redirect(Request.UrlReferrer.ToString());
        }


    }
}