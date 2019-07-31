﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    // IMPORTANT - DO NOT COMMIT THIS FILE TO REPOSITORY 
    // CHANGE _connectionString ACCORDING TO YOUR SETTINGS
    public class DatabaseConnection
    {
        protected SqlConnection connection;
        private string _connectionString = "SERVER=DESKTOP-KO5CS62; DATABASE=StationeryInventory; Integrated Security=true";

        public SqlConnection GetConnection()
        {
            connection = new SqlConnection(_connectionString);
            return connection;
        }
       
    }
}

