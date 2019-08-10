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
        public MResponse GetEmployees(string deptId)
        {
            EmployeeDAO empDao = new EmployeeDAO();
            MResponseList<MEmployee> response = new MResponseList<MEmployee>()
            {
                ResList = empDao.GetEmployeeByDepartment(deptId),
                Success = true
            };
            return response;
        }

        [Route("collectionpoints")]
        [HttpGet]
        public MResponse GetCollectionPoints()
        {
            CollectionPointDAO dao = new CollectionPointDAO();
            MResponseList<CollectionPoint> response = new MResponseList<CollectionPoint>()
            {
                ResList = dao.GetCollectionPoints(),
                Success = true
            };
            return response;
        }

        [Route("categories")]
        [HttpGet]
        public MResponse GetCategories() {
            CategoryDAO dao = new CategoryDAO();
            return new MResponseList<Category>() { ResList = dao.GetAll() };
        }

        [Route("items")]
        [HttpGet]
        public MResponse GetItems()
        {
            ItemDAO dao = new ItemDAO();
            return new MResponseList<MItemSpinner>() { ResList = dao.GetAllItems() };
        }

    }
}
