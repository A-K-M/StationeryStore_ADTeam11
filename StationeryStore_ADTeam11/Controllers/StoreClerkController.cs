using StationeryStore_ADTeam11.Filters;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

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
    }
}