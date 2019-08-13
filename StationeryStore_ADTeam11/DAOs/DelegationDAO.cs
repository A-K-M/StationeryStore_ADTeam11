using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;

namespace StationeryStore_ADTeam11.DAOs
{
    public class DelegationDAO:DatabaseConnection
    {
        public List<Delegation> GetDelegations()
        {
            List<Delegation> delegations = new List<Delegation>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from Delegation";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Delegation delegation = new Delegation()
                {
                    Id=(int)reader["ID"],
                    EmployeeId=(int)reader["EmpID"],
                    StartDate=(DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"]
                };
                delegations.Add(delegation);
            }
            conn.Close();
            return delegations;
        }
        public Delegation GetDelegationById(int id)
        {
            Delegation delegation = new Delegation();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from Delegation where ID='"+id+"'";
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
        public List<Delegation> GetDelegationsByEmpId(int empId)
        {
            List<Delegation> delegations = new List<Delegation>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from Delegation where EmpID='"+empId+"'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Delegation delegation = new Delegation()
                {
                    Id = (int)reader["ID"],
                    EmployeeId = (int)reader["EmpID"],
                    StartDate = (DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"]
                };
                delegations.Add(delegation);
            }
            conn.Close();
            return delegations;
        }
        public void CreateDelegation(Delegation delegation)
        {
            SqlConnection conn = connection;
            conn.Open();           
            string sql = @"INSERT INTO Delegation(StartDate,EndDate,EmpID,Reason) 
                                VALUES ('" + delegation.StartDate + "', '" + delegation.EndDate + "', '" + 
                                delegation.EmployeeId + "', '" +  delegation.Reason + "')";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            
        }
        public void CancelDelegation(int delegationId)
        {
            SqlConnection conn = connection;
            conn.Open();
            string sql=@"update Delegation set EndDate = '" + DateTime.Now + "' where ID = " + delegationId;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();

        }
    }
}