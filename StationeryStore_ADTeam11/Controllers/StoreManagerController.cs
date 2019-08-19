using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Filters;
using System.Web.Mvc;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;
using StationeryStore_ADTeam11.DAOs;
using System.IO;

namespace StationeryStore_ADTeam11.Controllers
{
    [AuthenticationFilter]
    [RoleFilter("Manager")]
    [LayoutFilter("_storeManagerLayout")]
    public class StoreManagerController : BaseController
    {
        // GET: StoreManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Suppliers()
        {
            List<Supplier> suppliers = new SupplierDAO().GetAllSuppliers();

            ViewData["Suppliers"] = suppliers;
            return View();
        }

        public ActionResult CreateSupplier()
        {
            return View();
        }

        public ActionResult AddSupplier(Supplier supplier)
        {
            SupplierDAO suppDAO = new SupplierDAO();

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

        }

        public ActionResult EditSupplier(Supplier supp)
        {
            SupplierDAO suppDAO = new SupplierDAO();

            Supplier supplier = suppDAO.EditSupplier(supp.Id);

            ViewData["supplier"] = supplier;
            return View();
        }

        public ActionResult UpdateSupplier(Supplier supp)
        {
            SupplierDAO suppDAO = new SupplierDAO();

            bool updated = suppDAO.UpdateSupplier(supp);
            if (updated)
            {
                SetFlash(Enums.FlashMessageType.Success, "Successfully updated supplier record for Supplier ID: " + supp.Id + ".");
                return RedirectToAction("Suppliers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Failed to update supplier record for Supplier ID: " + supp.Id + ".");
            return RedirectToAction("Suppliers");
        }

        public ActionResult DeleteSupplier(string id)
        {
            SupplierDAO suppDAO = new SupplierDAO();

            bool deleted = suppDAO.DeleteSupplier(id);
            if (deleted)
            {
                SetFlash(Enums.FlashMessageType.Success, "Supplier record for Ssupplier ID: " + id + " successfully deleted.");
                return RedirectToAction("Suppliers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Supplier record for Supplier ID: " + id + " was not deleted.");
            return RedirectToAction("Suppliers");
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

            bool result = adjustmentVoucherDAO.ReviewAdjustmentVoucher(id, "Approved", adjustmentVoucherDAO.GetVoucherItems(id));

            if (result)
            {
                SetFlash(Enums.FlashMessageType.Success, "Successfully Approved!");
                return RedirectToAction("AdjustmentVouchers");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong! Please try again later!");
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

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong! Please try again later!");
            return RedirectToAction("AdjustmentVouchers");
        }
        public List<int> ExtractSalesReports( )
        {
            int year = DateTime.Now.Year;

            EmployeeDAO employeeDAO = new EmployeeDAO();

            RequestDAO requestDAO = new RequestDAO();

            DepartmentDAO departmentDAO = new DepartmentDAO();

            List<Department> departments = departmentDAO.GetDepartments();

            List<int> monthlySales = new List<int>(); //{paper,pen,stapler}

            foreach (Department d in departments)
            {
                List<RequestReport> requestReports = requestDAO.GetRequestsByDepartment(d.Id);

                if (requestReports != null)
                {
                    int[] sales = new int[3];

                    foreach (RequestReport request in requestReports)
                    {
                        if (request.ReqYear == year)//DateTime.Today.year)
                        {
                            if (request.CategoryID == 1) //paper
                            {
                                sales[0] += request.Qty;
                            }
                            else if (request.CategoryID == 2) //pen
                            {
                                sales[1] += request.Qty;
                            }
                            else //stapler
                            {
                                sales[2] += request.Qty;
                            }
                        }
                    }
                    //monthlySales.Add(DateTime.Today.Month);
                    monthlySales.Add(sales[0]);
                    monthlySales.Add(sales[1]);
                    monthlySales.Add(sales[2]);
                }
            }
            System.Diagnostics.Debug.WriteLine(monthlySales);
            return monthlySales;

        }
        public ActionResult ViewCharts()
        {
            int[] salesReport = ExtractSalesReports().ToArray();
            List<int> request_paper = new List<int>();
            List<int> request_pen = new List<int>();
            List<int> request_stapler = new List<int>();
            for (int i = 0; i < salesReport.Length; i++)
            {
                request_paper.Add(salesReport[i++]);
                request_pen.Add(salesReport[i++]);
                request_stapler.Add(salesReport[i]);
            }
            ViewBag.request_paper = request_paper.ToArray();
            ViewBag.request_pen = request_pen.ToArray();
            ViewBag.request_stapler = request_stapler.ToArray();

            using (var reader = new StreamReader(@"D:\AD project\Final Testing\ReorderChart.csv"))
            {
                List<int> paper = new List<int>();
                List<int> pen = new List<int>();
                List<int> stapler = new List<int>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values[3] == "Paper")
                    {
                        paper.Add(Convert.ToInt32(values[4]));
                    }
                    else if (values[3] == "Pen")
                    {
                        pen.Add(Convert.ToInt32(values[4]));
                    }
                    else
                    {
                        stapler.Add(Convert.ToInt32(values[4]));
                    }
                }
                //int[] paper_array = paper.ToArray();
                //System.Diagnostics.Debug.WriteLine(paper_array);
                //ViewData["paper"] = paper.ToArray();
                //ViewData["pen"] = pen.ToArray();
                //ViewData["stapler"] = stapler.ToArray();

                ViewBag.reorder_paper = paper.ToArray();
                ViewBag.reorder_pen = pen.ToArray();
                ViewBag.reorder_stapler = stapler.ToArray();
            }
            return View();
        }
    }

 }