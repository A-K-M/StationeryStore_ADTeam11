using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;

namespace StationeryStore_ADTeam11.DAOs
{
    public class DepartmentDAO:DatabaseConnection
    {
        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select ID from Department where not ID='STOR'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Department department = new Department()
                {
                    Id = (string)reader["ID"]
                };

                departments.Add(department);
            }
            conn.Close();
            return departments;
        }

        public Department GetDepartmentByDeptId(string Id)
        {
            Department department = new Department();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from Department where ID='"+Id+"'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                department = new Department()
                {
                    Id = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    ContactId = (int)reader["ContactID"],
                    HeadId = (int)reader["HeadID"],
                    RepId = (int)reader["RepID"],
                    DelegationId = (int)reader["DelegateID"],
                    CollectionPoinId = (int)reader["CollectionPointID"],
                    DelegateStatus = (string)reader["DelegatedStatus"]
                };
            }
            conn.Close();
            return department;
        }

        public List<String> GetCollectionPointByDeptId(string deptId)
        {
            List<String> CollectionPoint = new List<string>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select d.Name as DeptName, c.Address as CollectionPoint,e.Name as ClerkName
                            from CollectionPoint as c, Department as d, Employee as e
                            where d.CollectionPointID=c.ID and c.EmpID=e.ID and d.ID='"+ deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                CollectionPoint.Add((string)reader["DeptName"]);
                CollectionPoint.Add((string)reader["CollectionPoint"]);
                CollectionPoint.Add((string)reader["ClerkName"]);
            }
            conn.Close();
            return CollectionPoint;
        }

        public void UpdateDepartmentCollectionPoint(string deptId,int cpId)
        {
            Department department = new Department();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"UPDATE Department SET CollectionPointID ='"+ cpId+ "' WHERE  ID='" + deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public bool UpdateDeptRepAndColPt(string deptId, int repId, int cpId)
        {
            //Department department = new Department();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"UPDATE Department SET RepID ='" + repId + "', CollectionPointID ='" + cpId + "' WHERE  ID='" + deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            if (command.ExecuteNonQuery() == 0)
            {
                connection.Close();
                return false;
            }

            connection.Close();
            return true;
        }

        //public void UpdateDepartmentDelegation(string deptId, int empId,string status)
        //{
        //    Department department = new Department();
        //    SqlConnection conn = connection;
        //    conn.Open();
        //    string sql = @"UPDATE Department SET DelegateID ='" + empId + "', DelegatedStatus ='"+status+"' WHERE  ID='" + deptId + "'";
        //    SqlCommand command = new SqlCommand(sql, conn);
        //    command.ExecuteNonQuery();
        //    conn.Close();
        //}

        public bool InsertDelegation(Delegation del, string deptId)
        {

            SqlTransaction transaction = null;
            int newDelegationId = 0;
            string status = "Ongoing";

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand("INSERT INTO Delegation (StartDate, EndDate, EmpID,Reason) VALUES (@startDate,@endDate,@empId,@reason) ; SELECT CAST(scope_identity() AS int) ", connection, transaction);
                cmd1.Parameters.AddWithValue("@startDate", del.StartDate);
                cmd1.Parameters.AddWithValue("@endDate", del.EndDate);
                cmd1.Parameters.AddWithValue("@empId", del.EmployeeId);
                cmd1.Parameters.AddWithValue("@reason", del.Reason);
                newDelegationId = (Int32)cmd1.ExecuteScalar();
                if (newDelegationId == 0) throw new Exception();

                SqlCommand cmd2 = new SqlCommand("UPDATE Department SET DelegateID = @delegateId, DelegatedStatus=@status WHERE ID = @id", connection, transaction);
                cmd2.Parameters.AddWithValue("@status",status );
                cmd2.Parameters.AddWithValue("@delegateId", newDelegationId);
                cmd2.Parameters.AddWithValue("@id", deptId);
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

        public bool CancelDelegation(string deptId, int delegationId)
        {
            SqlTransaction transaction = null;
            string status = "Completed";

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                string sql = @"UPDATE Department SET DelegatedStatus =@status  WHERE ID = @deptId";

                //string sql = @"UPDATE Department SET DelegateID = 0 WHERE ID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.Parameters.AddWithValue("@status", status);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();

                sql = @"UPDATE Delegation SET EndDate = '" + DateTime.Now + "' where ID = @delegationId";
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
            finally
            {
                if (connection != null) connection.Close();
            }

            return true;
        }
    }
}