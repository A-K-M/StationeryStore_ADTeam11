﻿using StationeryStore_ADTeam11.Filters;
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
    public class StoreClerkController : Controller
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

            int insertedId = adjustmentVoucherDAO.Add(11233);

            int result = adjustmentVoucherDAO.AddVoucherItems(itemData, insertedId);

            if (result > 0)
            {
                return Json("Successfully Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                adjustmentVoucherDAO.DeleteAdjustmentVoucher(insertedId);

                return Json("Something went wrong! Please try again later.", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewStockCard()
        {
            Session["Username"] = "Clerk User";
            Session["Role"] = "Clerk";

            List<StockCard> stockCards = StockCardDAO.GetAllStockCards();


            ViewData["stockCards"] = stockCards;
            return View();
        }

        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            List<StockCard> stockCards = new List<StockCard>();
            Item item = new Item();

            if (SearchBy == "ID")
            { 
                try
                {
                    string Id = Convert.ToString(SearchValue);
                    stockCards = StockCardDAO.GetStockCardsbyId(Id);
                    item = ItemDAO
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not an Id", SearchValue);
                }
                return Json(stockCards, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //stockCards = StockCardDAO.GetStockCardsbyId(Id)
                return Json(stockCards, JsonRequestBehavior.AllowGet);
            }

        }
    }
}