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

      
        public bool InsertDelegation(Delegation del, int headId) {

            SqlTransaction transaction = null;
            int newDelegationId = 0;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand("INSERT INTO Delegation (StartDate, EndDate, EmpID) VALUES (@startDate,@endDate,@empId) ; SELECT CAST(scope_identity() AS int) ", connection, transaction);
                cmd1.Parameters.AddWithValue("@startDate", del.StartDate);
                cmd1.Parameters.AddWithValue("@endDate",del.EndDate);
                cmd1.Parameters.AddWithValue("@empId",del.EmployeeId);
                newDelegationId =(Int32) cmd1.ExecuteScalar() ;
                if (newDelegationId == 0) throw new Exception();

                SqlCommand cmd2 = new SqlCommand("UPDATE Department SET DelegateID = @delegateId WHERE HeadID = @id", connection, transaction);
                cmd2.Parameters.AddWithValue("@status", Constant.STATUS_PENDING);
                cmd2.Parameters.AddWithValue("@delegateId",newDelegationId);
                cmd2.Parameters.AddWithValue("@id", headId);
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
                connection.Close();

            }
            return true;

        }
        public bool CancelDelegation(int headId,int delegationId)
        {
            SqlTransaction transaction = null;
                 
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                string sql = @"UPDATE Department SET DelegateID = 0 WHERE HeadID = @headId";
                SqlCommand cmd = new SqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@headId", headId);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                sql = @"UPDATE Delegation SET EndDate = '" + DateTime.Today + "' where ID = @delegationId";
                cmd = new SqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@delegationId", delegationId);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null) transaction.Rollback();
                return false;
            }
            finally {
                if (connection != null) connection.Close();
            }

            return true;
        }
        public List<Delegation> GetDelegations(string deptId)
        {
            List<Delegation> delegations = new List<Delegation>();
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                string sql = @"SELECT e.Name,e.Email,d.ID,d.EmpID,d.StartDate,d.EndDate FROM Delegation d,Employee e
                           WHERE d.EmpID = e.ID AND e.DeptID = @deptId
                           ORDER BY d.ID DESC";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@deptId", deptId);
                 reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Delegation delegation = new Delegation()
                    {
                        Id = (int)reader["ID"],
                        EmployeeId = (int)reader["EmpID"],
                        EmployeeName = (string)reader["Name"],
                        Email = (string)reader["Email"],
                        StartDate = (DateTime)reader["StartDate"],
                        EndDate = (DateTime)reader["EndDate"]
                    };
                    delegations.Add(delegation);
                }
                reader.Close();
                sql = @"SELECT de.ID delegateId,de.EndDate FROM Delegation de,Department d
                        WHERE d.DelegateID = de.ID AND d.ID = @deptId AND d.DelegatedStatus=Ongoing";
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@deptId", deptId);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int currentDelegate = (int)reader["delegateId"];
                    DateTime endDate = (DateTime)reader["EndDate"];
                    reader.Close();
                    if (DateTime.Today.CompareTo(endDate) > 0) {
                        string status = "Completed";
                        sql = "UPDATE Department SET DelegatedStatus = @status WHERE ID = @Id";
                        //sql = "UPDATE Department SET DelegateId = 0 WHERE ID = @Id";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@Id", deptId);
                        cmd.Parameters.AddWithValue("@status", status);
                        int row = cmd.ExecuteNonQuery();
                        if (row == 0) throw new Exception();
                    }
                    else
                    {
                        int index = delegations.FindIndex(d => d.Id == currentDelegate);
                        delegations[index].Status = true;
                    }
                }
            }
            catch
            {
                return null;
            }
            finally {
                if (reader != null && !reader.IsClosed) reader.Close();
                connection.Close();
            }
            return delegations;
        }
        public Delegation GetDelegationById(int id)
        {
            Delegation delegation = new Delegation();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from Delegation where ID='" + id + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                delegation.Id = (int)reader["ID"];
                delegation.EmployeeId = (int)reader["EmpID"];
                delegation.StartDate = (DateTime)reader["StartDate"];
                delegation.EndDate = (DateTime)reader["EndDate"];
            }
            conn.Close();
            return delegation;
        }
        public List<Delegation> GetDelegationsByEmpId(int empId, string deptId)
        {
            List<Delegation> delegations = new List<Delegation>();
            SqlConnection connnection = connection;
            connnection.Open();
            string sql = @"select * from Delegation where EmpID='" + empId + "'";
            SqlCommand command = new SqlCommand(sql, connnection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                           
                while (reader.Read())
                {
                    Delegation delegation = new Delegation()
                    {
                        Id = (int)reader["ID"],
                        EmployeeId = (int)reader["EmpID"],
                        StartDate = (DateTime)reader["StartDate"],
                        EndDate = (DateTime)reader["EndDate"]
                    };
                    if (DateTime.Today.CompareTo(delegation.EndDate) > 0)
                    {
                        string status = "Completed";
                        sql = "UPDATE Department SET DelegatedStatus = @status WHERE ID = @Id";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@Id", deptId);
                        int row = cmd.ExecuteNonQuery();
                        if (row == 0) throw new Exception();
                    }
                    delegations.Add(delegation);
                }
            }
            catch(Exception e)
            {
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                connection.Close();
            }
            return delegations;
        }
        public void CreateDelegation(Delegation delegation)
        {
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"INSERT INTO Delegation(StartDate,EndDate,EmpID,Reason) 
                                VALUES ('" + delegation.StartDate + "', '" + delegation.EndDate + "', '" +
                                delegation.EmployeeId + "', '" + delegation.Reason + "')";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            //Email

        }
        public void CancelDelegation(int delegationId)
        {
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"update Delegation set EndDate = '" + DateTime.Now + "' where ID = " + delegationId;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();

        }

      

    }
}