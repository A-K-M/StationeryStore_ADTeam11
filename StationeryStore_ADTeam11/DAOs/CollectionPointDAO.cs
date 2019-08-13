using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;
using System.Data.SqlClient;

namespace StationeryStore_ADTeam11.DAOs
{
    public class CollectionPointDAO:DatabaseConnection
    {
        public List<CollectionPoint> GetCollectionPoints()
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from CollectionPoint";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CollectionPoint collectionPoint = new CollectionPoint()
                {
                    Id = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    ClerkId = (int)reader["EmpID"],
                    CollectionTime = (string)reader["CollectionTime"],
                    Address = (string)reader["Address"]
                };
                collectionPoints.Add(collectionPoint);
            }
            conn.Close();
            return collectionPoints;
        }
        public CollectionPoint GetCollectionPointById(int Id)
        {
            CollectionPoint collectionPoint= new CollectionPoint();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from CollectionPoint where ID='"+Id+"'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                collectionPoint = new CollectionPoint()
                {
                    Id = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    ClerkId = (int)reader["EmpID"],
                    CollectionTime = (string)reader["CollectionTime"],
                    Address = (string)reader["Address"]
                };
            }
            conn.Close();
            return collectionPoint;
        }
    }
}