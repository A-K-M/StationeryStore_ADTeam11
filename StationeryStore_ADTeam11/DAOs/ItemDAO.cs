﻿using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class ItemDAO : DatabaseConnection
    {
        public List<Item> GetItems(int category_id)
        {
            List<Item> items = new List<Item>();

            Item item = null;

            string sql = "SELECT * FROM Item WHERE CategoryID = @category_id";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@category_id", SqlDbType.Int);
            cmd.Parameters["@category_id"].Value = category_id;

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                item = new Item()
                {
                    Id = Convert.ToInt32(data["ID"]),
                    CategoryId = Convert.ToInt32(data["CategoryID"]),
                    Description = data["Description"].ToString(),
                    ThresholdValue = Convert.ToInt32(data["ThresholdValue"]),
                    ReorderQty = Convert.ToInt32(data["ReorderQty"]),
                    Uom = data["UOM"].ToString(),
                    BinNo = data["BinNo"].ToString(),
                    FirstSupplier = data["FirstSupplier"].ToString(),
                    FirstPrice = Convert.ToDouble(data["FirstPrice"]),
                    SecondSupplier = data["SecondSupplier"].ToString(),
                    SecondPrice = Convert.ToDouble(data["SecondPrice"]),
                    ThirdSupplier = data["ThirdSupplier"].ToString(),
                    ThirdPrice = Convert.ToDouble(data["ThirdPrice"])
                };

                items.Add(item);
            }

            data.Close();

            return items;
        }
    }
}