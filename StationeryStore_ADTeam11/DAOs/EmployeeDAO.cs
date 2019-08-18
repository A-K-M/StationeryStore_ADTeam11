using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StationeryStore_ADTeam11.MobileModels;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.DAOs
{
    public class EmployeeDAO : DatabaseConnection 
    {
        public Employee GetEmployeeByUsername(string username)
        {
            Employee employee = null;
            try
            {
                connection.Open();
                string sql = @"select id,DeptID,Name,UserName,Password,Email,Role from employee where UserName = @userName";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userName", username);
                employee = Employee.mapToOBj(command.ExecuteReader());
                if (employee == null)
                {
                    throw new Exception();
                }
            }
            catch {
                return null;
            }
            finally {
                connection.Close();
            }

            if (employee.Role.Equals(Constant.ROLE_EMPLOYEE)) {
                if (isRepresentative(employee)) employee.Role = Constant.ROLE_REPRESENTATIVE;
            }
            return employee;
        }

        public bool isRepresentative(Employee emp) {
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                string sql = @"SELECT RepID FROM Department WHERE ID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@deptId", emp.DepartmentId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int RepID = (int)reader["RepID"];
                    if (RepID == emp.Id)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (reader != null) reader.Close();
                connection.Close();
            }
            return false;
        }
        public Employee login(string userName,string password)
        {
            Employee employee = null;
            SqlConnection conn = connection;

            try
            {
                conn.Open();
                string sql = "spMobileLogin";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@password", password);
                employee = Employee.mapToOBj(cmd.ExecuteReader());
            }
            catch
            {
                return null;
            }
            finally {
                conn.Close();
            }

            if (employee != null && employee.Role.Equals(Constant.ROLE_EMPLOYEE))
            {
                if (isRepresentative(employee)) employee.Role = Constant.ROLE_REPRESENTATIVE;
            }
            return employee;

        }
        public Employee GetEmployeeById(int Id)
        {
            Employee employee = null;
            SqlConnection conn = connection;
            try  {
                conn.Open();
                string sql = @"select * from employee where ID = '" + Id + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                employee = Employee.mapToOBj(command.ExecuteReader());
            }
            catch{
                return null;
            }
            finally {
                conn.Close();
            }

            if (employee.Role.Equals(Constant.ROLE_EMPLOYEE)) {
                if (isRepresentative(employee)) employee.Role = Constant.ROLE_REPRESENTATIVE;
            }
            return employee;

        }
        public string GetDepartmentIdByDepartmentHeadUsername(string username)
        {
            string deptId = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select DeptID from Employee where Username='" + username + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                deptId = (string)reader["DeptID"];

            }
            conn.Close();
            return deptId;
        }

        public bool UpdateUserRole(int empId)
        {
            string sql = "UPDATE Employee SET Role = 'Representative'WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", empId);
            connection.Open();

            if (cmd.ExecuteNonQuery() == 0)
            {
                connection.Close();
                return false;
            }

            connection.Close();
            return true;
        }

        public List<Employee> GetEmployeeByDeptId(string deptId)
        {
            List<Employee> employees = new List<Employee>();
            Employee employee = null;
            SqlConnection conn = connection;
            conn.Open();
            //string sql = @"select * from employee where DeptID = '" + deptId + "'";
            //string sql = @"select * from Employee as e, Department as d
            //                where not e.ID=d.HeadID and not e.ID=d.RepID and e.DeptID='"+deptId+"'";

            string sql = "SELECT e.* " +
                          "FROM Employee e, Department d " +
                           "WHERE e.DeptID = d.ID " +
                            "AND e.ID != d.HeadID " +
                            "AND e.ID != d.RepID " +
                            "AND d.ID = '" + deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                employee = new Employee()
                {
                    Id = (int)reader["id"],
                    DepartmentId = (string)reader["DeptID"],
                    Name = (string)reader["Name"],
                    UserName = (string)reader["UserName"],
                    Password = (string)reader["Password"],
                    Email = (string)reader["Email"],
                    Role = (string)reader["Role"]
                };
                employees.Add(employee);
            }
            conn.Close();
            return employees;
        }
        public Employee GetEmployeeByName(string deptId,string name)
        {
            Employee employee = null;
            SqlConnection conn = connection;
            conn.Open();
            string sql = @"select ID from Employee where Name = '" + name + "' and DeptID = '" + deptId + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                employee = new Employee()
                {
                    Id = (int)reader["id"]
                };

            }
            conn.Close();
            return employee;

        }
        public bool checkEmployeeExist(int empId, string deptId) {

            try
            {
                connection.Open();
                string sql = @"SELECT COUNT(*) FROM Employee WHERE ID = @empId AND DeptID = @deptId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                if ((int)cmd.ExecuteScalar() == 0) throw new Exception();
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
        public bool checkDelegation(int empId)
        {
            try
            {
                connection.Open();
                string sql = @"SELECT dl.EmpID
                            FROM Department as d, Delegation as dl, Employee as e
                            WHERE d.DelegateID=dl.ID and d.DelegatedStatus='Ongoing' and e.DeptID=d.ID and e.ID ="+ empId + "";
                SqlCommand cmd = new SqlCommand(sql, connection);

                if ((int)cmd.ExecuteScalar() != empId)
                {
                    throw new Exception();
                }
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

        //public async Task<List<MEmployee>> GetEmployees()
        //{

        //    List<MEmployee> mEmployees = new List<MEmployee>();
        //    await connection.OpenAsync().ConfigureAwait(false);
        //    string sql = @"SELECT id,DeptID,Name,Email FROM employee";
        //    SqlCommand cmd = new SqlCommand(sql, connection);
        //    SqlDataReader reader = await cmd.ExecuteReaderAsync();
        //    while (reader != null && await reader.ReadAsync())
        //    {

        //        mEmployees.Add(new MEmployee()
        //        {
        //            Id = (int)reader["id"],
        //            DepartmentId = (string)reader["deptID"],
        //            Name = (string)reader["Name"],
        //            Email = (string)reader["Email"]
        //        });
        //        System.Diagnostics.Debug.WriteLine("Employee Read " + (string)reader["Name"]);

        //    }
        //    reader.Close();
        //    connection.Close();
        //    return mEmployees;
        //}

        //public Task<List<MEmployee>> GetEmployees()
        //{
        //    return Task.Run(async () =>
        //    {
        //        List<MEmployee> mEmployees = new List<MEmployee>();
        //        await connection.OpenAsync().ConfigureAwait(false);
        //        string sql = @"SELECT id,DeptID,Name,Email FROM employee";
        //        SqlCommand cmd = new SqlCommand(sql, connection);
        //        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        //        while (await reader.ReadAsync())
        //        {

        //            mEmployees.Add(new MEmployee()
        //            {
        //                Id = (int)reader["id"],
        //                DepartmentId = (string)reader["deptID"],
        //                Name = (string)reader["Name"],
        //                Email = (string)reader["Email"]
        //            });
        //            System.Diagnostics.Debug.WriteLine("Employee Read " + (string)reader["Name"]);

        //        }
        //        reader.Close();
        //        connection.Close();
        //        return mEmployees;
        //    });
        //}

    }
}