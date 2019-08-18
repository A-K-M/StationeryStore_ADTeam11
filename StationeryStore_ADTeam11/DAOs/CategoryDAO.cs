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

            connection.Open();

            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                category = new Category()
                {
                    Id = (int)data["ID"],
                    Name = data["Name"].ToString()
                };

                categories.Add(category);
            }

            data.Close();
            return categories;
        }

        //DELETE FROM HERE IF SOMETHING WENT WRONG

        public void AddCategory(String name)
        {
            string sql = "insert into Category values('" + name + "')";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
        }

        public List<Category> GetAllCategoryName()
        {
            List<Category> categories = new List<Category>();
            Category category = null;
            string sql = "SELECT Name FROM Category";
            SqlCommand cmd = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader data = cmd.ExecuteReader();
            while (data.Read())
            {
                category = new Category()
                {
                    Name = data["Name"].ToString()
                };
                categories.Add(category);

            }
            data.Close();
            return categories;
        }

        //NANT MOE'S CODE ENDED HERE
    }
}