
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class AdjustmentVoucherDAO : DatabaseConnection
    {
        public List<AdjustmentVoucherViewModel> GetByStatus(string status)
        {
            List<AdjustmentVoucherViewModel> vouchers = new List<AdjustmentVoucherViewModel>();

            AdjustmentVoucherViewModel adjustmentVoucher = null;

            string sql = "SELECT e.Name, av.VoucherID, av.Date, av.Status, SUM(iav.Qty) as TotalQuantity" +
                          "  FROM Employee e, AdjustmentVoucher av, ItemAdjVoucher iav" +
                          " WHERE av.Status = @value" +
                          "  AND av.EmployeeID = e.ID" + 
                          " AND av.VoucherID = iav.VoucherID" +
                          " GROUP BY e.Name, av.VoucherID, av.Date, av.Status";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@value", SqlDbType.VarChar);
            cmd.Parameters["@value"].Value = status;

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                adjustmentVoucher = new AdjustmentVoucherViewModel() {
                    Name = data["Name"].ToString(),
                    Id = Convert.ToInt32(data["VoucherID"]),
                    Date = Convert.ToDateTime(data["Date"]),
                    Status = data["Status"].ToString(),
                    TotalQuantity = Convert.ToInt32(data["TotalQuantity"])
                };

                vouchers.Add(adjustmentVoucher);
            }

            data.Close();
            connection.Close();

            return vouchers;
        }

        public int Add(int employeeId)
        {
            DateTime now = DateTime.Now;

            string sqlFormattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string status = "Pending";

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

            string sql = "SELECT iav.VoucherID, iav.Qty, iav.Reason, i.Description FROM ItemAdjVoucher iav, Item i WHERE VoucherID = @Id AND i.ID = iav.ItemID";

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            cmd.Parameters.Add("@Id", SqlDbType.Int);
            cmd.Parameters["@Id"].Value = id;

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                voucherItems = new VoucherItemVM() {

                    ItemDescription = data["Description"].ToString(),
                    Quantity = Convert.ToInt32(data["Qty"]),
                    Reason = data["Reason"].ToString()
                };

                itemList.Add(voucherItems);
            }

            data.Close();
            connection.Close();

            return itemList;
        }
    }
}