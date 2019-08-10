using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;
using System.Linq;

namespace StationeryStore_ADTeam11.DAOs
{
    public class StockCardDAO : DatabaseConnection
    {

        public  List<StockCard> GetAllStockCards()
        {
            List<StockCard> stockCards = new List<StockCard>();
         
                connection.Open();

                string sql = @"SELECT * FROM Stockcard";
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    StockCard stockCard = new StockCard()
                    {
                        Id = (int)reader["Id"],
                        ItemId = (string)reader["ItemID"],
                        Date = (DateTime)reader["DateTime"],
                        Qty = (int)reader["Qty"],
                        Balance = (int)reader["Balance"],
                        RefType = (string)reader["Reftype"]
                    };
                    stockCards.Add(stockCard);
                }

                connection.Close();
            

            stockCards = stockCards.OrderByDescending(x => x.Date).ToList();

            return stockCards;
        }


        public List<StockCard> GetStockCardsByItemId(string Id)
        {
            List<StockCard> stockCards = new List<StockCard>();
            connection.Open();

            string sql = @"SELECT * FROM Stockcard WHERE ItemId = '" + Id + "'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                StockCard stockCard;
                try
                {
                    stockCard = new StockCard()
                    {
                        Id = (int)reader["Id"],
                        ItemId = (string)reader["ItemID"],
                        Date = (DateTime)reader["DateTime"],
                        Qty = (int)reader["Qty"],
                        Balance = (int)reader["Balance"],
                        RefType = (string)reader["Reftype"]
                    };
                    stockCards.Add(stockCard);
                }
                catch
                {
                    stockCards = null;
                }
            }
            reader.Close();
            connection.Close();
            

            if (stockCards != null)
            {
                stockCards = stockCards.OrderByDescending(x => x.Id).ToList();
            }

            return stockCards;
        }
    }
}