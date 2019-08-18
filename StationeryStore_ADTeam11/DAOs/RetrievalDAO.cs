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
    public class RetrievalDAO : DatabaseConnection
    {
        public List<Retrieval> GetRetrievalList()
        {
            List<Retrieval> retrievals = new List<Retrieval>();
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                string sql = "spRetrievalList";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Retrieval r = new Retrieval();
                    r.ItemId = (string)reader["itemid"];
                    r.Qty = (int)reader["total"];
                    r.Description = reader["Description"].ToString();
                    r.BinNo = reader["BinNo"].ToString();
                    r.RetrievalQty = r.Qty;
                    retrievals.Add(r);
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
            return retrievals;
        }
        public bool InsertRetrievals(List<Retrieval> retrievals, int clerkId)
        {
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                string sql = "";
                DateTime date = DateTime.Now;

                transaction = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand(sql, connection, transaction);
                foreach (Retrieval retrieval in retrievals)
                {
                    sql += $"INSERT INTO Retrieval (ItemID,Date,Status,EmployeeID,RetrievalQty) VALUES('{retrieval.ItemId}','{date}','{Constant.STATUS_PENDING}','{clerkId}','{retrieval.RetrievalQty}');\n";
                }
                if (cmd.ExecuteNonQuery() != retrievals.Count()) throw new Exception();


                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null) transaction.Rollback();
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;

        }
        public List<Retrieval> GetItemsAndQty()
        {
            List<Retrieval> retrievals = new List<Retrieval>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select list.itemid as itemid, SUM(list.NeededQty) as sum from (select i.ID as itemid,r.ID as reqid,ir.NeededQty 
                        from Request r, ItemRequest ir, Item i 
                        where r.Status  in ('Disbursed', 'Approved') and r.ID = ir.RequestID and ir.ItemID = i.id) as list group by list.itemid";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Retrieval r = new Retrieval();
                r.ItemId = (string)reader["ItemID"];
                r.Qty = (int)reader["sum"];

                retrievals.Add(r);
            }
            reader.Close();
            conn.Close();
            return retrievals;
        }
        public int GetOutstandingQtyByItemId(string itemId)
        {
            int qty = 0;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select (ir.NeededQty - ir.ActualQty) as remaining
                        from ItemRequest ir , Request r
                        where ir.id in (select ItemRequestID from Outstanding where Status = 'Pending')
                        and ir.RequestID = r.ID 
                        and ir.ItemID = '" + itemId + "' ";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                qty = (int)reader["remaining"];
            }
            reader.Close();
            conn.Close();
            return qty;
        }
        public List<ItemRequest> GetReqDeptListByItemId(string itemId)
        {
            List<ItemRequest> list = new List<ItemRequest>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select (select name from Department where id = (select deptid from Employee where id = r.EmployeeID)) as dept, 
                            ir.NeededQty ,
                                ir.ItemId, ir.RequestID, ir.id
                           from Request r, ItemRequest ir
                            where ir.ItemID = '" + itemId + "' and r.ID = ir.RequestID  and Status in ('Approved', 'Disbursed')";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ItemRequest ir = new ItemRequest();
                ir.ItemId = (string)reader["ItemId"];
                ir.DeptName = (string)reader["dept"];
                ir.NeededQty = (int)reader["NeededQty"];
                ir.RequestId = (int)reader["RequestID"];
                ir.Id = (int)reader["id"];
                list.Add(ir);
            }
            reader.Close();
            conn.Close();
            return list;
        }
        public List<Outstanding> GetPendingOutstandingsListByItemId(string itemId)
        {
            List<Outstanding> outstandings = new List<Outstanding>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select (select name from Department where id = (select deptid from Employee where id = r.EmployeeID)) as dept,
                        (ir.NeededQty - ir.ActualQty) as remaining,
                        r.DateTime as ReqDate,
                        ir.ItemId,
						o.id
                        from ItemRequest ir , Request r, Outstanding o 
                        where ir.id in (select ItemRequestID from Outstanding where Status = 'Pending')
                        and ir.RequestID = r.ID 
						and o.ItemRequestID = ir.ID
                        and ir.ItemID = '" + itemId + "' order by ReqDate";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Outstanding o = new Outstanding();
                o.Id = (int)reader["id"];
                o.ItemId = (string)reader["ItemId"];
                o.Dept = (string)reader["dept"];
                o.RemainingQty = (int)reader["remaining"];
                o.DateTime = (DateTime)reader["reqdate"];
                outstandings.Add(o);
            }
            reader.Close();
            conn.Close();
            return outstandings;
        }
        public bool checkOutstandingList(string itemId)
        {
            SqlConnection conn = connection;
            conn.Open();
            bool hasOutstanding = false;
            string sql = @"select o.* from Outstanding o , ItemRequest ir  
                        where o.ItemRequestID = ir.ID 
                        and o.Status = 'pending' 
                        and ir.ItemID = '" + itemId + "' ";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                hasOutstanding = true;
            }
            reader.Close();
            conn.Close();
            return hasOutstanding;
        }
        public void CreateRetrieval(int clerkId, List<Retrieval> list)
        {
           
            SqlConnection conn = connection;
            conn.Open();
            string sql = "";
            foreach (Retrieval item in list)
            {
                sql += "  insert into Retrieval(itemID, Status, EmployeeID, RetrievalQty) values ('" + item.ItemId + "', 'Approved', '" + clerkId + "', " + item.Qty + " );";

            }
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();

         
            foreach (Retrieval r in list)
            {
                string out_ids = "";
                string req_ids = "";
                string ir_query = "";
                string ItemId = r.ItemId;
                int retrieved = r.RetrievalQty;
                List<Outstanding> out_list = GetPendingOutstandingsListByItemId(ItemId);
                if (out_list != null)
                {
                    foreach (Outstanding row in out_list)
                    {
                        int real = 0;
                        if (retrieved > 0)
                        {
                            real = retrieved;
                        }
                        retrieved -= row.RemainingQty;
                        if (retrieved >= 0)
                        {
                            out_ids += row.Id.ToString() + ", ";
                            real = retrieved;
                        }
                    }
                    out_ids = out_ids.TrimEnd(',', ' ');
                    if (out_ids != "")
                        UpdateOutstanding(out_ids);
                }


                List<ItemRequest> request_list = GetReqDeptListByItemId(ItemId);
                if (request_list != null && retrieved > 0)
                {
                    foreach (ItemRequest request in request_list)
                    {
                        int real = 0;
                        if (retrieved > 0)
                        {
                            real = retrieved;
                        }
                        retrieved -= request.NeededQty;
                        if (retrieved >= 0)
                        {
                            req_ids += request.RequestId.ToString() + ", ";
                            real = retrieved;
                            UpdateItemRequest(request.Id, request.NeededQty);
                        }
                        else
                        {
                            ir_query += CreateOutstandingQuery(request.Id, request.NeededQty - real);
                            UpdateItemRequest(request.Id, real);
                        }
                    }
                    req_ids = req_ids.TrimEnd(',', ' ');
                    if (req_ids != "")
                        UpdateRequestStatus(req_ids);
                    if (ir_query != "")
                        CreateOutstanding(ir_query);
                }
            }

        }
        public string GetItemDescByItemId(string itemId)
        {
            string itemDesc = "";
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select Description from item where id = '" + itemId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                itemDesc = (string)reader["Description"];
            }
            reader.Close();
            conn.Close();
            return itemDesc;
        }
        public void UpdateOutstanding(string ids)
        {
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"update Outstanding set status ='Disbursed' where id in (" + ids + ") ;";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public void UpdateRequestStatus(string ids)
        {
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"update Request set status ='Disbursed' where id in (" + ids + ") ;";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public string CreateOutstandingQuery(int ir_id, int qty)
        {
            return "insert into Outstanding  (ItemRequestID, qty, Status,DateTime) values (" + ir_id + "," + qty + ",'Pending','"+DateTime.Now+"');";
        }
        public void UpdateItemRequest(int ir_id, int actual_qty)
        {
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"update ItemRequest set ActualQty = " + actual_qty + " where id = " + ir_id + ";\n";

            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public void CreateOutstanding(string query)
        {
            SqlConnection conn = connection;
            conn.Open();
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public List<ItemRequest> GetItemRequestAndDepts(string itemId)
        {
            List<ItemRequest> list = new List<ItemRequest>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select (select name from Department where id = (select deptid from Employee where id = r.EmployeeID)) as dept, 
                            ir.NeededQty ,
                                ir.ItemId, ir.RequestID, ir.id, ir.ActualQty
                           from Request r, ItemRequest ir
                            where ir.ItemID = '" + itemId + "' and r.ID = ir.RequestID  and Status  in ('Disbursed', 'Approved') ";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ItemRequest ir = new ItemRequest();
                ir.ItemId = (string)reader["ItemId"];
                ir.DeptName = (string)reader["dept"];
                ir.NeededQty = (int)reader["NeededQty"];
                ir.RequestId = (int)reader["RequestID"];
                ir.ActualQty = (int)reader["ActualQty"];
                ir.Id = (int)reader["id"];
                list.Add(ir);
            }
            reader.Close();
            conn.Close();
            return list;
        }
    }
}
