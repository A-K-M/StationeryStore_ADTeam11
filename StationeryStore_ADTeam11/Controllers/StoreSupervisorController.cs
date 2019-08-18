using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11.Controllers
{
    [AuthenticationFilter]
    [RoleFilter("Supervisor")]
    [LayoutFilter("_storeSupervisorLayout")]
    public class StoreSupervisorController : BaseController
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

        public ActionResult ApproveAdjustmentVoucher(int id)
        {
            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            bool result = adjustmentVoucherDAO.ReviewAdjustmentVoucher(id, "Approved", adjustmentVoucherDAO.GetVoucherItems(id));

            if (result)
            {
                SetFlash(Enums.FlashMessageType.Success, "Successfully Approved");
                return RedirectToAction("AdjustmentVouchers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong. Please try again later!");
            return RedirectToAction("AdjustmentVouchers");
        }

        public ActionResult RejectAdjustmentVoucher(int id)
        {
            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            bool result = adjustmentVoucherDAO.ReviewAdjustmentVoucher(id, "Rejected", adjustmentVoucherDAO.GetVoucherItems(id));

            if (result)
            {
                SetFlash(Enums.FlashMessageType.Success, "Successfully Rejected!");
                return RedirectToAction("AdjustmentVouchers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong. Please try again later!");
            return RedirectToAction("AdjustmentVouchers");
        }

        public ActionResult ReorderStockList()
        {
            PurchaseOrderDAO purchaseOrder = new PurchaseOrderDAO();

            ViewData["StockList"] = purchaseOrder.ReorderStockLists("Pending");

            return View();
        }

        public ActionResult ReorderStockDetail(int id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            ViewData["Items"] = purchaseOrderDAO.ReorderStockDetail(id);

            return View();
        }

        public ActionResult ApproveReorderStock(int id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            bool result = purchaseOrderDAO.ReviewReorderStock(id, "Approved");

            if (result)
            {
                SetFlash(Enums.FlashMessageType.Success, "Approved!");
                return RedirectToAction("ReorderStockList");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong please try again later!");
            return RedirectToAction("ReorderStockList");
        }

        public ActionResult RejectReorderStock(int id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            bool result = purchaseOrderDAO.ReviewReorderStock(id, "Rejected");

            if (result)
            {
                SetFlash(Enums.FlashMessageType.Success, "Rejected!");
                return RedirectToAction("ReorderStockList");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong please try again later!");
            return RedirectToAction("ReorderStockList");
        }
    }
}