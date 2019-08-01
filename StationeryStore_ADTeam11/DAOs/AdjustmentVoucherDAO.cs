using StationeryStore_ADTeam11.Models;
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
    }
}