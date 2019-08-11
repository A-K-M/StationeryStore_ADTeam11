using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class RequestDAO : DatabaseConnection
    {
        public List<StationeryRequest> GetRequestList()
        {
            List<StationeryRequest> requestlist = new List<StationeryRequest>();
            
           
            string sql = "SELECT R.Status as Status,E.Name as Name,R.DateTIme as Date,R.Id as RequestId FROM Request R,Employee E WHERE E.ID=R.EmployeeID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                StationeryRequest request = new StationeryRequest()
                {
                    EmpName = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    Status = (string)reader["Status"],
                    RequestId = (string)reader["RequestId"],
                };
                requestlist.Add(request);
            }
            
            return requestlist;
        }

        public List<StationeryRequest> ViewPendingRequestDetails(string requestId)
        {
            List<StationeryRequest> requestlist = new List<StationeryRequest>();

            string sql = "SELECT e.UserName as Name,i.Description,ir.NeededQty  as Qty,r.ID as RequestId " +
                         "FROM Employee e, Request r, Item i,ItemRequest ir " +
                         "WHERE e.id = r.EmployeeID AND r.ID = ir.RequestID AND ir.ItemID = i.ID AND r.ID = @reqId";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@reqId", requestId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                StationeryRequest request = new StationeryRequest()
                {
                    EmpName = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    TotalItem = (int)reader["Qty"],
                    RequestId = (string)reader["RequestId"]
                };
                requestlist.Add(request);
            }
            connection.Close();
            return requestlist;
        }

        public bool UpdateStatus(string status, string reqId)
        {
            string sql = "UPDATE Request SET Status= @status WHERE ID= @reqId";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@reqId", reqId);
            connection.Open();
            int row = cmd.ExecuteNonQuery(); // return no of rows affected by query
            connection.Close();
            return row > 0;
        }

        public List<RequisitionVM> GetRequistionListByEmpId(int empId)
        {
            List<RequisitionVM> requisitionList = new List<RequisitionVM>();
            RequisitionVM requisition = null;

            string sql = @"SELECT r.ID, r.[DateTime], r.[Status], SUM(ir.NeededQty) AS Quantity " +
                        "FROM Request r, ItemRequest ir " +
                        "WHERE r.ID = ir.RequestID " +
                        "AND r.EmployeeID = @value " +
                        "GROUP BY r.ID, r.[DateTime], r.[Status]";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@value", SqlDbType.Int);
            cmd.Parameters["@value"].Value = empId;

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                requisition = new RequisitionVM()
                {
                    Id = data["ID"].ToString(),
                    Date = Convert.ToDateTime(data["DateTime"]),
                    Status = data["Status"].ToString(),
                    Quantity = Convert.ToInt32(data["Quantity"])
                };

                requisitionList.Add(requisition);
            }
            data.Close();
            connection.Close();

            return requisitionList;
        }


        public List<RequisitionVM> GetReqListByDepartment(string deptId)
        {
            List<RequisitionVM> reqList = new List<RequisitionVM>();
            RequisitionVM req = null;
            SqlDataReader reader = null;
            try
            {
                string sql = " SELECT e.UserName,req.DateTime,req.Status,req.ID,SUM(ireq.NEEDEDQTY) Quantity " +
                              " FROM Request req,Employee e,ItemRequest ireq " +
                              " WHERE req.EmployeeID = e.ID AND ireq.RequestID=req.id AND " +
                                "req.EmployeeID IN(SELECT e.ID FROM Employee e WHERE e.DeptID = 'COMM') " +
                              " GROUP BY req.ID,e.UserName,req.DateTime,req.Status";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    req = new RequisitionVM()
                    {
                        Id = reader["ID"].ToString(),
                        EmployeeName = reader["UserName"].ToString(),
                        Date = (DateTime)reader["DateTime"],
                        Status = reader["Status"].ToString(),
                        Quantity = (int)reader["Quantity"]
                    };
                    reqList.Add(req);
                }

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return reqList;
        }
        public List<MRequestItem> GetRequestItems(string reqId) {
            List<MRequestItem> itemList = new List<MRequestItem>();
            MRequestItem req = null;
            SqlDataReader reader = null;
            try
            {
                string sql = "SELECT i.Description,ir.NeededQty  as Qty " +
                             "FROM Request r, Item i,ItemRequest ir " +
                             "WHERE r.ID = ir.RequestID AND ir.ItemID = i.ID AND r.ID = @reqId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@reqId", reqId);
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    req = new MRequestItem()
                    {
                        Description = reader["Description"].ToString(),
                        Quantity = (int)reader["Qty"]
                    };
                    itemList.Add(req);
                }

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return itemList;
        }
    }
}