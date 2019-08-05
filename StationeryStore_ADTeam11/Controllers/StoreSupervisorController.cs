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

            ViewData["AdjustmentVouchers"] = adjustmentVoucherDAO.GetByStatus("Pending");

            return View();
        }

        public JsonResult FilterAdjustmentVouchers(string id) //Since we are using default route the parameter name must be id instead of status unless we wanna modify routes
        {
            AdjustmentVoucherDAO adjustment = new AdjustmentVoucherDAO();

            return Json(adjustment.GetByStatus(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VoucherItems(int id)
        {
            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            ViewData["VoucherItems"] = adjustmentVoucherDAO.GetVoucherItems(id);

            return View();
        }
    }
}