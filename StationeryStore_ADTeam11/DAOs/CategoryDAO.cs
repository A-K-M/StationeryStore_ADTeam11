using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class CategoryDAO : DatabaseConnection
    {
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            Category category = null;

            string sql = "SELECT * FROM Category";

            SqlCommand cmd = new SqlCommand(sql, connection);

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                category = new Category()
                {
                    Id = data["ID"].ToString(),
                    Name = data["Name"].ToString()
                };

                categories.Add(category);
            }

            data.Close();
            return categories;
        }
    }
}