using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models.Queries
{
    public class EmployeesQuery
    {
        public AppDb Db { get; }

        public EmployeesQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Employee>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM employees";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<Employee> RetrieveOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, first_name, last_name, date_of_birth, position, date_joined, updated 
                                FROM employees WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IActionResult> CreateOne(Employee employee)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO employees (id, first_name, last_name, date_of_birth, position, date_joined) 
                                VALUES (@id, @first_name, @last_name, @date_of_birth, @position, @date_joined);";
            employee.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> UpdateOne(Employee employee)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE employees SET first_name = @first_name, last_name = @last_name, 
                                date_of_birth = @date_of_birth, position = @position, date_joined = @date_joined WHERE id = @id";
            employee.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> DeleteOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM employees WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id
            });
            var result = await cmd.ExecuteNonQueryAsync();
            return new OkObjectResult(result);
        }

        public async Task<int> RetrieveLastId()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM employees WHERE id = (SELECT MAX(id) FROM employees)";
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0].Id : -1;
        }

        public async Task<List<Employee>> ReadAll(DbDataReader reader)
        {
            var employees = new List<Employee>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var employee = new Employee(Db)
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateOfBirth = reader.GetDateTime(3),
                        Position = reader.GetInt32(4),
                        DateJoined = reader.GetDateTime(5),
                        Updated = reader.GetDateTime(6)
                    };
                    employees.Add(employee);
                }
            }
            return employees;
        }
    }
}
