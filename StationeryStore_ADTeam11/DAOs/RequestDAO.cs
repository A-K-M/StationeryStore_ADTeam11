using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;
using StationeryStore_ADTeam11.View_Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class RequestDAO : DatabaseConnection
    {
        public List<StationeryRequest> GetRequestList()
        {
            List<StationeryRequest> requestlist = new List<StationeryRequest>();
            
           
            string sql = "select R.Status as Status,E.Name as Name,R.DateTIme as Date,R.Id as RequestId from Request R,Employee E where E.ID=R.EmployeeID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                StationeryRequest request = new StationeryRequest()
                {
                    EmpName = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    Status = (string)reader["Status"],
                    RequestId = (string)reader["RequestId"],
                };
                requestlist.Add(request);
            }
            
            return requestlist;
        }

        public List<StationeryRequest> ViewPendingRequestDetails(String requestId)
        {
            List<StationeryRequest> requestlist = new List<StationeryRequest>();

            string sql = "select e.UserName as Name,i.Description as Description ,ir.NeededQty  as Qty,r.ID as RequestId from Employee e, Request r, Item i,ItemRequest ir where e.id = r.EmployeeID and r.ID = ir.RequestID and ir.ItemID = i.ID and r.ID = '" + requestId+"'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                StationeryRequest request = new StationeryRequest()
                {
                    EmpName = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    TotalItem = (int)reader["Qty"],
                    RequestId = (string)reader["RequestId"]
                };
                requestlist.Add(request);
            }
            connection.Close();
            return requestlist;
        }

        public void UpdateStatus(string status, string reqId)
        {
            string sql = "update Request set Status='"+ status +"' where ID='" + reqId +"'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();

        }
    }
}