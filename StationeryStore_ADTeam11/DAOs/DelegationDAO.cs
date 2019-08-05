using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class DelegationDAO : DatabaseConnection
    {
        public List<Delegation> GetDelegationStatus(int empId)
        {
            List<Delegation> delegation = new List<Delegation>();
            string sql = "select StartDate, EndDate from Delegation where EmpID='"+ empId +"'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader != null && reader.Read())
            {
                Delegation del = new Delegation()
                {
                    StartDate = (DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"],
                };

                delegation.Add(del);
            }
            reader.Close();
          
            return delegation;
        }
        public bool InsertDelegation(Delegation del, string deptId) {

            SqlConnection conn = connection;
            SqlTransaction transaction = null;
            SqlParameter param = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand("UPDATE Department SET DelegatedStatus = @status WHERE ID = @id", conn, transaction);
                cmd1.Parameters.AddWithValue("@status", Constant.STATUS_PENDING);
                cmd1.Parameters.AddWithValue("@id",deptId);
                if (cmd1.ExecuteNonQuery() == 0) throw new Exception();

                SqlCommand cmd2 = new SqlCommand("INSERT INTO Delegation (StartDate, EndDate, EmpID) VALUES (@startDate,@endDate,@empId)", conn, transaction);
                cmd2.Parameters.AddWithValue("@startDate", del.StartDate);
                cmd2.Parameters.AddWithValue("@endDate",del.EndDate);
                cmd2.Parameters.AddWithValue("@empId",del.EmployeeId);
                if (cmd2.ExecuteNonQuery() == 0) throw new Exception();

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null) { transaction.Rollback(); }
                return false;
            }
            finally
            {
                conn.Close();

            }
            return true;

        }
    }
}