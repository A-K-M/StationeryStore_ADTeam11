using StationeryStore_ADTeam11.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationeryStore_ADTeam11.DAOs;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;
using StationeryStore_ADTeam11.Filters;

namespace StationeryStore_ADTeam11.Controllers
{
    [LayoutFilter("_deptartmentEmployeeLayout")]
    public class DepartmentRepresentativeController : DepartmentEmployeeController // Dept Rep is inherited from Dept Employee
    {
        EmployeeDAO employeeDAO = new EmployeeDAO();

        CollectionPointDAO collectionPointDAO = new CollectionPointDAO();

        DepartmentDAO departmentDAO = new DepartmentDAO();

        RequestDAO requestDAO = new RequestDAO();

        ItemRequestDAO itemRequestDAO = new ItemRequestDAO();

        ItemDAO itemDAO = new ItemDAO();
    
        public string GetDeptId(string username) //Getting DepartmentID by Username
        {           
            string deptId = employeeDAO.GetDepartmentIdByDepartmentHeadUsername(username);

            return deptId;
        }
        
        public ActionResult CollectionPoint()
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>();            
            collectionPoints = collectionPointDAO.GetCollectionPoints(); //Getting all 6 collection point locations
            ViewData["collectionPoints"] = collectionPoints;

            
            string deptId = GetDeptId("helen"); 

            Department department = departmentDAO.GetDepartmentByDeptId(deptId);

            //Getting current repname of department
            string repName = employeeDAO.GetEmployeeById(department.RepId).Name; 

            //Getting current collection point of department
            string collectionPointName = collectionPointDAO.GetCollectionPointById(department.CollectionPoinId).Name; 
            
            ViewData["representativeName"] = repName;
            ViewData["collectionPoint"] = collectionPointName;

            return View();
        }
        [HttpPost]
        public ActionResult CollectionPoint(Department point)
        {
            string deptId = GetDeptId("helen");
           
            departmentDAO.UpdateDepartmentCollectionPoint(deptId, point.CollectionPoinId);

            return RedirectToAction("CollectionPoint", "DepartmentRepresentative");
        }
      
        public ActionResult Disbursement()
        {
            string deptId = GetDeptId("helen");
            List<string> collectionPoint = departmentDAO.GetCollectionPointByDeptId(deptId);

            List<RequestDetailViewModel> itemRequests = new List<RequestDetailViewModel>();

            List<ItemRequest> itemrequests = itemRequestDAO.GetTotalDisburseItems(deptId);

            foreach (var req in itemrequests)
            {
                RequestDetailViewModel rvm = new RequestDetailViewModel
                {
                    ItemDescription = itemDAO.GetItemDescription(req.ItemId),
                    NeededQty = req.NeededQty,
                    ActualQty = req.ActualQty,
                    ItemId = req.ItemId
                };
                itemRequests.Add(rvm);
            }
            ViewData["collectionPoint"] = collectionPoint;
            ViewData["itemRequests"] = itemRequests;

            return View();
        }
        public ActionResult ApproveDisbursement()
        {
            OutstandingDAO outstandingDAO = new OutstandingDAO();
            string deptId = GetDeptId("helen");
            foreach(var ir in itemRequestDAO.GetDisburseItems(deptId))
            {
                outstandingDAO.AddItemRequest(ir);
                requestDAO.UpdateDisbursedDate(ir);
            }
            return RedirectToAction("Disbursement");
        }
    }
    
}