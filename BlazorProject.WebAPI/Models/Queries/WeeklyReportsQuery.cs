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
    public class WeeklyReportsQuery
    {
        public AppDb Db { get; }

        public WeeklyReportsQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<WeeklyReport>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM weekly_reports";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<WeeklyReport> RetrieveOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, year, week_number FROM weekly_reports WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IActionResult> DeleteOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM weekly_reports WHERE id = @id";
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
            cmd.CommandText = @"SELECT * FROM weekly_reports WHERE id = (SELECT MAX(id) FROM weekly_reports)";
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0].Id : -1;
        }

        public async Task<List<WeeklyReport>> ReadAll(DbDataReader reader)
        {
            var reports = new List<WeeklyReport>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var report = new WeeklyReport(Db)
                    {
                        Id = reader.GetInt32(0),
                        Year = reader.GetInt32(1),
                        WeekNumber = reader.GetInt32(2),
                    };
                    reports.Add(report);
                }
            }
            return reports;
        }
    }
}
