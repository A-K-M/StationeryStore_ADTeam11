using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;

namespace StationeryStore_ADTeam11.DAOs
{
    public class ItemRequestDAO:DatabaseConnection
    {
        public List<ItemRequest> GetRequests(string voucherId)
        {
            List<ItemRequest> requests = new List<ItemRequest>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from ItemRequest where RequestID='" + voucherId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                    ItemRequest request = new ItemRequest()
                    {
                        ItemId=(string)reader["ItemID"],
                        NeededQty=(int)reader["NeededQty"],
                        ActualQty = (int)reader["ActualQty"]
                    };
                    requests.Add(request);
            }
            conn.Close();
            return requests;
        }
        public List<ItemRequest> GetTotalDisburseItems(string deptId)
        {
            List<ItemRequest> requests = new List<ItemRequest>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select ir.ItemID,sum(ir.NeededQty) as NeededQty,sum(ir.ActualQty) as ActualQty
                            from Request as r,Employee as e, ItemRequest as ir
                            where r.DisbursedDate is null and r.EmployeeID =e.ID and e.DeptID='" + deptId+"' and ir.RequestID=r.ID " +
                            "group by ir.ItemID";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ItemRequest request = new ItemRequest()
                {
                    ItemId = (string)reader["ItemID"],
                    NeededQty = (int)reader["NeededQty"],
                    ActualQty = (int)reader["ActualQty"]
                };
                requests.Add(request);
            }
            conn.Close();
            return requests;
        }
        public List<ItemRequest> GetDisburseItems(string deptId)
        {
            List<ItemRequest> requests = new List<ItemRequest>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select ir.ID,ir.RequestID ,ir.NeededQty, ir.ActualQty from ItemRequest as ir
                            where not ir.ActualQty=ir.NeededQty and ir.RequestID 
                            in(select r.ID from Request as r, Employee as e
                            where r.EmployeeID=e.ID and e.DeptID='"+deptId+"' and r.DisbursedDate is null)";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ItemRequest request = new ItemRequest()
                {
                    Id = (int)reader["ID"],
                    RequestId=(string)reader["RequestID"],
                    NeededQty = (int)reader["NeededQty"],
                    ActualQty = (int)reader["ActualQty"]
                };
                requests.Add(request);
            }
            conn.Close();
            return requests;
        }
    }
}