using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class CategoryDAO : DatabaseConnection
    {
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            Category category = null;

            string sql = "SELECT * FROM Category ORDER BY ID ASC";

            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                 reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    category = new Category()
                    {
                        Id = (int)reader["ID"],
                        Name = reader["Name"].ToString()
                    };

                    categories.Add(category);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return categories;
        }


        public void AddCategory(String name)
        {
            string sql = "insert into Category values('" + name + "')";
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            finally {
                connection.Close();
            }
        }

        public List<Category> GetAllCategoryName()
        {
            List<Category> categories = new List<Category>();
            Category category = null;
            string sql = "SELECT Name FROM Category";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader data = null;
            try
            {
                connection.Open();
                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    category = new Category()
                    {
                        Name = data["Name"].ToString()
                    };
                    categories.Add(category);

                }
            }
            finally
            {
                data.Close();
                connection.Close();
            }

            return categories;
        }

    }
}