using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class CollectionPointDAO : DatabaseConnection
    {
        public List<CollectionPoint> GetMCollectionPoints()
        {
            CollectionPoint p = null;

            string sql = "SELECT * FROM CollectionPoint";

            connection.Open();

            SqlCommand cmd = new SqlCommand(sql, connection);

            List<CollectionPoint> points = CollectionPoint.MapToList(cmd.ExecuteReader());

            connection.Close();
            return points;
        }
        public List<CollectionPoint> GetCollectionPointsByClerk(int clerkId)
        {
            CollectionPoint p = null;

            string sql = "SELECT * FROM CollectionPoint WHERE EmpID = @clerkID";

            connection.Open();

            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@clerkID", clerkId);
            List<CollectionPoint> points = CollectionPoint.MapToList(cmd.ExecuteReader());

            connection.Close();
            return points;
        }

        public MCollectionAndRep GetCollecitonPointAndRep(string deptId)
        {
            MCollectionAndRep res = null;
            try
            {
                connection.Open();
                string sql = @"SELECT e.ID repId,e.Name repName,c.ID pointId,c.Name pointName FROM Department d 
                                JOIN CollectionPoint c ON d.CollectionPointID = c.ID
						        JOIN Employee e ON d.RepID = e.ID WHERE d.ID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    res = new MCollectionAndRep()
                    {
                        RepId = (int)reader["repId"],
                        RepName = reader["repName"].ToString(),
                        PointId = (int)reader["pointId"],
                        PointName = reader["pointName"].ToString()
                    };
                }

            }
            catch (Exception e)
            {

                return null;
            }
            finally
            {
                connection.Close();
            }
            return res;

        }
        public bool UpdateCollectionPoint(int collectionPointId, string deptId)
        {
            string sql = @"UPDATE Department SET CollectionPointID = @pointId WHERE ID = @deptId";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@pointId", collectionPointId);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return true;
        }

        public MResponse UpdateCollectionPointAndRep(int collectionPointId, int deptRepresentativeId, string deptId)
        {
            bool depResExist = new EmployeeDAO().checkEmployeeExist(deptRepresentativeId, deptId);
            if (!depResExist) return new MResponse() { Success = false, Message = "Employee doesn't exist." };

            try
            {
                connection.Open();
                string sql = @"UPDATE Department SET CollectionPointID = @pointId,RepID = @repId WHERE ID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@pointId", collectionPointId);
                cmd.Parameters.AddWithValue("@repId", deptRepresentativeId);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception();
            }
            catch (Exception e)
            {
                return new MResponse() { Success = false };
            }
            finally
            {
                connection.Close();
            }

            return new MResponse() { Success = true };
        }


        //public async Task<List<CollectionPoint>> GetCollectionPointsTAsync()
        //{

        //    CollectionPoint p = null;

        //    string sql = "SELECT * FROM CollectionPoint";
        //    await connection.OpenAsync().ConfigureAwait(false);

        //    SqlCommand cmd = new SqlCommand(sql, connection);

        //    List<CollectionPoint> points = new List<CollectionPoint>();
        //    SqlDataReader reader = await cmd.ExecuteReaderAsync();
        //    while (await reader.ReadAsync())
        //    {
        //        p = new CollectionPoint()
        //        {
        //            Id = (int)reader["ID"],
        //            Name = reader["Name"].ToString(),
        //            Address = reader["Address"].ToString(),
        //            CollectionTime = reader["CollectionTime"].ToString()
        //        };
        //        System.Diagnostics.Debug.WriteLine("Collection Point Read  " + reader["Name"].ToString());
        //        points.Add(p);
        //    }
        //    reader.Close();
        //    connection.Close();

        //    return points;

        //}

        //public Task<List<CollectionPoint>> GetCollectionPointsTAsync()
        //{
        //    return Task.Run(async () =>
        //   {
        //       CollectionPoint p = null;

        //       string sql = "SELECT * FROM CollectionPoint";
        //       await connection.OpenAsync().ConfigureAwait(false);

        //       SqlCommand cmd = new SqlCommand(sql, connection);

        //       List<CollectionPoint> points = new List<CollectionPoint>();
        //       SqlDataReader reader = await cmd.ExecuteReaderAsync();
        //       while (await reader.ReadAsync())
        //       {
        //           p = new CollectionPoint()
        //           {
        //               Id = (int)reader["ID"],
        //               Name = reader["Name"].ToString(),
        //               Address = reader["Address"].ToString(),
        //               CollectionTime = reader["CollectionTime"].ToString()
        //           };
        //           points.Add(p);
        //           System.Diagnostics.Debug.WriteLine("Collection Point Read  " + reader["Name"].ToString());

        //       }
        //       reader.Close();
        //       connection.Close();

        //       return points;
        //   });

        //}
        public List<CollectionPoint> GetCollectionPoints() //NZCK
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
        public CollectionPoint GetCollectionPointById(int Id) //NZCK
        {
            CollectionPoint collectionPoint = new CollectionPoint();
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select * from CollectionPoint where ID='" + Id + "'";
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