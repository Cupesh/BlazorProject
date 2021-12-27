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
    public class ContractsQuery
    {
        public AppDb Db { get; }

        public ContractsQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Contract>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM contracts";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<Contract> RetrieveOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, description, contractor, worker, hours_worked, date_requested, completed, updated 
                                FROM contracts WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IActionResult> CreateOne(Contract contract)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO contracts (id, description, contractor, worker, hours_worked, date_requested, completed)
                                VALUES (@id, @description, @contractor, @worker, @hours_worked, @date_requested, @completed)";
            contract.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> UpdateOne(Contract contract)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE contracts SET description = @description, contractor = @contractor, 
                                worker = @worker, hours_worked = @hours_worked, date_requested = @date_requested,
                                completed = @completed WHERE id = @id";
            contract.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> DeleteOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM contracts WHERE id = @id";
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
            cmd.CommandText = @"SELECT * FROM contracts WHERE id = (SELECT MAX(id) FROM contracts)";
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0].Id : -1;
        }

        public async Task<List<Contract>> ReadAll(DbDataReader reader)
        {
            var contracts = new List<Contract>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var contract = new Contract(Db)
                    {
                        Id = reader.GetInt32(0),
                        Description = reader.GetString(1),
                        Contractor = reader.GetInt32(2),
                        Worker = reader.GetInt32(3),
                        HoursWorked = reader.GetInt32(4),
                        DateRequested = reader.GetDateTime(5),
                        Completed = reader.GetBoolean(6),
                        Updated = reader.GetDateTime(7)
                    };
                    contracts.Add(contract);
                }
            }
            return contracts;
        }
    }
}
