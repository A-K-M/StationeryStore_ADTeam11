using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StationeryStore_ADTeam11.Controllers
{
    [RoutePrefix("api/clerk")]
    public class StoreClerkApiController : ApiController
    {
        [Route("stockcard/item/{itemCode}")]
        [HttpGet]
        public MResponse getStockCard(string itemCode)
        {
            Item item = new ItemDAO().GetItemById(itemCode);
            if (item == null) return new MResponse(false);
            return new MResponseListAndObj<StockCard, Item>()
            {
                Success = true,
                ResList = new StockCardDAO().GetStockCardsbyId(itemCode),
                ResObj = item
            };
        }

        //[Route("adjustmentvoucher/{clerkId}")]
        //[HttpGet]

    }
    
}
