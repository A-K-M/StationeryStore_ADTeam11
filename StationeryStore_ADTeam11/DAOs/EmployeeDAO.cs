using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class EmployeeDAO : DatabaseConnection 
    {
        public Employee GetEmployeeByUsername(string username)
        {
            Employee employee = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select id,DeptID,Name,UserName,Password,Email,Role from employee where UserName = '" + username + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                employee = new Employee()
                {
                    Id = (int)reader["id"],
                    DepartmentId = (string)reader["deptID"],
                    Name = (string)reader["Name"],
                    UserName = (string)reader["UserName"],
                    Password = (string)reader["Password"],
                    Email = (string)reader["Email"],
                    Role = (string)reader["Role"]
                };
                
            }
            conn.Close();
            return employee;

        }
        public List<MEmployee> GetEmployeeByDepartment(string deptId)
        {
            List<MEmployee> mEmployees = new List<MEmployee>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"SELECT id,DeptID,Name,Email FROM employee where DeptID = @deptId";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@deptId", deptId);
            SqlDataReader reader = cmd.ExecuteReader();
            mEmployees = MEmployee.MapToList(reader);
            reader.Close();
            conn.Close();
            return mEmployees;


        }
        public bool checkEmployeeExist(int empId, string deptId) {

            try
            {
                connection.Open();
                string sql = @"SELECT COUNT(*) FROM Employee WHERE ID = @empId AND DeptID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                if ((int)cmd.ExecuteScalar() == 0) throw new Exception();
            }
            catch (Exception e)
            {
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