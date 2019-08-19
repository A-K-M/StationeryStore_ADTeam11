using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class EmailDAO:DatabaseConnection
    {
        private string email;

        public string EmailReviewStationeryRequest(int empId)
        {
            string sql = "select Email from Employee where Role='Head' and DeptID = (select DeptID from Employee where ID = " + empId + ")";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                email = reader["Email"].ToString();
            }
            reader.Close();
            return email;
        }

        public Employee EmailRequestStatus(int reqId)
        {
            SqlDataReader reader = null;
            Employee employee = null;

            try
            {
                connection.Open();
                string sql = "SELECT Email,Name from Employee where ID = (select EmployeeID from Request where ID =" + reqId + ")";
                SqlCommand cmd = new SqlCommand(sql, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employee = new Employee()
                    {
                        Email = reader["Email"].ToString(),
                        Name = reader["Name"].ToString(),
                    };
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return employee;
        }

        public Employee EmailUpdateDepartmentRep(int pointID)
        {
            Employee employee = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                string sql = "  select e.Email ,e.Name from Employee as e, CollectionPoint as c where e.ID = c.EmpID and c.ID = '" + pointID + "'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employee = new Employee()
                    {
                        Email = reader["Email"].ToString(),
                        Name = reader["Name"].ToString(),
                    };
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return employee;
        }

        public Employee EmailDelegation(int empID)
        {
            SqlDataReader reader = null;
            Employee employee = null;

            try
            {
                connection.Open();
                string sql = "select Email,Name from Employee where Employee.ID = " + empID;
                SqlCommand cmd = new SqlCommand(sql, connection);
                reader  = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employee = new Employee()
                    {
                        Email = reader["Email"].ToString(),
                        Name = reader["Name"].ToString(),
                    };
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return employee;
        }

    }
}