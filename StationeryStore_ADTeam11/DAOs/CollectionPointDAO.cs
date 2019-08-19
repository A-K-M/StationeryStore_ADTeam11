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
        public List<CollectionPoint> GetCollectionPoints()
        {
            CollectionPoint p = null;
            List < CollectionPoint > points = new List<CollectionPoint>();
          string sql = "SELECT * FROM CollectionPoint";
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(sql, connection);

               points = CollectionPoint.MapToList(cmd.ExecuteReader());
            }
            finally {
                connection.Close();
            }
            return points;
        }


        public List<CollectionPoint> GetCollectionPointsByClerk(int clerkId)
        {
            CollectionPoint p = null;
            string sql = "SELECT * FROM CollectionPoint WHERE EmpID = @clerkID";
            List<CollectionPoint> points = new List<CollectionPoint>();
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@clerkID", clerkId);
                points= CollectionPoint.MapToList(cmd.ExecuteReader());
            }
            finally
            {
                connection.Close();
            }
            return points;
        }

        public MCollectionAndRep GetCollecitonPointAndRep(string deptId)
        {
            MCollectionAndRep res = null;
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                string sql = @"SELECT e.ID repId,e.Name repName,c.ID pointId,c.Name pointName,c.CollectionTime time FROM Department d 
                                JOIN CollectionPoint c ON d.CollectionPointID = c.ID
						        JOIN Employee e ON d.RepID = e.ID WHERE d.ID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                 reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    res = new MCollectionAndRep()
                    {
                        RepId = (int)reader["repId"],
                        RepName = reader["repName"].ToString(),
                        PointId = (int)reader["pointId"],
                        PointName = reader["pointName"].ToString(),
                        ColTime = reader["time"].ToString()
                    };
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

        public CollectionPoint GetCollectionPointByDeptID(string deptId)
        {
            string sql = @"SELECT cp.Name,cp.CollectionTime,cp.Address 
                            FROM CollectionPoint cp,Department d
                            WHERE cp.ID = d.CollectionPointID AND d.ID = @deptId ";
            SqlDataReader reader = null;
            CollectionPoint collectionPoint = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                reader = cmd.ExecuteReader();

                if (reader.Read()) { 
                    collectionPoint = new CollectionPoint()
                    {
                        Name = (string)reader["Name"],
                        CollectionTime = (string)reader["CollectionTime"],
                        Address = (string)reader["Address"]
                    };
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

            return collectionPoint;
        }
        public CollectionPoint GetCollectionPointById(int Id) //NZCK
        {
            CollectionPoint collectionPoint = new CollectionPoint();
            SqlConnection conn = connection;
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                string sql = @"select * from CollectionPoint where ID='" + Id + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
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
            }
            finally {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return collectionPoint;
        }
    }
}