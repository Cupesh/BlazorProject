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
    public class PayRatesQuery
    {
        public AppDb Db { get; }

        public PayRatesQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<PayRate>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM pay_rates";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<PayRate> RetrieveOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, description, rate, updated 
                                FROM pay_rates WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IActionResult> CreateOne(PayRate payRate)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO pay_rates (id, description, rate)
                                VALUES (@id, @description, @rate)";
            payRate.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> UpdateOne(PayRate payRate)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE pay_rates SET description = @description, rate = @rate WHERE id = @id";
            payRate.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> DeleteOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM pay_rates WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id
            });
            var result = await cmd.ExecuteNonQueryAsync();
            return new OkObjectResult(result);
        }

        public async Task<List<PayRate>> ReadAll(DbDataReader reader)
        {
            var payRates = new List<PayRate>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var payRate = new PayRate(Db)
                    {
                        Id = reader.GetInt32(0),
                        Description = reader.GetString(1),
                        Rate = reader.GetDecimal(2),
                        Updated = reader.GetDateTime(3)
                    };
                    payRates.Add(payRate);
                }
            }
            return payRates;
        }
    }
}
