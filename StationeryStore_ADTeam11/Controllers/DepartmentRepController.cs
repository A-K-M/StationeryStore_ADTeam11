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
    public class DepartmentRepController : ApiController
    {
        [Route("collectionpoints/{pointId}")]
        [HttpPut]
        public MResponse UpdateCollectionPoint(int pointId)
        {
            bool success = new CollectionPointDAO().UpdateCollectionPoint(pointId, "COMM");
            MResponse response = new MResponse(success);

            return response;
        }

        [Route("disbursements/{deptId}")]
        [HttpGet]
        public MResponse GetDisbursementInfo(string deptId)
        {
            CollectionPoint point = new CollectionPointDAO().GetCollectionPointByDeptID(deptId);
            List<ItemRequest> list = new DisbursementDAO().GetDisburseItemsForRep(deptId);
            bool showApproveButton = false;
            if (list.Count() > 0)
            {
                showApproveButton = DateTime.Today.DayOfWeek == DayOfWeek.Monday;
            }

            return new MResponseListAndObj<ItemRequest, CollectionPoint>() {
                ResObj = point,
                ResList = list ,
                Success = showApproveButton
            };
        }

    }
}
