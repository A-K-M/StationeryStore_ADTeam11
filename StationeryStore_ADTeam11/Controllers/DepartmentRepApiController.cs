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
    [RoutePrefix("api/representative")]
    public class DepartmentRepApiController : ApiController
    {
        [Route("{deptId}/collectionpoints/{pointId}")]
        [HttpPut]
        public MResponse UpdateCollectionPoint(int pointId,string deptId)
        {
            bool success = new CollectionPointDAO().UpdateCollectionPoint(pointId,deptId);
            MResponse response = new MResponse(success);

            return response;
        }

        [Route("{deptId}/disbursements")]
        [HttpGet]
        public MResponse GetDisbursementInfo(string deptId)
        {
            CollectionPoint point = new CollectionPointDAO().GetCollectionPointByDeptID(deptId);
            List<ItemRequest> list = new DisbursementDAO().GetDisburseItemsForRep(deptId);
            //bool showApproveButton = false;
            //if (list.Count() > 0)
            //{
            //    showApproveButton = DateTime.Today.DayOfWeek == DayOfWeek.Monday;
            //}

            return new MResponseListAndObj<ItemRequest, CollectionPoint>() {
                ResObj = point,
                ResList = list ,
                Success = true
            };
        }

        [Route("{deptId}/disbursements/approve")]
        [HttpPost]
        public MResponse ApproveDisbursement(string deptId) {
            DisbursementDAO dao = new DisbursementDAO();
            return new MResponse() { Success = dao.ApproveDisbursement(deptId) };
        }

    }
}
