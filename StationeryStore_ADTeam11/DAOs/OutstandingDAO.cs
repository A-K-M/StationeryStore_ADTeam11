﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;

namespace StationeryStore_ADTeam11.DAOs
{
    public class OutstandingDAO:DatabaseConnection
    {
        public void AddItemRequest(ItemRequest request)
        {
            int outstandingQty = request.NeededQty - request.ActualQty;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"insert into Outstanding(ItemRequestID,Qty,Status)
                            values ("+request.Id+","+outstandingQty+",'pending')";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }


         public int GetPendingOutstandingCount()
        {

            int count = 0;
            connection.Open();
            string sql = @"select count(*) from Outstanding where status = 'Pending'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            count = (int)cmd.ExecuteScalar();

            return count;
        }
    }
}