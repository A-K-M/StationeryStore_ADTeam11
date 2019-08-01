﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.Controllers
{
    public class StoreManagerController : Controller
    {
        // GET: StoreManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Suppliers()
        {
            List<Supplier> suppliers = SupplierDAO.getAllSuppliers();

            ViewData["Suppliers"] = suppliers;
            return View();
        }

        public ActionResult AddSupplier(Supplier supplier)
        {
            bool saved = SupplierDAO.addSupplier(supplier);

            ViewData["saved"] = saved;
            return View();
        }

        public ActionResult EditSupplier(Supplier supp)
        {
            Supplier supplier = SupplierDAO.editSupplier(supp.Id);

            ViewData["supplier"] = supplier;
            return View();
        }

        public ActionResult UpdateSupplier(Supplier supp)
        {
            bool updated = SupplierDAO.updateSupplier(supp);

            ViewData["updated"] = updated;
            ViewData["supplier"] = supp;
            return View();
        }

        public ActionResult DeleteSupplier(string id)
        {
            bool deleted = SupplierDAO.deleteSupplier(id);

            ViewData["deleted"] = deleted;
            ViewData["id"] =id;
            return View();
        }
    }
}