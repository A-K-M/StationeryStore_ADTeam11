using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using StationeryStore_ADTeam11.Models;

/// <summary>
/// Summary description for Class1
/// </summary>
public class SupplierDAO : DAO
{
    public static List<Supplier> getAllSuppliers()
    {
        List<Supplier> suppliers = new List<Supplier>(); 

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sql = @"SELECT * FROM Supplier";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Supplier supplier = new Supplier()
                {
                    Id = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    GstNumber = (string)reader["GstNo"],
                    ContactName = (string)reader["ContactName"],
                    PhoneNo = (int)reader["PhoneNo"],
                    Fax = (int)reader["Fax"],
                    Address = (string)reader["Address"]
                };
                suppliers.Add(supplier);
            }
        }

        return suppliers;
    }

    public static bool addSupplier(Supplier supp)
    {
        bool saved = false;

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlAddSupp = @"INSERT INTO Supplier (ID, Name, GSTNo, ContactName, PhoneNo, Fax, Address) 
                                VALUES ('" + supp.Id + "', '" + supp.Name + "', '" + supp.GstNumber + "', '" + supp.ContactName + "', "
                                            + supp.PhoneNo + ", " + supp.Fax + ", '" + supp.Address + "')";
                SqlCommand cmd = new SqlCommand(sqlAddSupp, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            saved = true;
        }
        catch (Exception e)
        {
            saved = false;
        }


        return saved;
    }
}
