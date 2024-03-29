﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.Util;
using StationeryStore_ADTeam11.View_Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class RequestDAO : DatabaseConnection
    {
        public List<StationeryRequest> GetRequestList(string deptID)
        {
            List<StationeryRequest> requestlist = new List<StationeryRequest>();
            SqlDataReader reader = null;
            try
            {

                string sql = "SELECT R.Status as Status,E.Name as Name,R.DateTime as Date,R.Id as RequestId , SUM(IR.NeededQty) AS [TotalQuantity] " +
                        "FROM Request R,Employee E, ItemRequest IR " +
                        "WHERE E.ID = R.EmployeeID " +
                        "AND E.DeptID = '" + deptID + "' " +
                        "AND R.ID = IR.RequestID " +
                        "GROUP BY R.Status ,E.Name,R.DateTime,R.Id; ";

                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    StationeryRequest request = new StationeryRequest()
                    {
                        EmpName = (string)reader["Name"],
                        Date = (DateTime)reader["Date"],
                        Status = (string)reader["Status"],
                        RequestId = (int)reader["RequestId"],
                        TotalItem = (int)reader["TotalQuantity"]
                    };
                    requestlist.Add(request);
                }
            }
            finally {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return requestlist;
        }

        public bool ApproveAllRequest(string deptId) {
            try
            {
                connection.Open();
                string sql = @" UPDATE Request SET Status = 'Approved'
                                WHERE ID IN( SELECT r.ID FROM Request r, Employee e
                                             WHERE r.EmployeeID = e.ID AND e.DeptID = @deptId) AND Status = 'Pending'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;


        }
        public List<Request> GetRequests(int empId) //NZCK
        {
            //empId = 11237;

            List<Request> requests = new List<Request>();
            SqlConnection conn = connection;
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                string sql = @"select * from Request where EmployeeID='" + empId + "'and Status='Approved'";
                SqlCommand command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["DisbursedDate"].GetType().Equals(typeof(DBNull)))
                    {
                        Request request = new Request()
                        {
                            Id = (int)reader["ID"],
                            EmployeeId = (int)reader["EmployeeID"],
                            DateTime = (DateTime)reader["DateTime"],
                            Status = (string)reader["Status"]
                        };
                        //Console.WriteLine("Hello there");
                        requests.Add(request);
                    }

                }
            }
            finally {
                if (reader != null) reader.Close();
                conn.Close();
            }
            return requests;
        }

        public void UpdateDisbursedDate(ItemRequest request) //NZCK
        {
            int outstandingQty = request.NeededQty - request.ActualQty;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"update Request set DisbursedDate='" + DateUtils.Now() + "', Status='Completed' where ID= @reqId";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@reqId", request.RequestId);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public List<StationeryRequest> ViewPendingRequestDetails(int requestId)
        {
            List<StationeryRequest> requestlist = new List<StationeryRequest>();
            SqlDataReader reader = null;

            try
            { 
            string sql = "SELECT e.UserName as Name,i.Description,ir.NeededQty  as Qty,r.ID as RequestId " +
                         "FROM Employee e, Request r, Item i,ItemRequest ir " +
                         "WHERE e.id = r.EmployeeID AND r.ID = ir.RequestID AND ir.ItemID = i.ID AND r.ID = @reqId";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@reqId", requestId);
            connection.Open();
             reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                StationeryRequest request = new StationeryRequest()
                {
                    EmpName = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    TotalItem = (int)reader["Qty"],
                    RequestId = (int)reader["RequestId"]
                };
                requestlist.Add(request);
            }
        }
         finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return requestlist;
        }

        public bool UpdateStatus(string status, int reqId)
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

        public List<RequisitionVM> GetApprovedRequests()
        {
            List<RequisitionVM> requisitions = new List<RequisitionVM>();
            RequisitionVM requisition = null;

            string sql = "SELECT r.ID, r.DateTime, r.Status, e.Name, SUM(ir.NeededQty) AS [TotalItems] " +
                          "FROM Request r, Employee e, ItemRequest ir " +
                          "WHERE Status = 'Approved' " +
                          "AND r.EmployeeID = e.ID " +
                          "AND r.ID = ir.RequestID " +
                          "GROUP BY  r.ID, r.DateTime, r.Status, e.Name " +
                          "ORDER BY r.DateTime ASC; ";

            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                requisition = new RequisitionVM()
                {
                    Id = Convert.ToInt32(data["ID"]),
                    Date = Convert.ToDateTime(data["DateTime"]),
                    Status = data["Status"].ToString(),
                    EmployeeName = data["Name"].ToString(),
                    Quantity = Convert.ToInt32(data["TotalItems"])
                };

                requisitions.Add(requisition);
            }
            data.Close();
            connection.Close();

            return requisitions;
        }

        public List<RequisitionVM> GetRequistionListByEmpId(int empId)
        {
            List<RequisitionVM> requisitionList = new List<RequisitionVM>();
            RequisitionVM requisition = null;
            SqlDataReader data = null;
            try
            {
                string sql = "SELECT r.ID, r.[DateTime], r.[Status], SUM(ir.NeededQty) AS Quantity, e.DeptID " +
                              "FROM Request r, ItemRequest ir, Employee e " +
                               "WHERE r.ID = ir.RequestID " +
                                "AND r.EmployeeID = @value " +
                                "AND r.EmployeeID = e.ID " +
                                "GROUP BY r.ID, r.[DateTime], r.[Status], e.DeptID";

                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.Add("@value", SqlDbType.Int);
                cmd.Parameters["@value"].Value = empId;

                connection.Open();

                 data = cmd.ExecuteReader();

                while (data.Read())
                {
                    requisition = new RequisitionVM()
                    {
                        Id = Convert.ToInt32(data["ID"]),
                        Date = Convert.ToDateTime(data["DateTime"]),
                        Status = data["Status"].ToString(),
                        Quantity = Convert.ToInt32(data["Quantity"]),
                        DepartmentId = data["DeptID"].ToString()
                    };

                    requisitionList.Add(requisition);
                }
            }
            finally
            {
                if (data != null) data.Close();
                connection.Close();
            }

            return requisitionList;
        }

        public List<RequestDetailViewModel> GetRequestDetail(int reqId)
        {
            List<RequestDetailViewModel> requestDetailList = new List<RequestDetailViewModel>();
            SqlDataReader data = null;

            try
            {
                string sql = "SELECT i.Description, ir.NeededQty, r.DateTime, cp.Name " +
                            "FROM ItemRequest ir, Item i, Request r, Employee e, Department d, CollectionPoint cp " +
                            "WHERE r.ID = @reqId " +
                            "AND ir.RequestID = r.ID " +
                            "AND i.ID = ir.ItemID " +
                            "AND e.ID = r.EmployeeID " +
                            "AND d.ID = e.DeptID " +
                            "AND cp.ID = d.CollectionPointID";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@reqId", reqId);
                connection.Open();

                 data = cmd.ExecuteReader();

                RequestDetailViewModel requestDetail = null;

                while (data.Read())
                {
                    requestDetail = new RequestDetailViewModel()
                    {
                        ItemDescription = data["Description"].ToString(),
                        NeededQty = Convert.ToInt32(data["NeededQty"]),
                        RequestedDate = Convert.ToDateTime(data["DateTime"]),
                        CollectionPoint = data["Name"].ToString()
                    };

                    requestDetailList.Add(requestDetail);
                }
            }
            finally
            {
                if (data != null) data.Close();
                connection.Close();
            }

            return requestDetailList;
        }

        public Request GetRequestById(string id)
        {
            string sql = "SELECT * FROM Request WHERE ID = @id";

            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            //connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            Request request = null;

            while (data.Read())
            {
                request = new Request()
                {
                    Id = (int)data["ID"],
                    Status = data["Status"].ToString(),
                    DateTime = Convert.ToDateTime(data["DateTime"]),
                   // DisbursedDate = Convert.ToDateTime(data["DisbursedDate"])
                };
            }

            data.Close();
            //connection.Close();

            return request;
        }

        public string CancelRequest(string id, int empId)
        {
            string sql = "SELECT EmployeeID FROM Request WHERE ID = @id";

            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            int employeeId = 0;

            while (data.Read())
            {
                employeeId = Convert.ToInt32(data["EmployeeID"]);
            }

            data.Close();

            if (employeeId != empId)
            {
                connection.Close();
                return "unauthorized";
            }

            Request req = GetRequestById(id);

            if (req.Status != "Pending")
            {
                connection.Close();
                return "reviewed";
            }

            SqlTransaction transaction = null;
            try
            {
                string cancelRequestSql = "";

                transaction = connection.BeginTransaction();

                sql = "DELETE FROM ItemRequest WHERE RequestID = @id";

                cmd = new SqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@id", id);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();


                cancelRequestSql = "DELETE FROM Request WHERE ID = @id";

                cmd = new SqlCommand(cancelRequestSql, connection, transaction);
                cmd.Parameters.AddWithValue("@id", id);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return "failed";
            }
            finally
            {
                connection.Close();
            }

            return "success";
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
                                "req.EmployeeID IN(SELECT e.ID FROM Employee e WHERE e.DeptID = @deptId) " +
                              " GROUP BY req.ID,e.UserName,req.DateTime,req.Status";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    req = new RequisitionVM()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
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

        public List<MRequestItem> GetRequestItems(int reqId) {
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

        /*DELETE FROM HERE IF SOMETHING WRONG!*/

        public bool CreateRequest(int empId, List<RequestStationery> reqStationery)
        {
            DateTime date = DateTime.Now;
            string sqlFormattedDate = date.ToString("yyyy-MM-dd HH:mm:ss.fff");
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                string sql = "INSERT INTO Request (EmployeeID , DateTime , Status , DisbursedDate) OUTPUT INSERTED.ID VALUES" +
                "(" + empId + ", '" + sqlFormattedDate + "', 'Pending', null)";
                SqlCommand cmd = new SqlCommand(sql, connection, transaction);

                int reqID = Convert.ToInt32(cmd.ExecuteScalar());


                sql = "";
                foreach (var req in reqStationery)
                {

                    sql += "INSERT INTO ItemRequest (RequestID,ItemID, NeededQty, ActualQty) VALUES ('" + reqID + "','" +
                           req.ItemId + "', " + req.Quantity + ", 0 );";
                }
                cmd = new SqlCommand(sql, connection, transaction);
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        /***************************************************************************************/
        public List<RequestReport> GetRequestsByDepartment(string deptId)
        {
            List<RequestReport> requestReports=new List<RequestReport>();
            SqlDataReader reader = null;
            try
            {
                string sql = "Select YEAR(r.DateTime) as DateTime,i.CategoryID,SUM(ir.ActualQty) as ActualQty " +
                    "from Request r,ItemRequest ir, Item as i " +
                    "where r.ID = ir.RequestID and ir.ItemID = i.ID " +
                    "and r.EmployeeID in(select e.ID from Employee as e where e.DeptID=@deptId) "+
                    "group by YEAR(r.DateTime),i.CategoryID,ir.ItemID";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    RequestReport req = new RequestReport()
                    {
                        ReqYear = (int)reader["DateTime"],
                        CategoryID = (int)reader["CategoryID"],
                        Qty = (int)reader["ActualQty"]
                    };
                    requestReports.Add(req);
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
            return requestReports;
        }



        public int getTodayReqCount()
        {

            int count = 0;
                connection.Open();
                string sql = @"select Count(*) from Request where  CONVERT(date,DateTime) = CONVERT(date,getdate());";
                SqlCommand cmd = new SqlCommand(sql, connection);
            count = (int)cmd.ExecuteScalar();
            
            return count;
        }

        public bool ApproveAllRequests(string deptId)
        {
            try
            {
                connection.Open();

                string sql = @"UPDATE Request SET Status = 'Approved' " +
                    " WHERE ID IN( SELECT r.ID FROM Request r, Employee e " +
                    " WHERE r.EmployeeID = e.ID AND e.DeptID = @deptId) AND Status = 'Pending'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId",deptId);
                int count = Convert.ToInt32(cmd.ExecuteNonQuery());
                if (count == 0) throw new Exception();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

    }
}