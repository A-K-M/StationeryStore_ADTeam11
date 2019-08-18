using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class SupplierItemDAO : DatabaseConnection
    {
        public bool Selected { get; internal set; }

        public List<Supplier> GetSupplierNames()
        {
            List<Supplier> suppliernames = new List<Supplier>();
            Supplier supplier = null;
            string sql = "SELECT * FROM Supplier";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader data = cmd.ExecuteReader();
            while (data.Read())
            {
                supplier = new Supplier()
                {
                    Id = data["ID"].ToString(),
                    Name = data["Name"].ToString()
                };
                suppliernames.Add(supplier);
            }
            data.Close();
            connection.Close();
            return suppliernames;
        }
    }
}