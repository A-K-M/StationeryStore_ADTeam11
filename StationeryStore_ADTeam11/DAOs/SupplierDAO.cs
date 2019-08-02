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

            conn.Close();
        }

        return suppliers;
    }

    public static Supplier findSupplierbyId(string id)
    {
        Supplier supplier = new Supplier();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT * FROM Supplier WHERE ID = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    supplier = new Supplier()
                    {
                        Id = (string)reader["ID"],
                        Name = (string)reader["Name"],
                        GstNumber = (string)reader["GstNo"],
                        ContactName = (string)reader["ContactName"],
                        PhoneNo = (int)reader["PhoneNo"],
                        Fax = (int)reader["Fax"],
                        Address = (string)reader["Address"]
                    };
                }

                conn.Close();
            }
        }
        catch (Exception e)
        {
            supplier.Id = null;
        }

        return supplier;
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

    public static Supplier editSupplier(string suppId)
    {
        Supplier supplier = new Supplier();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string sql = @"SELECT * FROM Supplier WHERE Supplier.ID = '" + suppId + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                supplier = new Supplier()
                {
                    Id = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    GstNumber = (string)reader["GstNo"],
                    ContactName = (string)reader["ContactName"],
                    PhoneNo = (int)reader["PhoneNo"],
                    Fax = (int)reader["Fax"],
                    Address = (string)reader["Address"]
                };
            }

            conn.Close();
        }

        return supplier;
    }

    public static bool updateSupplier(Supplier updSupplier)
    {
        Supplier supplier = new Supplier();
        Supplier oriSupplier = new Supplier();
        bool updated;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string sql = @"SELECT * FROM Supplier WHERE Supplier.ID = '" + updSupplier.Id + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                oriSupplier = new Supplier()
                {
                    Id = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    GstNumber = (string)reader["GstNo"],
                    ContactName = (string)reader["ContactName"],
                    PhoneNo = (int)reader["PhoneNo"],
                    Fax = (int)reader["Fax"],
                    Address = (string)reader["Address"]
                };
            }

            conn.Close();
        }

        if (oriSupplier.Id == updSupplier.Id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sqlUpdateSupp = @"UPDATE Supplier SET Name = '" + updSupplier.Name + "', GSTNo = '" + updSupplier.GstNumber + "', ContactName = '" + updSupplier.ContactName
                                            + "', PhoneNo = '" + updSupplier.PhoneNo + "', Fax = '" + updSupplier.Fax + "', Address = '" + updSupplier.Address + "' " +
                                            "WHERE ID = '" + oriSupplier.Id + "'";
                    SqlCommand cmd = new SqlCommand(sqlUpdateSupp, conn);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
                updated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                updated = false;
            }
        }
        else
        {
            updated = false;
        }

        return updated;
    }

    public static bool deleteSupplier(string id)
    {
        bool deleted = false;

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlUpdateSupp = @"DELETE FROM Supplier WHERE ID = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sqlUpdateSupp, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            deleted = true;
        }
        catch (Exception e)
        {
            deleted = false;
        }

        return deleted;
    }
}
