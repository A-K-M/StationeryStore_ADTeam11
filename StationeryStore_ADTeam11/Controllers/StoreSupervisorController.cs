using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_storeSupervisorLayout")]
    public class StoreSupervisorController : Controller
    {
        // GET: StoreSupervisor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdjustmentVouchers()
        {
            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            ViewData["AdjustmentVouchers"] = adjustmentVoucherDAO.GetAll();

            return View();
        }
    }
}