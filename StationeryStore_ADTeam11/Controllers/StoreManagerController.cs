using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Filters;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_storeManagerLayout")]
    public class StoreManagerController : Controller
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

            List<Supplier> suppliers = SupplierDAO.getAllSuppliers();

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
            Session["Username"] = "User";
            Session["Role"] = "Role";

            bool saved = false;
            string duplicateMsg = "supplier ID already exist";

            Supplier existingSupp = SupplierDAO.findSupplierbyId(supplier.Id);
            if (supplier.Id != existingSupp.Id)
            {
                saved = SupplierDAO.addSupplier(supplier);
            }
            else
            {
                ViewData["duplicateMsg"] = duplicateMsg;
            }

            ViewData["saved"] = saved;
            return View();
        }

        public ActionResult EditSupplier(Supplier supp)
        {
            Session["Username"] = "User";
            Session["Role"] = "Role";

            Supplier supplier = SupplierDAO.editSupplier(supp.Id);

            ViewData["supplier"] = supplier;
            return View();
        }

        public ActionResult UpdateSupplier(Supplier supp)
        {
            Session["Username"] = "User";
            Session["Role"] = "Role";

            bool updated = SupplierDAO.updateSupplier(supp);

            ViewData["updated"] = updated;
            ViewData["supplier"] = supp;
            return View();
        }

        public ActionResult DeleteSupplier(string id)
        {
            Session["Username"] = "User";
            Session["Role"] = "Role";

            bool deleted = SupplierDAO.deleteSupplier(id);

            ViewData["deleted"] = deleted;
            ViewData["id"] =id;
            return View();
        }
    }
}