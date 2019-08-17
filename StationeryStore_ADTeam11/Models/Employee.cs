using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.MobileModels;

namespace StationeryStore_ADTeam11.Models
{
    public class Employee
    {
        private int _id;
        private string _departmentId;
        private string _name;
        private string _userName;
        private string _password;
        private string _email;
        private string _role;

        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        internal static Employee mapToOBj(SqlDataReader reader)
        {
            Employee employee = null;
            try
            {
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
            }
            catch
            {
                return null;
            }
            finally {
               if(reader != null) reader.Close();
            }
            return employee;
        }
    }


}