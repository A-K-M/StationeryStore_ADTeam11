using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
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
    }
}