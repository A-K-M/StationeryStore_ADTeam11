using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class CollectionPoint
    {
        private int _id;
        private string _name;
        private string _clerkId;
        private string _address;
        private string _collectionTime;
        public int Id { get; set; }

        public string Name { get; set; }

        public string ClerkId { get; set;  }

        public string Address { get; set; }

        public string CollectionTime { get; set; }

        public static List<CollectionPoint> MapToList(SqlDataReader reader) {
            List<CollectionPoint> points = new List<CollectionPoint>();
            while (reader.Read())
            {
                points.Add(new CollectionPoint()
                {
                    Id = (int)reader["ID"],
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    CollectionTime = reader["CollectionTime"].ToString()
                });
            }
            reader.Close();
            return points;
        }
    }

}
