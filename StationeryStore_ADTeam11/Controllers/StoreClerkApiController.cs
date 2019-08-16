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
        public MResponse GetStockCard(string itemCode)
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
        public MResponse GetAdjVoucherList(int clerkId) {
            List<AdjustmentVoucherVM> voucherList = new AdjustmentVoucherDAO().GetAdjVoucherByClerk(clerkId);
            return new MResponseList<AdjustmentVoucherVM>() { ResList = voucherList };
        }

        [Route("adjustmentvoucher/{voucherId}/detail")]
        [HttpGet]
        public MResponse GetAdjVoucherItems(int voucherId)
        {
            List<MAdjustmentItem> itemList = new AdjustmentVoucherDAO().GetAdjVoucherItems(voucherId);
            return new MResponseList<MAdjustmentItem>() { ResList = itemList };
        }

        [Route("{clerkId}/adjustmentvoucher")]
        [HttpPost]
        public MResponse CreateAdjVoucher(int clerkId,List<MAdjustmentItem> items)
        {
            AdjustmentVoucherDAO dao = new AdjustmentVoucherDAO();
            return new MResponse() { Success = dao.CreateAdjVoucher(clerkId, items) };
        }

        [Route("retrievals")]
        [HttpGet]
        public MResponse GetRetrievalList()
        {
            List<Retrieval> retrievals = new RetrievalDAO().GetRetrievalList();
            return new MResponseList<Retrieval>() { ResList = retrievals };
        }

        [Route("{clerkId}/disbursements")]
        [HttpGet]
        public MResponse GetDisbursements(int clerkId)
        {
            List<MDisbursement> retrievals = new DisbursementDAO().GetDisbursementsByClerk(clerkId);
            return new MResponseList<MDisbursement>() { ResList = retrievals };
        }

        [Route("{clerkId}/collectionPoints")]
        [HttpGet]
        public MResponse GetCollectionPoints(int clerkId)
        {
            List<CollectionPoint> points = new CollectionPointDAO().GetCollectionPointsByClerk(clerkId);
            return new MResponseList<CollectionPoint>() { ResList = points };
        }
    }
    
}
