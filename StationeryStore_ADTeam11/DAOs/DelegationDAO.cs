using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
            return delegation;
        }
    }
}