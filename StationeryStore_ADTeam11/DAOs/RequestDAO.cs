using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;

namespace StationeryStore_ADTeam11.DAOs
{
    public class RequestDAO:DatabaseConnection
    {
        //Getting Approved Active Requests
        public List<Request> GetRequests(int empId)
        {
            //empId = 11237;

            List<Request> requests = new List<Request>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from Request where EmployeeID='"+empId+"'and Status='Approved'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["DisbursedDate"].GetType().Equals(typeof(DBNull)))
                {
                    Request request = new Request()
                    {
                        Id = (string)reader["ID"],
                        EmployeeId = (int)reader["EmployeeID"],
                        DateTime = (DateTime)reader["DateTime"],
                        Status = (string)reader["Status"]
                };
                    //Console.WriteLine("Hello there");
                    requests.Add(request);
                }
                //else
                //{
                //    Request request = new Request()
                //    {
                //        Id = (string)reader["ID"],
                //        EmployeeId = (int)reader["EmployeeID"],
                //        DateTime = (DateTime)reader["DateTime"],
                //        Status = (string)reader["Status"],
                //        DisbursedDate = (DateTime)reader["DisbursedDate"]
                //    };
                //    requests.Add(request);
                //}               
                
            }
            conn.Close();
            return requests;
        }
        public void UpdateDisbursedDate(ItemRequest request)
        {
            int outstandingQty = request.NeededQty - request.ActualQty;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"update Request set DisbursedDate='"+DateTime.Now+"'where ID='"+request.RequestId+"'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }
}