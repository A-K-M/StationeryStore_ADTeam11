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

            string sql = @"SELECT po.ID, po.Date, po.Status, e.Name
                        FROM PurchaseOrder po, Employee e
                        WHERE Status = @status
                        AND po.EmpID = e.ID";

            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@status", status);

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

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
            data.Close();
            connection.Close();

            return reorderStockLists;
        }

        public List<PurchaseOrderItem> ReorderStockDetail(int id)
        {
            List<PurchaseOrderItem> detailList = new List<PurchaseOrderItem>();

            string sql = @"SELECT poi.*, i.FirstPrice, i.FirstSupplier
                            FROM PurchaseOrderItem poi, Item i
                            WHERE PurchaseID = @id
                            AND i.ID = poi.ItemID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

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
            data.Close();
            connection.Close();

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

            string sql = @"SELECT * FROM PurchaseOrder WHERE Status = 'Approved'";

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

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
            data.Close();
            connection.Close();

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

            ReorderStockListVM purchaseOrder = null;

            string sql = @"SELECT *
                        FROM PurchaseOrder
                        WHERE Status IN ('Ordered', 'Delivered', 'Processing')";

            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader data = cmd.ExecuteReader();

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
            data.Close();
            connection.Close();

            return purchaseOrders;
        }


    }
}