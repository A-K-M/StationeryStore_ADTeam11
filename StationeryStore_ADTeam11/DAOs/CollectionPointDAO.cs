using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class CollectionPointDAO : DatabaseConnection
    {
        public List<CollectionPoint> GetCollectionPoints()
        {
            CollectionPoint p = null;

            string sql = "SELECT * FROM CollectionPoint";
            
            connection.Open();

            SqlCommand cmd = new SqlCommand(sql, connection);

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
        public bool UpdateCollectionPoint(int collectionPointId,string deptId) {
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
            finally {
                connection.Close();
            }

            return true;
        }

        public MResponse UpdateCollectionPointAndRep(int collectionPointId,int deptRepresentativeId, string deptId)
        {
            bool depResExist = new EmployeeDAO().checkEmployeeExist(deptRepresentativeId,deptId);
            if (!depResExist) return new MResponse() {Success = false,Message="Employee doesn't exist." };

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
                return new MResponse() { Success=false};
            }
            finally
            {
                connection.Close();
            }

            return new MResponse() { Success = true };
        }

    }
}