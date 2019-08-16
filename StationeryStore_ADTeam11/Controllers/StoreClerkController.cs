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
    [LayoutFilter("_storeClerkLayout")]
    public class StoreClerkController : BaseController
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

        [HttpPost]
        public JsonResult AddAdjustmentVoucher(List<ItemAdjVoucher> itemData)
        {
            if (itemData == null)
            {
                return Json("Your item list is empty!", JsonRequestBehavior.AllowGet);
            }

            AdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

            if (adjustmentVoucherDAO.Add(11233, itemData))
            {
                return Json("Successfully Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Something went wrong! Please try again later.", JsonRequestBehavior.AllowGet);
            }
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
            Session["Username"] = "Clerk User";
            Session["Role"] = "Clerk";

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

        [HttpPost]
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
        }
        public ActionResult ViewLowStockItems()  //PredictReorderQuantity
        {
            ItemDAO itemDAO = new ItemDAO();
            List<LowStockItemViewModel> list = new List<LowStockItemViewModel>();
            List<Item> ItemIdsAndThresholdValue = itemDAO.GetItemIdsAndThresholdValue();
            string ids = "";
            foreach (var i in ItemIdsAndThresholdValue)
            {
                if (i.ThresholdValue>itemDAO.GetBalanceByItemId(i.Id))
                {
                    ids += "'"+i.Id.ToString()+"', ";
                }
            }
            ids = ids.TrimEnd(',', ' ');
            List<Item> items = itemDAO.GetLowStockItems(ids);

            foreach (var row in items)
            {
                LowStockItemViewModel itemVM = new LowStockItemViewModel();
                itemVM.Balance = itemDAO.GetBalanceByItemId(row.Id);
                itemVM.ItemList = row;
                list.Add(itemVM);
            }

            ViewData["LowStockList"] = list;
            return View(list);



            //ItemDAO itemDAO = new ItemDAO();
            //List<LowStockItemViewModel> list = new List<LowStockItemViewModel>();
            //List<Item> items = itemDAO.GetLowStockItems();

            //foreach (var row in items)
            //{
            //    LowStockItemViewModel itemVM = new LowStockItemViewModel();
            //    itemVM.Balance = itemDAO.GetBalanceByItemId(row.Id);
            //    itemVM.ItemList = row;
            //    list.Add(itemVM);                
            //}

            //ViewData["LowStockList"] = list;
            //return View(list);
        }

        public ActionResult ItemSuppliers(String Id)
        {
            Session["Username"] = "Clerk User";
            Session["Role"] = "Clerk";
            List<Supplier> itemSuppliers = null;

            if (Id != null)
            {
                itemSuppliers = new SupplierDAO().FindSuppliersByItemId(Id);
            }

            ViewData["suppliers"] = itemSuppliers;
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
            if (Id == null)
            {
                return PartialView("_noSupplierResults", Id);
            }
            else if (itemSuppliers != null)
            {
                return PartialView("_itemSupplier", itemSuppliers);
            }
            else
            {
                return PartialView("_noSupplierResults", Id);
            }
        }

        public PartialViewResult ReplaceSupplierList(string Id)
        {
            List<Supplier> itemSuppliers = new SupplierDAO().FindSuppliersExceptId(Id);
            if (Id == null)
            {
                return PartialView("_noSupplierResults", Id);
            }
            else if (itemSuppliers != null)
            {
                return PartialView("_replaceSupplierList", itemSuppliers);
            }
            else
            {
                return PartialView("_noSupplierResults", Id);
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
            string ItemId;
            int retrieved;
            foreach (string key in Request.Form.AllKeys)
            {
                string out_ids = "";
                string req_ids = "";
                string ir_query = "";
                ItemId = Convert.ToString(key);
                retrieved = Convert.ToInt32(Request[key]);
                RetrievalDAO retrieval = new RetrievalDAO();
                retrieval.CreateRetrieval(username, ItemId, retrieved);

                List<Outstanding> out_list = retrieval.GetPendingOutstandingsListByItemId(ItemId);
                if (out_list != null)
                {
                    foreach (Outstanding row in out_list)
                    {
                        int real = 0;
                        if (retrieved > 0)
                        {
                            real = retrieved;
                        }
                        retrieved -= row.RemainingQty;
                        if (retrieved >= 0)
                        {
                            out_ids += row.Id.ToString() + ", ";
                            real = retrieved;
                        }
                    }
                    out_ids = out_ids.TrimEnd(',', ' ');
                    if (out_ids != "")
                        retrieval.UpdateOutstanding(out_ids);
                }


                List<ItemRequest> request_list = retrieval.GetReqDeptListByItemId(ItemId);
                if (request_list != null && retrieved > 0)
                {
                    foreach (ItemRequest request in request_list)
                    {
                        int real = 0;
                        if (retrieved > 0)
                        {
                            real = retrieved;
                        }
                        retrieved -= request.NeededQty;
                        if (retrieved >= 0)
                        {
                            req_ids += request.RequestId.ToString() + ", ";
                            real = retrieved;
                            retrieval.UpdateItemRequest(request.Id, request.NeededQty);
                        }
                        else
                        {
                            ir_query += retrieval.CreateOutstandingQuery(request.Id, request.NeededQty - real);
                            retrieval.UpdateItemRequest(request.Id, real);
                        }
                    }
                    req_ids = req_ids.TrimEnd(',', ' ');
                    if (req_ids != "")
                        retrieval.UpdateRequestStatus(req_ids);
                    if (ir_query != "")
                        retrieval.CreateOutstanding(ir_query);
                }

            }
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
    }
}
