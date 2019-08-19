using Newtonsoft.Json.Linq;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.Util;
using StationeryStore_ADTeam11.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StationeryStore_ADTeam11.Controllers
{
    [RoutePrefix("api/head")]
    public class DepartmentHeadApiController : ApiController
    {

        [Route("{headId}/{deptId}/delegates")]
        [HttpPost]
        public MResponse PostDelegate(Delegation delegation,int headId,string deptId) {
            
            bool success = new DelegationDAO().InsertDelegation(delegation,deptId);
            if (success) {
                Email email = new Email();
                Employee e = new EmployeeDAO().GetEmployeeById(headId);
                email.SendEmail(delegation.Email, "Authority Delegation",email.CreateMsgBody(delegation,e.Name));
            }
            MResponse response = new MResponse(success);
            
            return response;
        }

        [Route("{deptId}/delegates")]
        [HttpGet]
        public MResponseList<Delegation> getDelegates(string deptId) {
            DelegationDAO delegationDAO = new DelegationDAO();
            MResponseList<Delegation> respone = new MResponseList<Delegation>()
            {
                ResList = delegationDAO.GetDelegations(deptId),
             };
            return respone;
        }

        [Route("{headId}/{deptId}/delegates/cancel")]
        [HttpPut]
        public MResponse CancelDelegate(int headId,string deptId,Delegation delegation)
        {

            bool success = new DelegationDAO().CancelDelegation(deptId,delegation.Id);
            if (success)
            {
                Email email = new Email();
                Employee e = new EmployeeDAO().GetEmployeeById(headId);
                email.SendEmail(delegation.Email, "Authority Delegation Cancel", email.CancelMsgBody(delegation, e.Name));
            }
            MResponse response = new MResponse(success);

            return response;
        }

        [Route("dept/{deptId}/collectionpoints/representative")]
        [HttpGet]
        public MResponse GetCollectionPointAndRep(string deptId)
        {
            CollectionPointDAO dao = new CollectionPointDAO();
            MCollectionAndRep collectionAndRep = dao.GetCollecitonPointAndRep(deptId);
            MResponseObj<MCollectionAndRep> response
                = new MResponseObj<MCollectionAndRep>()
                {
                    ResObj = collectionAndRep,
                    Success = collectionAndRep != null
                };
            return response;
        }


        [Route("{deptId}/collectionpoints/{pointId}/representative/{repId}")]
        [HttpPut]
        public MResponse UpdatePointAndRep(int pointId, int repId,string deptId) {

           return new CollectionPointDAO().UpdateCollectionPointAndRep(pointId,repId,deptId);
       
        }

       

        [Route("{deptId}/requests")]
        [HttpGet]
        public MResponse GetRequestHistory(string deptId)
        {
            RequestDAO dao = new RequestDAO();
            return new MResponseList<RequisitionVM>() { ResList = dao.GetReqListByDepartment(deptId) };
        }

        [Route("requests/{reqId}/detail")]
        [HttpGet]
        public MResponse GetRequestDetail(int reqId)
        {
            RequestDAO dao = new RequestDAO();
            return new MResponseList<MRequestItem>() { ResList = dao.GetRequestItems(reqId) };
        }

        [Route("{deptId}/requests/approve_all")]
        [HttpGet]
        public MResponse ApproveAllRequst(string deptId)
        {
            RequestDAO dao = new RequestDAO();
            return new MResponse() { Success = dao.ApproveAllRequests(deptId) };
        }


        [Route("requests")]
        [HttpPatch]
        public MResponse UpdateReqStatus(RequisitionVM status)
        {
            bool success = new RequestDAO().UpdateStatus(status.Status, status.Id);
            MResponse response = new MResponse(success);

            return response;
        }

    }
}
