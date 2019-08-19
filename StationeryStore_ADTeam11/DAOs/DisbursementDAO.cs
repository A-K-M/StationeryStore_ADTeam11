using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.Util;
using StationeryStore_ADTeam11.View_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class DisbursementDAO : DatabaseConnection
    {
        public List<MDisbursement> GetDisbursementsByClerk(int clerkID)
        {
            List<MDisbursement> disbursements= new List<MDisbursement>();
            
            SqlDataReader reader = null;
            MDisbursement dis = null;
            try
            {
                connection.Open();
                string sql = "spGetRepresentative";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClerkID", clerkID);
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    dis = new MDisbursement()
                    {
                        DeptId = reader["ID"].ToString(),
                        RepName = reader["RepName"].ToString(),
                        DeptName = reader["DeptName"].ToString(),
                        Phone = reader["Phone"].ToString()
                    };
                    disbursements.Add(dis);
                    System.Diagnostics.Debug.WriteLine("MDis Dept  " + dis.DeptId);

                }
                reader.Close();
                List<ItemRequest> items = new List<ItemRequest>();
                sql = "spDisbursementsByClerk";
                cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClerkID", clerkID);
                reader = cmd.ExecuteReader();
                ItemRequest itemRequest = null;
                string deptId;
                int prev = -1;
                while (reader.Read()) {
                    itemRequest = new ItemRequest()
                    {
                        Description = reader["Description"].ToString(),
                        ItemId = reader["ItemID"].ToString(),
                        NeededQty = (int)reader["Total"],
                        ActualQty = (int)reader["Actual"]
                    };
                    deptId = reader["deptID"].ToString();
                    System.Diagnostics.Debug.WriteLine("Dept Reader  " + deptId);

                    // var dis = from d in disbursements where d.DeptId.Equals(deptId) select d;
                    int index = disbursements.FindIndex(d => d.DeptId.Equals(deptId));
                    if (prev == -1) prev = index;

                    if (index != -1)
                    {
                        if (prev != index) { items = new List<ItemRequest>(); }

                        items.Add(itemRequest);
                        disbursements[index].ItemList = items;
                        disbursements[index].CollectionPointID = (int)reader["CollectionPointID"];
                        prev = index;
                    }
      
                }
            }
            catch (Exception e)
            {
               System.Diagnostics.Debug.WriteLine("Error  "+e.Message);
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return disbursements;

        }

        public List<ItemRequest> GetDisburseItemsForRep(string deptId) {
            List<ItemRequest> items = new List<ItemRequest>();
            SqlDataReader reader = null;
            ItemRequest itemRequest = null;
            try
            {
                connection.Open();
                string sql = "spDisburseItemsForRep";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeptID", deptId);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itemRequest = new ItemRequest()
                    {
                        Description = reader["Description"].ToString(),
                        ItemId = reader["ID"].ToString(),
                        NeededQty = (int)reader["Needed"],
                        ActualQty = (int)reader["Actual"]
                    };
                    items.Add(itemRequest);
                }

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
            return items;

        }

        public bool UpdateDisbursement(string deptId,int clerkId, List<ItemRequest> items) {
            List<MAdjustmentItem> adjItems = new List<MAdjustmentItem>();
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                SqlCommand cmd = null;
                SqlDataReader reader = null;
                string reqSql = "";
                string irSql = "";
                string outSql = "";
                foreach (ItemRequest item in items)
                {

                    int updatedQty = item.ActualQty;
                    string sql = @" SELECT ir.RequestID,ir.ID,ir.NeededQty,ir.ActualQty
                                        FROM Request r,ItemRequest ir , Employee e
                                        WHERE R.ID = ir.RequestID AND r.EmployeeID = e.ID AND e.DeptID = @deptId
	                                          AND r.Status = 'Disbursed' AND ir.ItemID = @itemId
                                        ORDER BY r.DateTime DESC";
                    cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@deptId", deptId);
                    cmd.Parameters.AddWithValue("@itemId", item.ItemId);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (updatedQty == 0)
                        {
                            break;
                        }
                        int actual = (int)reader["ActualQty"];
                        int irID = (int)reader["ID"];
                        int reqId = (int)reader["RequestID"];
                        if (updatedQty >= actual)
                        {
                            updatedQty -= actual;
                          //  reqSql += sqlUpdateRequestStatus(reqId);
                            irSql += sqlUpdateItemReqQuantity(irID, 0);
                            outSql += sqlCreateOutstanding(irID, actual);
                        }
                        else if (updatedQty < actual)
                        {
                            irSql += sqlUpdateItemReqQuantity(irID, actual - updatedQty);
                            outSql += sqlCreateOutstanding(irID, updatedQty);
                            updatedQty = 0;
                        }
                    }
                    reader.Close();


                    //for create adjustment
                    adjItems.Add(new MAdjustmentItem()
                    {
                        ItemId = item.ItemId,
                        Description = item.Description,
                        Quantity = item.ActualQty,
                        Reason = "Disburse Edit"
                    });
                }
                transaction = connection.BeginTransaction();

                if (reqSql != "")
                {
                   SqlCommand cmd1 = new SqlCommand(reqSql, connection,transaction);
                    if (cmd1.ExecuteNonQuery() == 0) throw new Exception();
                }
                if (irSql != "")
                {
                    SqlCommand cmd2 = new SqlCommand(irSql, connection,transaction);
                    if (cmd2.ExecuteNonQuery() == 0) throw new Exception();
                }
                if (outSql != "")
                {
                    SqlCommand cmd3 = new SqlCommand(outSql, connection,transaction);
                    if (cmd3.ExecuteNonQuery() == 0) throw new Exception();
                }
                transaction.Commit();

            }
            catch
            {
                if (transaction != null) transaction.Rollback();
                return false;
            }
            finally {
                connection.Close();
            }
            //Create Adjustment Vouchers
            AdjustmentVoucherDAO adjDAO = new AdjustmentVoucherDAO();
           return  adjDAO.CreateAdjVoucher(clerkId, adjItems);
        }

        private string sqlCreateOutstanding(int irID, int qty)
        {
            return "INSERT INTO Outstanding(ItemRequestID,Qty,Status) VALUES("+irID+", "+qty+", '"+Constant.STATUS_PENDING+"') ;\n";
        }

        private string sqlUpdateItemReqQuantity(int irID, int qty)
        {
            return "UPDATE ItemRequest SET ActualQty ="+qty+" WHERE ID = " + irID + ";\n";
        }

        private string sqlUpdateRequestStatus(int reqId)
        {
            return "UPDATE Request SET Status = '"+Constant.STATUS_APPROVE+"' WHERE ID = " + reqId + ";\n";
        }

        public bool ApproveDisbursement(string deptId)
        {
            try
            {
                connection.Open();
                string sql = "spApproveDisbursement";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeptID", deptId);
                cmd.Parameters.AddWithValue("@Date", DateUtils.now());
                
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

        public List<DisbursementVM> WebGetDisbursementsByClerk(int clerkID)
        {
            List<DisbursementVM> disbursements = new List<DisbursementVM>();

            SqlDataReader reader = null;
            DisbursementVM dis = null;
            try
            {
                connection.Open();
                string sql = "spGetRepresentative";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClerkID", clerkID);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dis = new DisbursementVM()
                    {
                        DeptId = reader["ID"].ToString(),
                        RepName = reader["RepName"].ToString(),
                        DeptName = reader["DeptName"].ToString()
                    };
                    disbursements.Add(dis);
                    System.Diagnostics.Debug.WriteLine("MDis Dept  " + dis.DeptId);

                }
                reader.Close();
                List<ItemRequest> items = new List<ItemRequest>();
                sql = "spDisbursementsByClerk";
                cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClerkID", clerkID);
                reader = cmd.ExecuteReader();
                ItemRequest itemRequest = null;
                string deptId;
                int prev = -1;
                while (reader.Read())
                {
                    itemRequest = new ItemRequest()
                    {
                        Description = reader["Description"].ToString(),
                        ItemId = reader["ItemID"].ToString(),
                        NeededQty = (int)reader["Total"],
                        ActualQty = (int)reader["Actual"]
                    };
                    deptId = reader["deptID"].ToString();
                    System.Diagnostics.Debug.WriteLine("Dept Reader  " + deptId);

                    // var dis = from d in disbursements where d.DeptId.Equals(deptId) select d;
                    int index = disbursements.FindIndex(d => d.DeptId.Equals(deptId));
                    if (prev == -1) prev = index;

                    if (index != -1)
                    {
                        if (prev != index) { items = new List<ItemRequest>(); }

                        items.Add(itemRequest);
                        disbursements[index].ItemList = items;
                        disbursements[index].CollectionPointID = (int)reader["CollectionPointID"];
                        prev = index;
                    }
                    
                }
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error  " + e.Message);

                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return disbursements;

        }
    }
}