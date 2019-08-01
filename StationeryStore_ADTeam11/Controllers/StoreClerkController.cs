using StationeryStore_ADTeam11.Filters;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_storeClerkLayout")]
    public class StoreClerkController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAdjustmentVoucher()
        {
            CategoryDAO categoryDAO = new CategoryDAO();

            List<Category> categories = new List<Category>();

            categories = categoryDAO.GetAll();

            ViewData["Categories"] = categories;

            return View();
        }

        public JsonResult GetItemByCategory(int id)
        {
            ItemDAO itemDAO = new ItemDAO();

            return Json(itemDAO.GetItems(id), JsonRequestBehavior.AllowGet);
        }
    }
}