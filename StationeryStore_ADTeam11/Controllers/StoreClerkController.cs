using StationeryStore_ADTeam11.Filters;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using System.Net.Http;

namespace StationeryStore_ADTeam11.Controllers
{
    [AuthenticationFilter]
    [RoleFilter("Clerk")]
    [LayoutFilter("_storeClerkLayout")]
    public class StoreClerkController : BaseController
    {
        public ActionResult Index()
        {
            RequestDAO r = new RequestDAO();
            ItemDAO i = new ItemDAO();
            OutstandingDAO o = new OutstandingDAO();

            List<Item> ItemIdsAndThresholdValue = i.GetItemIdsAndThresholdValue();

            int low = 0;
            foreach (var row in ItemIdsAndThresholdValue)
            {
                if (row.ThresholdValue > i.GetBalanceByItemId(row.Id))
                {
                    low++;
                }
            }

            ViewData["todayCount"] = r.getTodayReqCount();
            ViewData["lowstock"] = low;
            ViewData["outCount"] = o.GetPendingOutstandingCount();
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

        [HttpPost]
        public JsonResult AddAdjustmentVoucher(List<ItemAdjVoucher> itemData)
        {
            if (itemData == null)
            {
                return Json("Your item list is empty!", JsonRequestBehavior.AllowGet);
            }

            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            if (adjustmentVoucherDAO.Add(Convert.ToInt32(Session["userid"].ToString()), itemData))
            {
                return Json("Successfully Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Something went wrong! Please try again later.", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RequestReorderList(List<PurchaseOrderItem> itemData)
        {
            if (itemData == null) return Json("No item to reorder!", JsonRequestBehavior.AllowGet);

            ItemDAO itemDAO = new ItemDAO();

            if (itemDAO.RequestReorderItems(Convert.ToInt32(Session["userid"].ToString()), itemData))
            {
                return Json("Successfully Requested", JsonRequestBehavior.AllowGet);
            }
            return Json("Something went wrong! Please try again later!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovedRequests()
        {
            RequestDAO request = new RequestDAO();
            ViewData["Requests"] = request.GetApprovedRequests();

            return View();
        }

        public ActionResult ApprovedRequestDetails(int id)
        {
            RequestDAO request = new RequestDAO();
            ViewData["RequestDetails"] = request.GetRequestDetail(id);

            return View();
        }

        public ActionResult ViewStockCard()
        {
            List<StockCard> stockCards = new StockCardDAO().GetAllStockCards();

            ViewData["stockCards"] = stockCards;
            return View();
        }

        public JsonResult GetItemStockCard(string Id)
        {
            List<Object> itemStockCard = new List<Object>();
            Item item = new Item();
            List<StockCard> stockCards = new List<StockCard>();
            ItemDAO itemDAO = new ItemDAO();

            try
            {
                item = itemDAO.GetItemById(Id);
                stockCards = new StockCardDAO().GetStockCardsByItemId(Id);
                itemStockCard.Add(item);
                if (stockCards != null)
                {
                    itemStockCard.Add(stockCards);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not an Id", Id);
            }
            return Json(itemStockCard, JsonRequestBehavior.AllowGet);
        }

        /*[HttpPost]
        public async Task<ActionResult> ViewLowStockItems(String itemCategory)  //PredictReorderQuantity
        {
            using (var client = new HttpClient())
            {
                string year = Convert.ToString(DateTime.Today.Year);

                string month = Convert.ToString(DateTime.Today.Month);

                string item = itemCategory;

                HttpResponseMessage res = await client.GetAsync("http://127.0.0.1:5000/model1?x=" + year + "&y=" + month + "&z=" + item);

                if (res.IsSuccessStatusCode)
                {
                    ViewData["Message"] = res.Content.ReadAsStringAsync().Result;

                    return View();
                }
                else
                {
                    return View("Error");
                }

            }
        }*/
        public async Task<string> PredictReorderQuantity(int itemCategory)
        {
            using (var client = new HttpClient())
            {
                string year = Convert.ToString(DateTime.Today.Year);

                string month = Convert.ToString(DateTime.Today.Month);

                string item = itemCategory.ToString();

                HttpResponseMessage res = await client.GetAsync("http://127.0.0.1:5000/model1?x=" + year + "&y=" + month + "&z=" + item);

                if (res.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Response : " + res.Content.ToString());
                    System.Diagnostics.Debug.WriteLine("Response : " + res.Content.ReadAsStringAsync().Result);
                    return res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return "Error";
                }

            }
        }
        public async Task<ActionResult> ViewLowStockItems()  //PredictReorderQuantity
        {
            ItemDAO itemDAO = new ItemDAO();
            List<LowStockItemViewModel> list = new List<LowStockItemViewModel>();
            List<Item> ItemIdsAndThresholdValue = itemDAO.GetItemIdsAndThresholdValue();
            string ids = "";
            foreach (var i in ItemIdsAndThresholdValue)
            {
                if (i.ThresholdValue > itemDAO.GetBalanceByItemId(i.Id))
                {
                    ids += "'" + i.Id.ToString() + "', ";
                }
            }
            ids = ids.TrimEnd(',', ' ');
            List<Item> items = itemDAO.GetLowStockItems(ids);

            foreach (var row in items)
            {
                LowStockItemViewModel itemVM = new LowStockItemViewModel();
                itemVM.Balance = itemDAO.GetBalanceByItemId(row.Id);
                itemVM.ItemList = row;
                ////////////////////////////////
                itemVM.SuggestedReorderQty= await PredictReorderQuantity(row.CategoryId); //row.CategoryId
                ////////////////////////////////
                list.Add(itemVM);
            }

            ViewData["LowStockList"] = list;
            return View(list);
        }

        public ActionResult ItemSuppliers(String Id)
        {
            List<Supplier> itemSuppliers = new List<Supplier>();

            if (Id != null)
            {
                itemSuppliers = new SupplierDAO().FindSuppliersByItemId(Id);
                ViewData["itemSuppliers"] = itemSuppliers;
                ViewData["itemId"] = Id;
            }

            return View();
        }

        public JsonResult GetSuppliersByItemId(string Id)
        {
            List<Supplier> itemSuppliers = new SupplierDAO().FindSuppliersByItemId(Id);
            return Json(itemSuppliers, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ItemSupplierList(string Id)
        {
            List<Supplier> itemSuppliers = new SupplierDAO().FindSuppliersByItemId(Id);
            List<Supplier> allSuppliers = new SupplierDAO().GetAllSuppliers();
            Item item = new ItemDAO().GetItemById(Id);
            ViewBag.item = item;
            ViewBag.itemSuppliers = itemSuppliers;
            ViewBag.allSuppliers = allSuppliers;

            if (Id == null)
            {
                return PartialView("_noSupplierResults", Id);
            }
            else if (itemSuppliers != null)
            {
                return PartialView("_itemSupplier", ViewBag);
            }
            else
            {
                return PartialView("_noSupplierResults", Id);
            }
        }

        public ActionResult ReplaceSupplier(string itemId, string supplierOrder, string supplierId, double price)
        {
            Item item = new ItemDAO().GetItemById(itemId);
            int order = 0;
            string suppOrder = null;

            if (supplierOrder == "FirstSupplier")
            {
                item.FirstSupplier = supplierId;
                item.FirstPrice = price;
                order = 1;
                suppOrder = "First Supplier";
            }
            else if (supplierOrder == "SecondSupplier")
            {
                item.SecondSupplier = supplierId;
                item.SecondPrice = price;
                order = 2;
                suppOrder = "Second Supplier";
            }
            else if (supplierOrder == "ThirdSupplier")
            {
                item.ThirdSupplier = supplierId;
                item.ThirdPrice = price;
                order = 3;
                suppOrder = "Third Supplier";
            }

            bool success = new ItemDAO().UpdateItemSupplier(item, order);

            if (success)
            {
                SetFlash(Enums.FlashMessageType.Success, "" + suppOrder + " of Item "
                    + itemId + " has been changed to supplier code " + supplierId + " and price of $" + String.Format("{0:0.00}", price) + "/unit successfully!");
                return RedirectToAction("ItemSuppliers", "StoreClerk", itemId);
            }
            else
            {
                SetFlash(Enums.FlashMessageType.Error, "Error in updating uspplier information for Item Id: " + itemId);
                return RedirectToAction("ItemSuppliers", "StoreClerk", itemId);
            }

        }

        public JsonResult ReplaceSupplierList2(string Id)
        {
            List<Supplier> itemSuppliers = new SupplierDAO().FindSuppliersExceptId(Id);
            if (Id == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else if (itemSuppliers != null)
            {
                return Json(itemSuppliers, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CreateRetrievalList()
        {

            RetrievalDAO retrieval = new RetrievalDAO();

            RetrievalList retrievalList = new RetrievalList();
            retrievalList.retrievals = retrieval.GetRetrievalList();
            //foreach (var item in retrievalList.retrievals)
            //{
            //    if (retrieval.checkOutstandingList(item.ItemId))
            //    {
            //        int outstandingQty = retrieval.GetOutstandingQtyByItemId(item.ItemId);
            //        item.Qty += outstandingQty;
            //    }
            //}
            return View(retrievalList);
        }

        [HttpPost]
        public ActionResult CreateRetrieval()
        {
            string username = Session["username"].ToString();
            List<Retrieval> r_list = new List<Retrieval>();
            foreach (string key in Request.Form.AllKeys)
            {
                Retrieval r = new Retrieval();
                r.ItemId = Convert.ToString(key);
                r.RetrievalQty = Convert.ToInt32(Request[key]);
                r_list.Add(r);
            }

            EmployeeDAO e = new EmployeeDAO();
            Employee e_info = e.GetEmployeeByUsername(username);

            RetrievalDAO retrieval = new RetrievalDAO();

            retrieval.CreateRetrieval(e_info.Id, r_list);

            return RedirectToAction("ViewRetrievalList");
        }

        [HttpGet]
        public ActionResult ViewRetrievalList()
        {
            List<RetrievalList> list = new List<RetrievalList>();
            RetrievalList retrievalList = new RetrievalList();
            RetrievalDAO retrieval = new RetrievalDAO();
            retrievalList.retrievals = retrieval.GetItemsAndQty();
            foreach (var item in retrievalList.retrievals)
            {
                if (retrieval.checkOutstandingList(item.ItemId))
                {
                    int outstandingQty = retrieval.GetOutstandingQtyByItemId(item.ItemId);
                    item.Qty += outstandingQty;
                }
            }

            foreach (var item in retrievalList.retrievals)
            {
                string itemId = item.ItemId;
                RetrievalList r_list = new RetrievalList();

                r_list.ItemId = itemId;
                r_list.ItemDesc = retrieval.GetItemDescByItemId(itemId);
                r_list.Total = item.Qty;
                r_list.RetrievedQty = 0;
                r_list.ItemReqList = retrieval.GetItemRequestAndDepts(itemId);
                foreach (var row in r_list.ItemReqList)
                {
                    r_list.RetrievedQty += row.ActualQty;
                }
                list.Add(r_list);

            }
            return View(list);

        }

        [HttpGet]
        public ActionResult ViewDisbursementList()
        {
            string username = Session["username"].ToString();
            EmployeeDAO e = new EmployeeDAO();
            int clerk_id = e.GetEmployeeByUsername(username).Id;

            DisbursementDAO d = new DisbursementDAO();
            List<DisbursementVM> list = d.WebGetDisbursementsByClerk(clerk_id);

            CollectionPointDAO c = new CollectionPointDAO();
            List<CollectionPoint> c_list = c.GetCollectionPointsByClerk(clerk_id);
            foreach (DisbursementVM row in list)
            {
                foreach (CollectionPoint col in c_list)
                {
                    if (col.Id == row.CollectionPointID)
                    {
                        row.CollectionPointName = col.Name;
                    }
                }
            }

            ViewData["c_list"] = c_list;
            return View(list);
        }

        public ActionResult ApprovedReorderList()
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            ViewData["Orders"] = purchaseOrderDAO.ApprovedReorderStockList();

            return View();
        }

        public ActionResult ApprovedReorderStockDetail(int id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            ViewData["StockDetails"] = purchaseOrderDAO.ReorderStockDetail(id);

            return View();
        }

        public ActionResult MakeOrder(int id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            if (purchaseOrderDAO.OrderStockList(id))
            {
                SetFlash(Enums.FlashMessageType.Success, "Ordered!");
                return RedirectToAction("ApprovedReorderList");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong! Please try again later");
            return RedirectToAction("ApprovedReorderList");
        }

        public ActionResult PurchaseOrderHistory()
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            ViewData["PurchaseOrders"] = purchaseOrderDAO.PurchaseOrderHistory();

            return View();
        }

        public ActionResult PurchaseOrderDetail(int id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            ViewData["Details"] = purchaseOrderDAO.ReorderStockDetail(id);

            return View();
        }

        public ActionResult ManageItems(int pid, string id)
        {
            PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();

            if (purchaseOrderDAO.MarkItems(pid, id))
            {
                SetFlash(Enums.FlashMessageType.Success, "Marked as Delivered!");
                return RedirectToAction("PurchaseOrderDetail", new { id = pid });
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");
            return RedirectToAction("PurchaseOrderDetail", new { id = pid });
        }
        public ActionResult EditDisbursementList(string dept_id, string dept_name)
        {
            DisbursementDAO disbursementDAO = new DisbursementDAO();
            CollectionPointDAO collectionPointDAO = new CollectionPointDAO();
            CollectionPoint collectionPoint = collectionPointDAO.GetCollectionPointByDeptID(dept_id);


            List<ItemRequest> itemList = disbursementDAO.GetDisburseItemsForRep(dept_id);

            DisbursementVM disbursement = new DisbursementVM();
            disbursement.DeptName = dept_name;
            disbursement.CollectionPointName = collectionPoint.Name;
            disbursement.ItemList = itemList;

            return View(disbursement);
        }

        //DELETE FROM HERE IF SOMETHING WENT WRONG

        public ActionResult ViewCategories()
        {
            CategoryDAO ctgDAO = new CategoryDAO();
            ViewData["Categories"] = ctgDAO.GetAll();
            return View();
        }


        public ActionResult AddCategory(string name)
        {

            CategoryDAO a = new CategoryDAO();
            a.AddCategory(name);
            return RedirectToAction("ViewCategories", "StoreClerk");
        }

        public ActionResult ViewItems()
        {
            ItemDAO itemDAO = new ItemDAO();
            ViewData["Items"] = itemDAO.GetAllItemsNM();
            return View();
        }

        public ActionResult DetailsItem(string itemid)
        {
            ItemDAO itemDAO = new ItemDAO();
            ViewData["itemid"] = itemid;
            ViewData["itemDetails"] = itemDAO.GetItemByIdNM(itemid);
            return View();
        }

        public ActionResult CreateItem()       {


            SupplierItemDAO suppliersDAO = new SupplierItemDAO();
            List<Supplier> supplierList = suppliersDAO.GetSupplierNames();
            CategoryDAO categoryList = new CategoryDAO();
            List<Category> catlist = categoryList.GetAll();

            ViewData["catlist"] = catlist;
            ViewData["suppliers"] = supplierList;

            return View();
        }

        public ActionResult AddItem(Item item)
        {
            ItemDAO it = new ItemDAO();
            CategoryDAO cat = new CategoryDAO();
            List<Category> catList = cat.GetAll();
            Category eachCategory = new Category();
            int count = catList.Count();
            for (int i = 0; i < count; i++)
            {
                if (item.CategoryName == catList[i].Name)
                {

                    item.CategoryId =catList[i].Id;
                }
            }
            if (it.AddItem(item))
            {
                SetFlash(Enums.FlashMessageType.Success, "Successfully inserted!");
                return RedirectToAction("ViewItems", "StoreClerk");
            }

            SetFlash(Enums.FlashMessageType.Error, "Something went wrong!");
            return RedirectToAction("ViewItems", "StoreClerk");
        }

        public ActionResult UpdateItem(string itemid)
        {
            ViewData["itemid"] = itemid;
            SupplierItemDAO suppliersDAO = new SupplierItemDAO();
            List<Supplier> supplierList = suppliersDAO.GetSupplierNames();

            CategoryDAO categoryList = new CategoryDAO();
            List<Category> catlist = categoryList.GetAll();

            ItemDAO itemDAO = new ItemDAO();
            Item itemList = itemDAO.GetItemByIdNM(itemid);

            ViewData["catlist"] = catlist;
            ViewData["itemList"] = itemList;
            ViewData["suppliers"] = supplierList;
            return View();
        }

        [HttpPost]
        public ActionResult EditItem(string itemid, Item item)
        {
            ItemDAO itemDAO = new ItemDAO();
            CategoryDAO categoryDAO = new CategoryDAO();
            Category category = new Category();
            List<Category> categoreyList = categoryDAO.GetAll();
            int length = categoreyList.Count;
            for (int i = 0; i < length; i++)
            {
                if (item.CategoryName == categoreyList[i].Name)
                {
                    item.CategoryId = categoreyList[i].Id;
                }
            }
            itemDAO.EditItem(item, itemid);
            ViewData["itemId"] = itemid;
            return RedirectToAction("ViewItems", "StoreClerk");
        }

        //NANT MOE'S CODE END HERE
    }
}
