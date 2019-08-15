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

        public Supplier FindSupplierById(string id)
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
                    try
                    {
                        supplier.GstNumber = (string)reader["GstNo"];
                    }
                    catch (Exception e)
                    {
                        supplier.GstNumber = "Nil";
                    }
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

        public List<Supplier> FindSuppliersByItemId(string id)
        {
            List<Supplier> suppliers = new List<Supplier>();
            Supplier supplier = new Supplier();
            Item item = new Item();

            connection.Open();

            try
            {
                string sql = @"Select ID FROM Item WHERE Id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        item = new Item()
                        {
                            Id = (string)reader["ID"]
                        };
                    }
                    catch (Exception e)
                    {
                        item.Id = null;
                    }

                }

            }
            catch (Exception e)
            {
                item.Id = null;
            }
            
            connection.Close();

            if (item.Id == id)
            {
                connection.Open();

                try
                {
                    string sql = @"Select S.ID, S.Name, S.GstNo, S.ContactName, S.PhoneNo, S.Fax, S.Address 
                                    FROM Supplier S JOIN Item I ON I.FirstSupplier = S.ID WHERE I.Id = '" + id + "'";
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

                        try
                        {
                            supplier.GstNumber = (string)reader["GstNo"];
                        }
                        catch (Exception e)
                        {
                            supplier.GstNumber = "Nil";
                        }

                    }

                }
                catch (Exception e)
                {
                    supplier = null;
                }
                suppliers.Add(supplier);

                connection.Close();
                connection.Open();

                try
                {
                    string sql2 = @"Select S.ID, S.Name, S.GstNo, S.ContactName, S.PhoneNo, S.Fax, S.Address 
                                    FROM Supplier S JOIN Item I ON I.SecondSupplier = S.ID WHERE I.Id = '" + id + "'";
                    SqlCommand cmd2 = new SqlCommand(sql2, connection);
                    SqlDataReader reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        supplier = new Supplier()
                        {
                            Id = (string)reader2["ID"],
                            Name = (string)reader2["Name"],
                            ContactName = (string)reader2["ContactName"],
                            PhoneNo = (int)reader2["PhoneNo"],
                            Fax = (int)reader2["Fax"],
                            Address = (string)reader2["Address"]
                        };

                        try
                        {
                            supplier.GstNumber = (string)reader2["GstNo"];
                        }
                        catch (Exception e)
                        {
                            supplier.GstNumber = "Nil";
                        }

                    }

                }
                catch (Exception e)
                {
                    supplier = null;
                }
                suppliers.Add(supplier);

                connection.Close();
                connection.Open();

                try
                {
                    string sql3 = @"Select S.ID, S.Name, S.GstNo, S.ContactName, S.PhoneNo, S.Fax, S.Address 
                                    FROM Supplier S JOIN Item I ON I.ThirdSupplier = S.ID WHERE I.Id = '" + id + "'";
                    SqlCommand cmd3 = new SqlCommand(sql3, connection);
                    SqlDataReader reader3 = cmd3.ExecuteReader();

                    while (reader3.Read())
                    {
                        supplier = new Supplier()
                        {
                            Id = (string)reader3["ID"],
                            Name = (string)reader3["Name"],
                            ContactName = (string)reader3["ContactName"],
                            PhoneNo = (int)reader3["PhoneNo"],
                            Fax = (int)reader3["Fax"],
                            Address = (string)reader3["Address"]
                        };

                        try
                        {
                            supplier.GstNumber = (string)reader3["GstNo"];
                        }
                        catch (Exception e)
                        {
                            supplier.GstNumber = "Nil";
                        }

                    }

                }
                catch (Exception e)
                {
                    supplier = null;
                }
                suppliers.Add(supplier);

                connection.Close();

            }

            else
                suppliers = null;

            return suppliers;
        }

        public List<Supplier> FindSuppliersExceptId(string id)
        {
            List <Supplier> suppliers = new List<Supplier>();
            Supplier supplier = new Supplier();

            try
            {
                connection.Open();

                string sql = @"SELECT * FROM Supplier WHERE ID != '" + id + "'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    supplier = new Supplier()
                    {
                        Id = (string)reader["ID"],
                        Name = (string)reader["Name"]
                    };
                    suppliers.Add(supplier);
                }

                connection.Close();
            }
            catch (Exception e)
            {
                suppliers = null;
            }

            return suppliers;
        }
    }
}
