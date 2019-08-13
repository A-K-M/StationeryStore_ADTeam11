﻿using Newtonsoft.Json.Linq;
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
    [RoutePrefix("api/departments")]
    public class DepartmentHeadApiController : ApiController
    {

        [Route("delegate")]
        [HttpPost]
        public MResponse PostDelegate(Delegation delegation) {
            
            bool success = new DelegationDAO().InsertDelegation(delegation,"COMM");
            MResponse response = new MResponse(success);
            
            return response;
        }

        [Route("delegates/{deptId}")]
        [HttpGet]
        public MResponseList<Delegation> getDelegates(string deptId) {
            DelegationDAO delegationDAO = new DelegationDAO();
            MResponseList<Delegation> respone = new MResponseList<Delegation>()
            {
                ResList = delegationDAO.GetDelegations(),
                Success = true
            };
            return respone;
        }

        [Route("delegates/cancel/{delegationId}")]
        [HttpPut]
        public MResponse CancelDelegate(int delegationId)
        {

            bool success = new DelegationDAO().CancelDelegation("COMM",delegationId);
            MResponse response = new MResponse(success);

            return response;
        }

        [Route("collectionpoints/representative")]
        [HttpGet]
        public MResponse GetCollectionPointAndRep()
        {
            CollectionPointDAO dao = new CollectionPointDAO();
            MCollectionAndRep collectionAndRep = dao.GetCollecitonPointAndRep("COMM");
            MResponseListAndObj<CollectionPoint, MCollectionAndRep> response
                = new MResponseListAndObj<CollectionPoint, MCollectionAndRep>()
                {
                    Success = collectionAndRep != null,
                    ResList = dao.GetCollectionPoints(),
                    ResObj = collectionAndRep
                };
            return response;
        }


        //[Route("collectionpoints/representative")]
        //[HttpGet]
        //public MResponse GetCollectionPointAndRep()
        //{
        //    return new CollectionPointDAO().GetCollecitonPointAndRep("COMM");
        //}

        [Route("collectionpoints/{pointId}/representative/{repId}")]
        [HttpPut]
        public MResponse UpdatePointAndRep(int pointId, int repId) {

           return new CollectionPointDAO().UpdateCollectionPointAndRep(pointId,repId,"COMM");
       
        }

        [Route("collectionpoints/{pointId}")]
        [HttpPut]
        public MResponse UpdateCollectionPoint(int pointId)
        {
            bool success = new CollectionPointDAO().UpdateCollectionPoint(pointId, "COMM");
            MResponse response = new MResponse(success);

            return response;
        }

        [Route("requests")]
        [HttpGet]
        public MResponse GetRequestHistory()
        {
            RequestDAO dao = new RequestDAO();
            return new MResponseList<RequisitionVM>() { ResList = dao.GetReqListByDepartment("COMM") };
        }

        [Route("requests/{reqId}/detail")]
        [HttpGet]
        public MResponse GetRequestDetail(string reqId)
        {
            RequestDAO dao = new RequestDAO();
            return new MResponseList<MRequestItem>() { ResList = dao.GetRequestItems("COMM/111/100") };
        }

        [Route("requests/{reqId}")]
        [HttpPatch]
        public MResponse UpdateReqStatus(string reqId, RequisitionVM status)
        {
            bool success = new RequestDAO().UpdateStatus(status.Status, "COMM/111/100");
            MResponse response = new MResponse(success);

            return response;
        }

    }
}
