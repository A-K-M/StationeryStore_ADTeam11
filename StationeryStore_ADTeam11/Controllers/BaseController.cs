﻿using System;
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
            Session["role"] = employee.Role;
            ViewData["error"] = error;
            switch (employee.Role)
            {
                case "Head":
                    return RedirectToAction("HeadIndex", "DepartmentHead");
                case "Employee":
                    return RedirectToAction("EmployeeIndex", "DepartmentEmployee");
                case "Representative":
                    return RedirectToAction("RepIndex", "DepartmentRepresentative");
                case "Manager":
                    return RedirectToAction("ManagerIndex", "StoreManager");
                case "Supervisor":
                    return RedirectToAction("SuperviosrIndex", "StoreSupervisor");
                case "Clerk":
                    return RedirectToAction("ClerkIndex", "StoreClerk");
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
            return RedirectToAction("Base", "Login");
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


    }
}