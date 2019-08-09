using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.MobileModels;
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
        [Route("stockcard/{itemCode}")]
        [HttpGet]
        public MResponseObj<MStockCard> getStockCard(string itemCode)
        {
            StockCardDAO stockCardDAO = new StockCardDAO();

            MResponseObj<MStockCard> response = new MResponseObj<MStockCard>() {
                ResObj = stockCardDAO.
            }
            return respone;
        }

    }
}
