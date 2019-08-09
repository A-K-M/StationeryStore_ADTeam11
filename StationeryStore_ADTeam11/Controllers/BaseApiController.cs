using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.Controllers
{
    [RoutePrefix("api")]
    public class BaseApiController : ApiController
    {
        [Route("employees/{deptId}")]
        [HttpGet]
        public ResponseListAndObj<MEmployee> GetEmployees(string deptId)
        {
            EmployeeDAO empDao = new EmployeeDAO();
            ResponseListAndObj<MEmployee> response = new ResponseListAndObj<MEmployee>()
            {
                ResList = empDao.GetEmployeeByDepartment(deptId),
                Success = true
            };
            return response;
        }

        [Route("collectionpoints")]
        [HttpGet]
        public ResponseListAndObj<CollectionPoint> GetCollectionPoints()
        {
            CollectionPointDAO dao = new CollectionPointDAO();
            ResponseListAndObj<CollectionPoint> response = new ResponseListAndObj<CollectionPoint>()
            {
                ResList = dao.GetCollectionPoints(),
                Success = true
            };
            return response;
        }

    }
}
