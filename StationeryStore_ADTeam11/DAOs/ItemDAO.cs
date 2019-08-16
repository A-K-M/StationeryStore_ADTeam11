using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.View_Models;
using StationeryStore_ADTeam11.Models;
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

            if (data.Read())
            {
                item = new Item()
                {
                    Id = data["ID"].ToString(),
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
            connection.Close();

            return items;
        }
        public string GetItemDescription(string id)
        {
            string itemDescription="";
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select Description from Item where ID='" + id + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                itemDescription = (string)reader["Description"];
            }
            conn.Close();
            return itemDescription;
        }

        public List<Item> GetItemIdsAndThresholdValue()
        {
            List<Item> itemList = new List<Item>();

            string sql = @"select id,ThresholdValue from item";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader data = cmd.ExecuteReader();

            Item item = null;

            while (data.Read())
            {
                item = new Item()
                {
                    Id = data["ID"].ToString(),
                    ThresholdValue = Convert.ToInt32(data["ThresholdValue"]),
                };

                itemList.Add(item);
            }
            data.Close();
            connection.Close();

            return itemList;

        }

        public List<Item> GetLowStockItems(string ids)
        {
            List<Item> itemList = new List<Item>();

            Item item = null;

            string sql = @"select i.*, c.name as Category from Item i, Category c 
                    where i.CategoryID = c.ID and 
                    i.ID in (" + ids + ")";


            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                item = new Item()
                {
                    Id = data["ID"].ToString(),
                    CategoryId = Convert.ToInt32(data["CategoryID"]),
                    Description = data["Description"].ToString(),
                    ReorderQty = Convert.ToInt32(data["ReorderQty"]),
                    ThresholdValue = Convert.ToInt32(data["ThresholdValue"]),
                    Uom = data["UOM"].ToString(),
                    BinNo = data["BinNo"].ToString(),
                    CategoryName = data["Category"].ToString()
                };

                itemList.Add(item);
            }
            data.Close();
            connection.Close();

            return itemList;
        }

        public int GetBalanceByItemId(string itemId)
        {
            string sql = "select top 1 balance from " +
                         "Stockcard where ItemID = '"+ itemId + "' order by id desc; ";

            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            int balance = 0;

            while (data.Read())
            {
                balance = Convert.ToInt32(data["balance"]);
            }

            data.Close();
            connection.Close();

            return balance;
        }

        public Item GetItemById(string id)
        {
            Item item = null;

            string sql = "SELECT * FROM Item WHERE ID = '" + id + "'";

            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader data = cmd.ExecuteReader();

            if (data.Read())
            {
                try
                {
                    item = new Item()
                    {
                        Id = data["ID"].ToString(),
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
                }
                catch
                {
                    item = null;
                }
            }
            data.Close();
            connection.Close();

            return item;
        }

        public List<MItemSpinner> GetAllItems()
        {
            List<MItemSpinner> itemList = new List<MItemSpinner>();
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                string sql = "SELECT ID,CategoryID,Description FROM Item ORDER BY CategoryID ASC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                reader = cmd.ExecuteReader();
                MItemSpinner item = null;
                while (reader.Read()) {
                    item = new MItemSpinner()
                    {
                        Id = reader["ID"].ToString(),
                        CategoryId = (int)reader["CategoryID"],
                        Description = reader["Description"].ToString()
                    };

                    itemList.Add(item);
                }
               
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return itemList;
        }


    }
}