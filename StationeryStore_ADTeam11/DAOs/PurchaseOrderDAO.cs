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

            return reorderStockLists;
        }
    }
}