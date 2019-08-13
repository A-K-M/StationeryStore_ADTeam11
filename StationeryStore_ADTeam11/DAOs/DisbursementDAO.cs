using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.DAOs
{
    public class DisbursementDAO : DatabaseConnection
    {
        public List<MDisbursement> GetDisbursementsByClerk(int clerkID)
        {
            List<MDisbursement> disbursements= new List<MDisbursement>();

            MDisbursement dis = null;
            try
            {
                connection.Open();
                string sql = @"SELECT  ";
                SqlCommand cmd = new SqlCommand(sql, connection);

                if (cmd.ExecuteNonQuery() == 0) throw new Exception();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
            return disbursements;

        }
    }
}