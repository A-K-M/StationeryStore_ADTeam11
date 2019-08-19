using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class PurchaseOrderDAO : DatabaseConnection
    {
        public List<ReorderStockListVM> ReorderStockLists(string status)
        {
            List<ReorderStockListVM> reorderStockLists = new List<ReorderStockListVM>();
            SqlDataReader data = null;
            try
            {
                string sql = @"SELECT po.ID, po.Date, po.Status, e.Name
                        FROM PurchaseOrder po, Employee e
                        WHERE Status = @status
                        AND po.EmpID = e.ID";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@status", status);

                connection.Open();

                data = cmd.ExecuteReader();

                ReorderStockListVM stocks = null;

                while (data.Read())
                {
                    stocks = new ReorderStockListVM()
                    {
                        Id = Convert.ToInt32(data["ID"]),
                        RequestedDate = Convert.ToDateTime(data["Date"]),
                        Status = data["Status"].ToString(),
                        EmpName = data["Name"].ToString()
                    };

                    reorderStockLists.Add(stocks);
                }
            }
            finally {
                if(data != null)data.Close();
                connection.Close();
            }
           

            return reorderStockLists;
        }

        public List<PurchaseOrderItem> ReorderStockDetail(int id)
        {
            List<PurchaseOrderItem> detailList = new List<PurchaseOrderItem>();
            SqlDataReader data = null;

            try
            {
                string sql = @"SELECT poi.*, i.FirstPrice, i.FirstSupplier
                            FROM PurchaseOrderItem poi, Item i
                            WHERE PurchaseID = @id
                            AND i.ID = poi.ItemID";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();

                data = cmd.ExecuteReader();

                PurchaseOrderItem item = null;

                while (data.Read())
                {
                    item = new PurchaseOrderItem()
                    {
                        Id = Convert.ToInt32(data["PurchaseID"]),
                        ItemId = data["ItemID"].ToString(),
                        PurchaseId = Convert.ToInt32(data["PurchaseID"]),
                        Description = data["Description"].ToString(),
                        Qty = Convert.ToInt32(data["Qty"]),
                        Status = data["Status"].ToString(),
                        Price = Convert.ToDouble(data["FirstPrice"]),
                        Supplier = data["FirstSupplier"].ToString(),
                        Amount = Convert.ToDouble(data["FirstPrice"]) * Convert.ToInt32(data["Qty"])
                    };
                    detailList.Add(item);
                }
            }
            finally
            {
                if (data != null) data.Close();
                connection.Close();
            }


            return detailList;
        }

        public bool ReviewReorderStock(int id, string status)
        {
            string sql = "UPDATE PurchaseOrder SET Status = '" + status + "' WHERE ID = " + id;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {                
                connection.Close();
                return false;
            }
            finally
            {
                connection.Close();
                
            }

            return true;
            //cmd.Parameters.AddWithValue("@id", id);
            //cmd.Parameters.AddWithValue("@status", status);

        }

        public List<ReorderStockListVM> ApprovedReorderStockList()
        {
            List<ReorderStockListVM> reorderStockLists = new List<ReorderStockListVM>();
            SqlDataReader data = null;

            try
            {
                string sql = @"SELECT * FROM PurchaseOrder WHERE Status = 'Approved'";

                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();

                data = cmd.ExecuteReader();

                ReorderStockListVM stocks = null;

                while (data.Read())
                {
                    stocks = new ReorderStockListVM()
                    {
                        Id = Convert.ToInt32(data["ID"]),
                        RequestedDate = Convert.ToDateTime(data["Date"]),
                        Status = data["Status"].ToString()
                    };

                    reorderStockLists.Add(stocks);
                }
            }
            finally
            {
                if (data != null) data.Close();
                connection.Close();
            }


            return reorderStockLists;
        }

        public bool OrderStockList(int id)
        {
            string sql = "UPDATE PurchaseOrder SET Status = 'Ordered' WHERE ID = " + id;

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                connection.Close();
                return false;
            }
            finally
            {
                connection.Close();

            }

            return true;
        }

        public List<ReorderStockListVM> PurchaseOrderHistory()
        {
            List<ReorderStockListVM> purchaseOrders = new List<ReorderStockListVM>();
            SqlDataReader data = null;

            ReorderStockListVM purchaseOrder = null;
            try
            {
                string sql = @"SELECT *
                        FROM PurchaseOrder
                        WHERE Status IN ('Ordered', 'Delivered', 'Processing')";

                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                data = cmd.ExecuteReader();

                while (data.Read())
                {
                    purchaseOrder = new ReorderStockListVM()
                    {
                        Id = Convert.ToInt32(data["ID"]),
                        RequestedDate = Convert.ToDateTime(data["Date"]),
                        Status = data["Status"].ToString()
                    };

                    purchaseOrders.Add(purchaseOrder);
                }
            }
            finally
            {
                if (data != null) data.Close();
                connection.Close();
            }

            return purchaseOrders;
        }

        public bool MarkItems(int pid, string itemId)
        {
            string sql = @"UPDATE PurchaseOrderItem
                        SET Status = 'Delivered'
                        WHERE PurchaseID = @pid
                        AND ItemID = @itemId";

            SqlTransaction transaction = null;

            try
            {
                connection.Open();

                transaction = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand(sql, connection, transaction);

                cmd.Parameters.AddWithValue("@pid", pid);
                cmd.Parameters.AddWithValue("@itemId", itemId);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                string status = "Ordered";

                string statusSql = @"SELECT Status
                                FROM PurchaseOrderItem
                                where PurchaseID = @id";

                cmd = new SqlCommand(statusSql, connection, transaction);
                cmd.Parameters.AddWithValue("@id", pid);

                SqlDataReader data = cmd.ExecuteReader();


                List<string> item_status = new List<string>();

                while (data.Read())
                {
                    item_status.Add(data["Status"].ToString());
                }

                data.Close();

                int d_count = 0;
                foreach (string s in item_status)
                {
                    if (s == "Delivered")
                    {
                        status = "Processing";
                        d_count++;
                    }
                }
                if (d_count == item_status.Count())
                    status = "Delivered";

                string updateStatusSql = @"UPDATE PurchaseOrder SET
                                    Status = @status
                                    WHERE ID = @id";

                cmd = new SqlCommand(updateStatusSql, connection, transaction);

                cmd.Parameters.AddWithValue("@id", pid);
                cmd.Parameters.AddWithValue("@status", status);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                string updateStockcardSql = $@"INSERT INTO Stockcard
                                            (ItemID, DateTime, Qty, Balance, RefType)
                                            VALUES ('{itemId}', GETDATE(), 
			                                            (SELECT Qty FROM PurchaseOrderItem 
			                                            WHERE PurchaseID = {pid} AND ItemID = '{itemId}'),
			                                            (SELECT TOP 1 Balance 
			                                            FROM Stockcard WHERE ItemID = '{itemId}'
			                                            ORDER BY ID DESC) + (SELECT Qty FROM PurchaseOrderItem 
			                                            WHERE PurchaseID = {pid} AND ItemID = '{itemId}'),
			                                            'ORD-{pid}')";

                cmd = new SqlCommand(updateStockcardSql, connection, transaction);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

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

        public bool UpdatePurchaseOrderStatus(int id)
        {
            string status = "Ordered";

            string sql = @"SELECT Status
                        FROM PurchaseOrderItem
                        where PurchaseID = @id";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();


            List<string> item_status = new List<string>();

            while (data.Read())
            {
                item_status.Add(data["Status"].ToString());
            }

            data.Close();

            int d_count = 0;
            foreach (string s in item_status)
            {
                if (s == "Delivered")
                {
                    status = "Processing";
                    d_count++;
                }
            }
            if(d_count==item_status.Count())
                status = "Delivered";

            string updateStatusSql = @"UPDATE PurchaseOrderItem SET
                                    Status = @status
                                    WHERE PurchaseID = @id";

            cmd = new SqlCommand(updateStatusSql, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@status", status);

            if (cmd.ExecuteNonQuery() == 0)
            {
                connection.Close();
                return false;
            }

            connection.Close();
            return true;
        }
    }
}