using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.DAOs;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace StationeryStore_ADTeam11.DAOs
{
    public class SupplierDAO : DatabaseConnection
    {
        public List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            connection.Open();

            string sql = @"SELECT * FROM Supplier";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Supplier supplier = new Supplier()
                {
                    Id = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    ContactName = (string)reader["ContactName"],
                    PhoneNo = (int)reader["PhoneNo"],
                    Fax = (int)reader["Fax"],
                    Address = (string)reader["Address"]
                };

                try
                {
                    supplier.GstNumber = (string)reader["GstNo"];
                }
                catch (Exception e)
                {
                    supplier.GstNumber = "Nil";
                }

                suppliers.Add(supplier);
            }

            connection.Close();


            return suppliers;
        }

        public Supplier FindSupplierbyId(string id)
        {
            Supplier supplier = new Supplier();

            try
            {
                    connection.Open();

                    string sql = @"SELECT * FROM Supplier WHERE ID = '" + id + "'";
                    SqlCommand cmd = new SqlCommand(sql, connection);
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

                    connection.Close();
            }
            catch (Exception e)
            {
                supplier.Id = null;
            }

            return supplier;
        }

        public bool AddSupplier(Supplier supp)
        {
            bool saved = false;

            supp.Id = supp.Id.ToUpper();

            try
            {
                connection.Open();

                string sqlAddSupp = @"INSERT INTO Supplier (ID, Name, GSTNo, ContactName, PhoneNo, Fax, Address) 
                            VALUES ('" + supp.Id + "', '" + supp.Name + "', '" + supp.GstNumber + "', '" + supp.ContactName + "', "
                                            + supp.PhoneNo + ", " + supp.Fax + ", '" + supp.Address + "')";
                SqlCommand cmd = new SqlCommand(sqlAddSupp, connection);
                cmd.ExecuteNonQuery();

                connection.Close();

                saved = true;
            }
            catch (Exception e)
            {
                saved = false;
            }


            return saved;
        }

        public Supplier EditSupplier(string suppId)
        {
            Supplier supplier = new Supplier();


                connection.Open();

                string sql = @"SELECT * FROM Supplier WHERE Supplier.ID = '" + suppId + "'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    supplier = new Supplier()
                    {
                        Id = (string)reader["ID"],
                        Name = (string)reader["Name"],
                        ContactName = (string)reader["ContactName"],
                        PhoneNo = (int)reader["PhoneNo"],
                        Fax = (int)reader["Fax"],
                        Address = (string)reader["Address"]
                    };
                }

                try
                {
                    supplier.GstNumber = (string)reader["GstNo"];
                }
                catch (Exception e)
                {
                    supplier.GstNumber = "Nil";
                }

                connection.Close();

            return supplier;
        }

        public bool UpdateSupplier(Supplier updSupplier)
        {
            Supplier supplier = new Supplier();
            Supplier oriSupplier = new Supplier();
            bool updated;


                connection.Open();

                string sql = @"SELECT * FROM Supplier WHERE Supplier.ID = '" + updSupplier.Id + "'";
                SqlCommand cmd = new SqlCommand(sql, connection);
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

                connection.Close();

            if (oriSupplier.Id == updSupplier.Id)
            {
                try
                {
                    
                    connection.Open();

                    string sqlUpdateSupp = @"UPDATE Supplier SET Name = '" + updSupplier.Name + "', GSTNo = '" + updSupplier.GstNumber + "', ContactName = '" + updSupplier.ContactName
                                            + "', PhoneNo = '" + updSupplier.PhoneNo + "', Fax = '" + updSupplier.Fax + "', Address = '" + updSupplier.Address + "' " +
                                            "WHERE ID = '" + oriSupplier.Id + "'";
                    SqlCommand cmd2 = new SqlCommand(sqlUpdateSupp, connection);
                    cmd2.ExecuteNonQuery();

                    connection.Close();
                    
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

        public bool DeleteSupplier(string id)
        {
            bool deleted = false;

            try
            {
               
                connection.Open();

                string sqlUpdateSupp = @"DELETE FROM Supplier WHERE ID = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sqlUpdateSupp, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
                
                deleted = true;
            }
            catch (Exception e)
            {
                deleted = false;
            }

            return deleted;
        }
    }
}
