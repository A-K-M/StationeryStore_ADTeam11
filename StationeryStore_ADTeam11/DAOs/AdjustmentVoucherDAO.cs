
using Microsoft.Ajax.Utilities;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
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


            //List<AdjustmentVoucherVM> result = VoucherItemPriceHelper(voucherList, itemVoucherList, itemsAbove250, employeeList);

            //return result;

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

        public List<AdjustmentVoucherVM> VoucherItemPriceHelper(List<AdjustmentVoucher> voucherList, List<ItemAdjVoucher> itemVoucherList,
            List<Item> itemList, List<Employee> employeeList)
        {
            List<AdjustmentVoucherVM> vouchersVMList = new List<AdjustmentVoucherVM>();
            AdjustmentVoucherVM voucherVM = null;

            Employee employee = null;

            List<int> voucherIdsForManager = (from voucherDetail in itemVoucherList
                                              join items in itemList
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

            //List<AdjustmentVoucherVM> voucherList = new List<AdjustmentVoucherVM>();
            //AdjustmentVoucherVM voucher = null;

            //string sql = "SELECT e.Name, av.VoucherID, av.Date, av.Status, SUM(iav.Qty) AS TotalQuantity " +
            //            "FROM Item i, AdjustmentVoucher av, ItemAdjVoucher iav, Employee e " +
            //            "WHERE i.ID = iav.ItemID " +
            //            "AND iav.VoucherID = av.VoucherID " +
            //            "AND e.ID = av.EmployeeID " +
            //            "AND av.Status = @value " +
            //            "AND(i.FirstPrice + i.SecondPrice + i.ThirdPrice) / 3 >= 250 " +
            //            " GROUP BY e.Name, av.VoucherID, av.Date, av.Status ";

            //SqlCommand cmd = new SqlCommand(sql, connection);

            //connection.Open();
            //cmd.Parameters.Add("@value", SqlDbType.VarChar);
            //cmd.Parameters["@value"].Value = status;

            //SqlDataReader data = cmd.ExecuteReader();

            //while (data.Read())
            //{
            //    voucher = new AdjustmentVoucherVM()
            //    {
            //        Name = data["Name"].ToString(),
            //        Id = Convert.ToInt32(data["VoucherID"]),
            //        Date = Convert.ToDateTime(data["Date"]),
            //        Status = data["Status"].ToString(),
            //        TotalQuantity = Convert.ToInt32(data["TotalQuantity"])
            //    };

            //    voucherList.Add(voucher);
            //}
            //data.Close();
            //connection.Close();

            //return voucherList;
        }

        public int Add(int employeeId)
        {
            DateTime now = DateTime.Now;

            string sqlFormattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string status = Constant.STATUS_PENDING;

            string sql = "INSERT INTO AdjustmentVoucher (EmployeeID, Date, Status) OUTPUT INSERTED.VoucherID VALUES (@employeeId, @date, @status)";

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            cmd.Parameters.Add("@employeeId", SqlDbType.Int);
            cmd.Parameters.Add("@date", SqlDbType.Date);
            cmd.Parameters.Add("@status", SqlDbType.VarChar);

            cmd.Parameters["@employeeId"].Value = employeeId;
            cmd.Parameters["@date"].Value = sqlFormattedDate;
            cmd.Parameters["@status"].Value = status;

            int result = Convert.ToInt32(cmd.ExecuteScalar());

            connection.Close();

            return result;
        }

        public int AddVoucherItems(List<ItemAdjVoucher> itemVouchers, int voucherId)
        {
            string sql = "";

            foreach (var item in itemVouchers)
            {
                sql += $"INSERT INTO ItemAdjVoucher (ItemID, VoucherID, Qty, Reason) VALUES ('{item.ItemId}', {voucherId}, {item.Quantity}, '{item.Reason}'); \n";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            int result = cmd.ExecuteNonQuery();

            connection.Close();

            return result;
        }

        public void DeleteAdjustmentVoucher(int id)
        {
            string sql = "DELETE FROM AdjustmentVoucher WHERE VoucherID=@voucherId";

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public List<VoucherItemVM> GetVoucherItems(int id)
        {
            List<VoucherItemVM> itemList = new List<VoucherItemVM>();

            VoucherItemVM voucherItems = null;

            string sql = "SELECT av.Status, iav.VoucherID, iav.Qty, iav.Reason, i.Description, (i.FirstPrice + i.SecondPrice + i.ThirdPrice) / 3 AS [AVG]  FROM ItemAdjVoucher iav, Item i, AdjustmentVoucher av WHERE iav.VoucherID = @Id AND i.ID = iav.ItemID AND av.VoucherID = iav.VoucherID";

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            cmd.Parameters.Add("@Id", SqlDbType.Int);
            cmd.Parameters["@Id"].Value = id;

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                voucherItems = new VoucherItemVM()
                {

                    VoucherID = Convert.ToInt32(data["VoucherID"]),
                    Status = data["Status"].ToString(),
                    ItemDescription = data["Description"].ToString(),
                    Quantity = Convert.ToInt32(data["Qty"]),
                    Price = Math.Round(Convert.ToDouble(data["AVG"])),
                    Reason = data["Reason"].ToString()
                };

                itemList.Add(voucherItems);
            }

            data.Close();
            connection.Close();

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

        public void ReviewAdjustmentVoucher(int id, string status)
        {
            string sql = "UPDATE AdjustmentVoucher SET Status = @status WHERE VoucherID = @id";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@status", SqlDbType.VarChar);
            cmd.Parameters["@status"].Value = status;

            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = id;

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public List<AdjustmentVoucherVM> GetAdjVoucherByClerk(int clerkId)
        {
            List<AdjustmentVoucherVM> voucherList = new List<AdjustmentVoucherVM>();
            SqlDataReader reader = null;
            try
            {
                connection.Open();

                string sql = " SELECT av.Date,av.Status,av.VoucherID,SUM(iav.Qty) TotalQuantity " +
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

                SqlCommand cmd = new SqlCommand(sql, connection,transaction);
                cmd.Parameters.AddWithValue("@employeeId",clerkId);
                cmd.Parameters.AddWithValue("@date", DateTime.Today);
                cmd.Parameters.AddWithValue("@status",Constant.STATUS_PENDING);
                int voucherID = Convert.ToInt32(cmd.ExecuteScalar());
                 
                sql = "";
                string sqlStockCard = "",temp="";
                foreach (var item in adjItems)
                {
                    sql += $"INSERT INTO ItemAdjVoucher (ItemID, VoucherID, Qty, Reason) VALUES ('{item.ItemId}', {voucherID}, {item.Quantity}, '{item.Reason}'); \n";
                    temp = $" INSERT INTO Stockcard (ItemID,DateTime,Qty,Balance,RefType)" +
                           $" VALUES ('{item.ItemId}','{DateTime.Today}','{item.Quantity}'," +
                           $"(SELECT TOP 1 Balance FROM Stockcard ORDER BY ID DESC)+{item.Quantity}" +
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