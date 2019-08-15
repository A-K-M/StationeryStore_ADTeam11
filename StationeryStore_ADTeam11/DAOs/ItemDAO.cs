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

        public List<LowStockItemViewModel> GetLowStockItems()
        {
            List<LowStockItemViewModel> items = new List<LowStockItemViewModel>();

            string sql = @"select Top 1 i.ID,c.Name,i.CategoryID,i.Description,i.ThresholdValue,s.Balance, i.ReorderQty,i.UOM
                            from Item as i, Stockcard as s, Category as c
                            where i.ID=s.ItemID and s.Balance<i.ThresholdValue and c.ID=i.CategoryID
                            order by s.Balance asc";

            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    LowStockItemViewModel item = new LowStockItemViewModel()
                    {
                        Id = (string)reader["ID"],
                        CategoryId = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName=reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Threshold = Convert.ToInt32(reader["ThresholdValue"]),
                        ReorderQty = Convert.ToInt32(reader["ReorderQty"]),
                        Uom = reader["UOM"].ToString(),
                        Balance= (int)reader["Balance"]
                    };
                    items.Add(item);

                }
                catch
                {
                    items = null;
                }
            }
            reader.Close();
            connection.Close();

            return items;
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