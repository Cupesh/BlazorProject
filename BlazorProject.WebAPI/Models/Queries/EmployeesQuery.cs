using BlazorProject.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models.Queries
{
    public class EmployeesQuery
    {
        public AppDb Db { get; }

        Employee employee { get; set; }

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
