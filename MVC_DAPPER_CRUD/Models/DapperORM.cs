using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Linq;

namespace MVC_DAPPER_CRUD.Models
{
    public static class DapperORM
    {
        public static string connectionString = @"Data Source=DESKTOP-SITH99N;Initial Catalog=DapperDB;User Id=sa;Password=123;";

        public static void ExecuteWithoutReturn(string procedureName, DynamicParameters param=null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<EmployeeModel> GetAllDeletedEmployees()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<EmployeeModel>("GetAllDeletedEmployees", commandType: CommandType.StoredProcedure);
            }
        }
    }
}
