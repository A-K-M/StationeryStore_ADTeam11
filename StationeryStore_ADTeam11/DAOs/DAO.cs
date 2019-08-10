using System;
using System.Data.SqlClient;

public class DAO
{
    protected SqlConnection connection;

    public static string _connectionString = "Server=DESKTOP-7J4MING;" 
        + "Database=StationeryInventory; Integrated Security=true";

    public DAO()
    {
        connection = new SqlConnection(_connectionString);
    }
}
