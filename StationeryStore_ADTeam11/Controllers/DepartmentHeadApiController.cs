using Newtonsoft.Json.Linq;
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
    [RoutePrefix("api/department")]
    public class DepartmentHeadApiController : ApiController
    {
        [Route("employees/{deptId}")]
        [HttpGet]
        public ResponseWithList<MEmployee> GetEmployees(string deptId) {
            ResponseWithList<MEmployee> response = new ResponseWithList<MEmployee>() {
                ResList = new EmployeeDAO().GetEmployeeByDepartment(deptId),
                Success = true
            };
            return response;
        }
        [Route("delegate")]
        [HttpPost]
        public MResponse PostDelegate(JObject obj) {
            dynamic jdata = obj;
            string deptId = jdata.DepartmentId;
            Delegation delegation = jdata.Delegate.ToObject<Delegation>();

            bool success = new DelegationDAO().InsertDelegation(delegation,deptId);
            MResponse response = new MResponse(success);
            
            return response;
        }

    }
}
