using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;
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
            {   Success = true,
                ResList = new StockCardDAO().GetStockCardsByItemId(itemCode),
                ResObj = item
            };
        }

        [Route("{clerkId}/adjustmentvoucher")]
        [HttpGet]
        public MResponse getAdjVoucherList(int clerkId) {
            List<AdjustmentVoucherViewModel> voucherList = new AdjustmentVoucherDAO().GetAdjVoucherByClerk(clerkId);
            return new MResponseList<AdjustmentVoucherViewModel>() { ResList = voucherList };
        }

        [Route("adjustmentvoucher/{voucherId}")]
        [HttpGet]
        public MResponse getAdjVoucherItems(int voucherId)
        {
            List<MAdjustmentItem> itemList = new AdjustmentVoucherDAO().GetAdjVoucherItems(voucherId);
            return new MResponseList<MAdjustmentItem>() { ResList = itemList };
        }

        [Route("{clerkId}/adjustmentvoucher")]
        [HttpPost]
        public MResponse createAdjVoucher(int clerkId,List<MAdjustmentItem> items)
        {
            AdjustmentVoucherDAO dao = new AdjustmentVoucherDAO();
            return new MResponse() { Success = dao.CreateAdjVoucher(clerkId, items) };
        }
    }
    
}
