using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Filters;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.DAOs;

namespace StationeryStore_ADTeam11.Controllers
{
    //[AuthenticationFilter]
    //[RoleFilter("Manager")]
    [LayoutFilter("_storeManagerLayout")]
    public class StoreManagerController : BaseController
    {
        // GET: StoreManager
        public ActionResult Index()
        {
            //Session Objects will have to be edited when including session objects later
            Session["Username"] = "User";
            Session["Role"] = "Role";
            return View();
        }

        public ActionResult Suppliers()
        {
           
            Session["Username"] = "User";
            Session["Role"] = "Role";

            List<Supplier> suppliers = new SupplierDAO().GetAllSuppliers();

            ViewData["Suppliers"] = suppliers;
            return View();
        }

        public ActionResult CreateSupplier()
        {
            Session["Username"] = "User";
            Session["Role"] = "Role";

            return View();
        }

        public ActionResult AddSupplier(Supplier supplier)
        {
            SupplierDAO suppDAO = new SupplierDAO();
            Session["Username"] = "User";
            Session["Role"] = "Role";

            bool saved = false;
            //string duplicateMsg = "supplier ID already exist";

            Supplier existingSupp = suppDAO.FindSupplierById(supplier.Id);

            if (supplier.Id == existingSupp.Id)
            {
                SetFlash(Enums.FlashMessageType.Error, "Supplier information for supplier name: " + supplier.Name + " was not added due to duplicate Supplier ID: " + supplier.Id + ".");
                return RedirectToAction("Suppliers");
            }
            else if (supplier.Id != existingSupp.Id)
            {
                saved = suppDAO.AddSupplier(supplier);
                if (saved)
                {
                    SetFlash(Enums.FlashMessageType.Success, "Supplier record for Supplier ID: " + supplier.Id + " successfully added!");
                    return RedirectToAction("Suppliers");
                }

                SetFlash(Enums.FlashMessageType.Error, "Failed to add supplier record for Supplier ID: " + supplier.Id + ".");
                return RedirectToAction("Suppliers");

            }
            else
            {
                SetFlash(Enums.FlashMessageType.Error, "Supplier record for Supplier ID: " + supplier.Id + " not saved.");
                return RedirectToAction("Suppliers");
            }

            //else
            //{
            //    ViewData["duplicateMsg"] = duplicateMsg;
            //}

            //ViewData["saved"] = saved;
            //return View();
        }

        public ActionResult EditSupplier(Supplier supp)
        {
            SupplierDAO suppDAO = new SupplierDAO();
            Session["Username"] = "User";
            Session["Role"] = "Role";

            Supplier supplier = suppDAO.EditSupplier(supp.Id);

            ViewData["supplier"] = supplier;
            return View();
        }

        public ActionResult UpdateSupplier(Supplier supp)
        {
            SupplierDAO suppDAO = new SupplierDAO();
            Session["Username"] = "User";
            Session["Role"] = "Role";

            bool updated = suppDAO.UpdateSupplier(supp);
            if (updated)
            {
                SetFlash(Enums.FlashMessageType.Success, "Successfully updated supplier record for Supplier ID: " + supp.Id + ".");
                return RedirectToAction("Suppliers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Failed to update supplier record for Supplier ID: " + supp.Id + ".");
            return RedirectToAction("Suppliers");
            //ViewData["updated"] = updated;
            //ViewData["supplier"] = supp;
            //return View();
        }

        public ActionResult DeleteSupplier(string id)
        {
            SupplierDAO suppDAO = new SupplierDAO();
            Session["Username"] = "User";
            Session["Role"] = "Role";

            bool deleted = suppDAO.DeleteSupplier(id);
            if (deleted)
            {
                SetFlash(Enums.FlashMessageType.Success, "Supplier record for Ssupplier ID: " + id + " successfully deleted.");
                return RedirectToAction("Suppliers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Supplier record for Supplier ID: " + id + " was not deleted.");
            return RedirectToAction("Suppliers");

            //ViewData["deleted"] = deleted;
            //ViewData["id"] =id;
            //return View();
        }

        public ActionResult AdjustmentVouchers()
        {
            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            ViewData["AdjustmentVouchers"] = adjustmentVoucherDAO.GetByStatusForManager("Pending");

            return View();
        }

        
        public JsonResult FilterAdjustmentVouchers(string id) //Since we are using default route the parameter name must be id instead of status unless we wanna modify routes
        {
            AdjustmentVoucherDAO adjustment = new AdjustmentVoucherDAO();

            return Json(adjustment.GetByStatusForManager(id), JsonRequestBehavior.AllowGet);
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

            adjustmentVoucherDAO.ReviewAdjustmentVoucher(id, "Approved", adjustmentVoucherDAO.GetVoucherItems(id));

            return RedirectToAction("AdjustmentVouchers");
        }

        public ActionResult RejectAdjustmentVoucher(int id)
        {
            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            adjustmentVoucherDAO.ReviewAdjustmentVoucher(id, "Rejected", adjustmentVoucherDAO.GetVoucherItems(id));

            return RedirectToAction("AdjustmentVouchers");
        }
    }
}