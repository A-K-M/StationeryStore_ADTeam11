﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.Controllers
{
    [RoutePrefix("api")]
    public class BaseApiController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public MResponse Login(MLogin e) {
            EmployeeDAO dao = new EmployeeDAO();
            Employee res =  dao.login(e.UserName, e.Password);
            
            return new MResponseObj<Employee>() { ResObj = res, Success = (res != null) };
        }

        [Route("employees/{deptId}")]
        [HttpGet]
        public MResponse GetEmployees(string deptId)
        {
            EmployeeDAO empDao = new EmployeeDAO();
            MResponseList<Employee> response = new MResponseList<Employee>()
            {
                ResList = empDao.GetEmployeeByDeptId(deptId),
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

        //[Route("async")]
        //[HttpGet]
        //public MResponse Test()
        //{

        //    Task<List<MEmployee>> task2 =  new EmployeeDAO().GetEmployees();

        //    Task<List<CollectionPoint>> task1 =  new CollectionPointDAO().GetCollectionPointsTAsync();
            
        //    ItemDAO dao = new ItemDAO();
            
        //    return new MResponseList<MItemSpinner>() { ResList = dao.GetAllItems() };
        //}

    }
}
