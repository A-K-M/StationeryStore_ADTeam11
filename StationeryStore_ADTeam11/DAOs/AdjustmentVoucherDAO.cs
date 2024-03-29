﻿
using Microsoft.Ajax.Utilities;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.Util;
using StationeryStore_ADTeam11.View_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class AdjustmentVoucherDAO : DatabaseConnection
    {
        public List<AdjustmentVoucherVM> GetByStatus(string status)
        {

            List<AdjustmentVoucher> voucherList = new List<AdjustmentVoucher>();
            AdjustmentVoucher av = null;

            List<ItemAdjVoucher> itemVoucherList = new List<ItemAdjVoucher>();
            ItemAdjVoucher iav = null;

            List<Item> itemList = new List<Item>();
            Item i = null;

            List<Employee> employeeList = new List<Employee>();
            Employee employee = null;

            string sql = "SELECT av.*, iav.ID AS [ItemAdjID], iav.ItemID, iav.VoucherID AS [ItemAdjVoucherID], iav.Qty, iav.Reason, i.*, e.ID AS [EmpId], e.Name AS [EmpName] " +
                          "FROM AdjustmentVoucher av, Item i, ItemAdjVoucher iav, Employee e " +
                          "WHERE av.VoucherID = iav.VoucherID " +
                          "AND av.Status=@value " +
                          "AND e.ID = av.EmployeeID " +
                          "AND iav.ItemID = i.ID";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@value", SqlDbType.VarChar);
            cmd.Parameters["@value"].Value = status;

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                av = new AdjustmentVoucher()
                {
                    Id = Convert.ToInt32(data["VoucherID"]),
                    EmployeeId = Convert.ToInt32(data["EmployeeID"]),
                    Date = Convert.ToDateTime(data["Date"]),
                    Status = data["Status"].ToString()
                };

                iav = new ItemAdjVoucher()
                {
                    Id = Convert.ToInt32(data["ItemAdjVoucherID"]),
                    ItemId = data["ItemID"].ToString(),
                    VoucherId = Convert.ToInt32(data["ItemAdjVoucherID"]),
                    Quantity = Convert.ToInt32(data["Qty"]),
                    Reason = data["Reason"].ToString()
                };

                i = new Item()
                {
                    Id = data["ID"].ToString(),
                    CategoryId = Convert.ToInt32(data["CategoryID"]),
                    Description = data["Description"].ToString(),
                    ThresholdValue = Convert.ToInt32(data["ThresholdValue"]),
                    ReorderQty = Convert.ToInt32(data["ReorderQty"]),
                    Uom = data["UOM"].ToString(),
                    BinNo = data["BinNo"].ToString(),
                    FirstSupplier = data["FirstSupplier"].ToString(),
                    FirstPrice = Convert.ToDouble(data["FirstPrice"]),
                    SecondSupplier = data["SecondSupplier"].ToString(),
                    SecondPrice = Convert.ToDouble(data["SecondPrice"]),
                    ThirdSupplier = data["ThirdSupplier"].ToString(),
                    ThirdPrice = Convert.ToDouble(data["ThirdPrice"]),
                };

                employee = new Employee()
                {
                    Id = Convert.ToInt32(data["EmpId"]),
                    Name = data["EmpName"].ToString()
                };

                voucherList.Add(av);
                itemVoucherList.Add(iav);
                itemList.Add(i);
                employeeList.Add(employee);
            }

            data.Close();
            connection.Close();

            List<Item> itemsAbove250 = new List<Item>();

            itemsAbove250 = itemList.Where(x => (x.FirstPrice + x.SecondPrice + x.ThirdPrice) / 3 >= 250).ToList();

            List<AdjustmentVoucherVM> vouchersVMList = new List<AdjustmentVoucherVM>();
            AdjustmentVoucherVM voucherVM = null;
            
            List<int> voucherIdsForManager = (from voucherDetail in itemVoucherList
                                              join items in itemsAbove250
                                              on voucherDetail.ItemId equals items.Id
                                              select voucherDetail.VoucherId).Distinct().ToList();

            foreach (int id in voucherIdsForManager)
            {
                voucherList.RemoveAll(x => x.Id == id);
            }

            voucherList = voucherList.GroupBy(x => x.Id)
              .Select(y => y.First())
              .ToList();

            foreach (AdjustmentVoucher voucher in voucherList)
            {
                employee = employeeList.Find(x => x.Id == voucher.EmployeeId);

                voucherVM = new AdjustmentVoucherVM()
                {
                    Name = employee.Name,
                    Id = voucher.Id,
                    Date = voucher.Date,
                    Status = voucher.Status,
                    TotalQuantity = itemVoucherList.Where(x => x.VoucherId == voucher.Id)
                                    .Sum(y => y.Quantity)
                };

                vouchersVMList.Add(voucherVM);
            }

            return vouchersVMList;
        }

        public List<AdjustmentVoucherVM> GetByStatusForManager(string status)
        {
            List<AdjustmentVoucher> voucherList = new List<AdjustmentVoucher>();
            AdjustmentVoucher av = null;

            List<ItemAdjVoucher> itemVoucherList = new List<ItemAdjVoucher>();
            ItemAdjVoucher iav = null;

            List<Item> itemList = new List<Item>();
            Item i = null;

            List<Employee> employeeList = new List<Employee>();
            Employee employee = null;

            string sql = "SELECT av.*, iav.ID AS [ItemAdjID], iav.ItemID, iav.VoucherID AS [ItemAdjVoucherID], iav.Qty, iav.Reason, i.*, e.ID AS [EmpId], e.Name AS [EmpName] " +
                          "FROM AdjustmentVoucher av, Item i, ItemAdjVoucher iav, Employee e " +
                          "WHERE av.VoucherID = iav.VoucherID " +
                          "AND av.Status=@value " +
                          "AND e.ID = av.EmployeeID " +
                          "AND iav.ItemID = i.ID";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@value", SqlDbType.VarChar);
            cmd.Parameters["@value"].Value = status;

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                av = new AdjustmentVoucher()
                {
                    Id = Convert.ToInt32(data["VoucherID"]),
                    EmployeeId = Convert.ToInt32(data["EmployeeID"]),
                    Date = Convert.ToDateTime(data["Date"]),
                    Status = data["Status"].ToString()
                };

                iav = new ItemAdjVoucher()
                {
                    Id = Convert.ToInt32(data["ItemAdjVoucherID"]),
                    ItemId = data["ItemID"].ToString(),
                    VoucherId = Convert.ToInt32(data["ItemAdjVoucherID"]),
                    Quantity = Convert.ToInt32(data["Qty"]),
                    Reason = data["Reason"].ToString()
                };

                i = new Item()
                {
                    Id = data["ID"].ToString(),
                    CategoryId = Convert.ToInt32(data["CategoryID"]),
                    Description = data["Description"].ToString(),
                    ThresholdValue = Convert.ToInt32(data["ThresholdValue"]),
                    ReorderQty = Convert.ToInt32(data["ReorderQty"]),
                    Uom = data["UOM"].ToString(),
                    BinNo = data["BinNo"].ToString(),
                    FirstSupplier = data["FirstSupplier"].ToString(),
                    FirstPrice = Convert.ToDouble(data["FirstPrice"]),
                    SecondSupplier = data["SecondSupplier"].ToString(),
                    SecondPrice = Convert.ToDouble(data["SecondPrice"]),
                    ThirdSupplier = data["ThirdSupplier"].ToString(),
                    ThirdPrice = Convert.ToDouble(data["ThirdPrice"]),
                };

                employee = new Employee()
                {
                    Id = Convert.ToInt32(data["EmpId"]),
                    Name = data["EmpName"].ToString()
                };

                voucherList.Add(av);
                itemVoucherList.Add(iav);
                itemList.Add(i);
                employeeList.Add(employee);
            }

            data.Close();
            connection.Close();

            voucherList = (from vouchers in voucherList
                          join voucherDetails in itemVoucherList
                          on vouchers.Id equals voucherDetails.VoucherId
                          join item in itemList
                          on voucherDetails.ItemId equals item.Id
                          where (item.FirstPrice + item.SecondPrice + item.ThirdPrice) / 3 > 250
                          select vouchers).ToList();

            voucherList = voucherList.GroupBy(x => x.Id)
                          .Select(y => y.First())
                          .ToList();

            List<AdjustmentVoucherVM> vouchersVMList = new List<AdjustmentVoucherVM>();
            AdjustmentVoucherVM voucherVM = null;

            foreach (AdjustmentVoucher voucher in voucherList)
            {
                employee = employeeList.Find(x => x.Id == voucher.EmployeeId);

                voucherVM = new AdjustmentVoucherVM()
                {
                    Name = employee.Name,
                    Id = voucher.Id,
                    Date = voucher.Date,
                    Status = voucher.Status,
                    TotalQuantity = itemVoucherList.Where(x => x.VoucherId == voucher.Id)
                                    .Sum(y => y.Quantity)
                };

                vouchersVMList.Add(voucherVM);
            }

            return vouchersVMList;
        }

        public bool Add(int employeeId, List<ItemAdjVoucher> itemAdjVouchers)
        {

            string sqlFormattedDate = DateUtils.Now();

            string status = Constant.STATUS_PENDING;

            SqlTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                string sql = "INSERT INTO AdjustmentVoucher (EmployeeID, Date, Status) OUTPUT INSERTED.VoucherID VALUES (@employeeId, @date, @status)";

                SqlCommand cmd = new SqlCommand(sql, connection, transaction);

                cmd.Parameters.Add("@employeeId", SqlDbType.Int);
                cmd.Parameters.Add("@date", SqlDbType.Date);
                cmd.Parameters.Add("@status", SqlDbType.VarChar);

                cmd.Parameters["@employeeId"].Value = employeeId;
                cmd.Parameters["@date"].Value = sqlFormattedDate;
                cmd.Parameters["@status"].Value = status;

                int voucherID = Convert.ToInt32(cmd.ExecuteScalar());


                // NEW LINES FROM HERE

                sql = "";
                string sqlStockCard = "", temp = "";

                foreach (var item in itemAdjVouchers)
                {
                    sql += $"INSERT INTO ItemAdjVoucher (ItemID, VoucherID, Qty, Reason) VALUES ('{item.ItemId}', {voucherID}, {item.Quantity}, '{item.Reason}'); \n";
                    temp = $" INSERT INTO Stockcard (ItemID,DateTime,Qty,Balance,RefType)" +
                           $" VALUES ('{item.ItemId}','{sqlFormattedDate}','{item.Quantity}'," +
                           $"(SELECT TOP 1 Balance FROM Stockcard WHERE ItemID='{item.ItemId}' ORDER BY ID DESC)+{item.Quantity}" +
                           $",'ADJ-{voucherID}'); \n";
                    sqlStockCard += temp;
                }

                cmd = new SqlCommand(sql, connection, transaction);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                cmd = new SqlCommand(sqlStockCard, connection, transaction);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                transaction.Commit();

                //NEW LINES END HERE     
            }
            catch (Exception e)
            {
                if(transaction != null)transaction.Rollback();
                return false;
            }
            finally
            {
                connection.Close();
            }

            return true;      
        }

        public void DeleteAdjustmentVoucher(int id)
        {
            try
            {
                string sql = "DELETE FROM AdjustmentVoucher WHERE VoucherID=@voucherId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();

                cmd.ExecuteNonQuery();
            }
            finally {
                connection.Close();
            }

        }

        public List<VoucherItemVM> GetVoucherItems(int id)
        {
            List<VoucherItemVM> itemList = new List<VoucherItemVM>();

            VoucherItemVM voucherItems = null;
            SqlDataReader data = null;
            try
            {
                string sql = "SELECT av.Status, iav.VoucherID, iav.Qty, iav.Reason, i.Description, iav.ItemID, (i.FirstPrice + i.SecondPrice + i.ThirdPrice) / 3 AS [AVG]  FROM ItemAdjVoucher iav, Item i, AdjustmentVoucher av WHERE iav.VoucherID = @Id AND i.ID = iav.ItemID AND av.VoucherID = iav.VoucherID";

                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();

                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;

                data = cmd.ExecuteReader();

                while (data.Read())
                {
                    voucherItems = new VoucherItemVM()
                    {

                        VoucherID = Convert.ToInt32(data["VoucherID"]),
                        ItemID = data["ItemID"].ToString(),
                        Status = data["Status"].ToString(),
                        ItemDescription = data["Description"].ToString(),
                        Quantity = Convert.ToInt32(data["Qty"]),
                        Price = Math.Round(Convert.ToDouble(data["AVG"])),
                        Reason = data["Reason"].ToString()
                    };

                    itemList.Add(voucherItems);
                }
            }
            finally {
                if(data != null) data.Close();
                connection.Close();
            }
            
            return itemList;
        }

        public List<MAdjustmentItem> GetAdjVoucherItems(int voucherID)
        {
            List<MAdjustmentItem> itemList = new List<MAdjustmentItem>();
            SqlDataReader data = null;

            MAdjustmentItem voucherItems = null;
            try
            {

                string sql = "SELECT iav.ItemID, iav.Qty, iav.Reason, i.Description" +
                             " FROM ItemAdjVoucher iav, Item i " +
                            "WHERE iav.VoucherID = @Id AND i.ID = iav.ItemID";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Id", voucherID);

                data = cmd.ExecuteReader();

                while (data.Read())
                {
                    voucherItems = new MAdjustmentItem()
                    {
                        ItemId = data["ItemID"].ToString(),
                        Description = data["Description"].ToString(),
                        Quantity = Convert.ToInt32(data["Qty"]),
                        Reason = data["Reason"].ToString()
                    };

                    itemList.Add(voucherItems);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally {
                if(data != null) data.Close();
                connection.Close();
            }
            

            return itemList;
        }

        public bool ReviewAdjustmentVoucher(int id, string status, List<VoucherItemVM> voucherItems)
        {
            SqlTransaction transaction = null;
            try
            {
                string sql = "UPDATE AdjustmentVoucher SET Status = @status WHERE VoucherID = @id";

                if (status == "Approved")
                {
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.Add("@status", SqlDbType.VarChar);
                    cmd.Parameters["@status"].Value = status;

                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters["@id"].Value = id;

                    connection.Open();

                    if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                    //connection.Close();

                    return true;
                }
                else if (status == "Rejected")
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    SqlCommand cmd = new SqlCommand(sql, connection, transaction);

                    cmd.Parameters.Add("@status", SqlDbType.VarChar);
                    cmd.Parameters["@status"].Value = status;

                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters["@id"].Value = id;

                    cmd.ExecuteNonQuery();

                    string stockCardSql = "";
                    string now = DateUtils.Now();
                    foreach (var item in voucherItems)
                    {
                        stockCardSql += $" INSERT INTO Stockcard (ItemID,DateTime,Qty,Balance,RefType)" +
                           $" VALUES ('{item.ItemID}','{now}','{-item.Quantity}'," +
                           $"(SELECT TOP 1 Balance FROM Stockcard WHERE ItemID='{item.ItemID}' ORDER BY ID DESC)-({item.Quantity})" +
                           $",'ADJ-{id}'); \n";
                    }

                    cmd = new SqlCommand(stockCardSql, connection, transaction);

                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                else
                {
                    return false;
                }
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

        public List<AdjustmentVoucherVM> GetAdjVoucherByClerk(int clerkId)
        {
            List<AdjustmentVoucherVM> voucherList = new List<AdjustmentVoucherVM>();
            SqlDataReader reader = null;
            try
            {
                connection.Open();

                string sql = " SELECT av.Date,av.Status,av.VoucherID,SUM(ABS(iav.Qty)) TotalQuantity " +
                             " FROM AdjustmentVoucher av, ItemAdjVoucher iav "+
                             " WHERE av.VoucherID = iav.VoucherID "+
                             " AND av.EmployeeID = @clerkId "+
                             " GROUP BY av.VoucherID,av.Date,av.Status ";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@clerkId", clerkId);
                AdjustmentVoucherVM adjVM = null;
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    adjVM = new AdjustmentVoucherVM()
                    {
                        Id = (int)reader["VoucherID"],
                        Date = (DateTime)reader["Date"],
                        Status = reader["Status"].ToString(),
                        TotalQuantity = (int)reader["TotalQuantity"]
                    };
                    voucherList.Add(adjVM);
                }

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }

            voucherList = voucherList.OrderByDescending(x => x.Id).ToList();

            return voucherList;
        }

        public bool CreateAdjVoucher(int clerkId,List<MAdjustmentItem> adjItems) {   
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                string sql = "INSERT INTO AdjustmentVoucher (EmployeeID, Date, Status) OUTPUT INSERTED.VoucherID VALUES (@employeeId, @date, @status)";

                string now = DateUtils.Now();

                SqlCommand cmd = new SqlCommand(sql, connection,transaction);
                cmd.Parameters.AddWithValue("@employeeId",clerkId);
                cmd.Parameters.AddWithValue("@date", now);
                cmd.Parameters.AddWithValue("@status",Constant.STATUS_PENDING);
                int voucherID = Convert.ToInt32(cmd.ExecuteScalar());
                 
                sql = "";
                string sqlStockCard = "",temp="";
                foreach (var item in adjItems)
                {
                    sql += $"INSERT INTO ItemAdjVoucher (ItemID, VoucherID, Qty, Reason) VALUES ('{item.ItemId}', {voucherID}, {item.Quantity}, '{item.Reason}'); \n";
                    temp = $" INSERT INTO Stockcard (ItemID,DateTime,Qty,Balance,RefType)" +
                           $" VALUES ('{item.ItemId}','{now}','{item.Quantity}'," +
                           $"(SELECT TOP 1 Balance FROM Stockcard WHERE ItemID='{item.ItemId}' ORDER BY ID DESC)+{item.Quantity}" +
                           $",'ADJ-{voucherID}'); \n";
                    sqlStockCard += temp;
                }

               cmd = new SqlCommand(sql, connection,transaction);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                cmd = new SqlCommand(sqlStockCard, connection, transaction);
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
    }
}