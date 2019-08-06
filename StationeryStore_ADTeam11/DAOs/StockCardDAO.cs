using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;
using System.Linq;

namespace StationeryStore_ADTeam11.DAOs
{
    public class StockCardDAO : DAO
    {

        public static List<StockCard> GetAllStockCards()
        {
            List<StockCard> stockCards = new List<StockCard>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT * FROM Stockcard";
                SqlCommand cmd = new SqlCommand(sql, conn);
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

                conn.Close();
            }

            stockCards = stockCards.OrderByDescending(x => x.Date).ToList();

            return stockCards;
        }


        public static List<StockCard> GetStockCardsbyId(string Id)
        {
            List<StockCard> stockCards = new List<StockCard>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT * FROM Stockcard WHERE ItemId = '" + Id + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
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

                conn.Close();
            }

            stockCards = stockCards.OrderByDescending(x => x.Date).ToList();

            return stockCards;
        }
    }
}