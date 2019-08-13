using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
                    DepartmentId = (string)reader["DeptID"],
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
        public Employee GetEmployeeById(int Id)
        {
            Employee employee = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from employee where ID = '" + Id + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                employee = new Employee()
                {
                    Id = (int)reader["id"],
                    DepartmentId = (string)reader["DeptID"],
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
        public string GetDepartmentIdByDepartmentHeadUsername(string username)
        {
            string deptId = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select DeptID from Employee where Username='" + username + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                deptId = (string)reader["DeptID"];

            }
            conn.Close();
            return deptId;
        }

        public List<Employee> GetEmployeeByDeptId(string deptId)
        {
            List<Employee> employees = new List<Employee>();
            Employee employee = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from employee where DeptID = '" + deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                employee = new Employee()
                {
                    Id = (int)reader["id"],
                    DepartmentId = (string)reader["DeptID"],
                    Name = (string)reader["Name"],
                    UserName = (string)reader["UserName"],
                    Password = (string)reader["Password"],
                    Email = (string)reader["Email"],
                    Role = (string)reader["Role"]
                };
                employees.Add(employee);
            }
            conn.Close();
            return employees;
        }
        public Employee GetEmployeeByName(string deptId,string name)
        {
            Employee employee = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select ID from Employee where Name = '" + name + "' and DeptID = '" + deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                employee = new Employee()
                {
                    Id = (int)reader["id"]
                };

            }
            conn.Close();
            return employee;

        }



    }
}