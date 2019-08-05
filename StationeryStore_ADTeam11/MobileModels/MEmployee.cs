using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MEmployee 
    {
        private int _id;
        private string _departmentId;
        private string _name;
        private string _email;

        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<MEmployee> mapToList(SqlDataReader reader)
        {
            List<MEmployee> mEmployees = new List<MEmployee>();

            while (reader != null && reader.Read())
            {

                mEmployees.Add(new MEmployee()
                {
                    Id = (int)reader["id"],
                    DepartmentId = (string)reader["deptID"],
                    Name = (string)reader["Name"],
                    Email = (string)reader["Email"]
                });

            }
            return mEmployees;
        }
    }
}