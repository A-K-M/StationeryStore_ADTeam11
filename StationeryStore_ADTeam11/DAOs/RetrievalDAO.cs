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
                while (reader.Read()) {
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


        public bool InsertRetrievals(List<Retrieval> retrievals,int clerkId) {
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                string sql = "";
                DateTime date = DateTime.Now;

                transaction = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand(sql, connection,transaction);
                foreach (Retrieval retrieval in retrievals) {
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
            string sql = @"select list.itemid as itemid, SUM(list.NeededQty) as sum from (select i.ID as itemid,r.ID as reqid,ir.NeededQty from Request r, ItemRequest ir, Item i 
                        where status = 'Approved' and r.ID = ir.RequestID and ir.ItemID = i.id) as list group by list.itemid";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Retrieval r = new Retrieval();
                r.ItemId = (string)reader["ItemID"];
                r.Qty = (int)reader["sum"];
                retrievals.Add(r);
            }
            conn.Close();
            return retrievals;
        }
        public List<ItemRequest> GetReqDeptListByItemId()
        {
            List<ItemRequest> list = new List<ItemRequest>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select (select name from  where id = (select deptid from Employee where id = r.EmployeeID)) as dept, 
                            ir.NeededQty ,
                                ir.ItemId
                           from Request r, ItemRequest ir
                            where r.ID = ir.RequestID  and Status = 'approved'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ItemRequest ir = new ItemRequest();
                ir.ItemId = (string)reader["ItemId"];
             //   ir.DeptName = (string)reader["dept"];
                ir.NeededQty = (int)reader["NeededQty"];
                list.Add(ir);
            }
            conn.Close();
            return list;
        }
        public List<Outstanding> GetPendingOutstandingsListByItemId()
        {
            List<Outstanding> outstandings = new List<Outstanding>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select (select name from Department where id = (select deptid from Employee where id = r.EmployeeID)) as dept,
                        (ir.NeededQty - ir.ActualQty) as remaining,
                        r.DateTime as ReqDate,
                        ir.ItemId
                        from ItemRequest ir , Request r
                        where ir.id in (select ItemRequestID from Outstanding where Status = 'Pending')
                        and ir.RequestID = r.ID  order by ReqDate";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Outstanding o = new Outstanding();
                o.ItemId = (string)reader["ItemId"];
                o.ReqId = (string)reader["reqid"];
                o.RemainingQty = (int)reader["remaining"];
                o.DateTime = (DateTime)reader["reqdate"];
                outstandings.Add(o);
            }
            conn.Close();
            return outstandings;
        }
        
        public bool checkOutstandingList(string itemId)
        {
            SqlConnection conn = connection;
            bool hasOutstanding = false;
            conn.Open();
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
            conn.Close();
            return hasOutstanding;
        }
    }
}
