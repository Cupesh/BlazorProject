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
    public class MonthlyReportsQuery
    {
        public AppDb Db { get; }

        public MonthlyReportsQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<MonthlyReport>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM monthly_reports";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<MonthlyReport> RetrieveOne(int year, int month)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT year, month FROM monthly_reports WHERE year = @year AND month = @month";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@year",
                DbType = DbType.Int32,
                Value = year,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@month",
                DbType = DbType.Int32,
                Value = month,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IActionResult> CreateOne(MonthlyReport report)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO monthly_reports (year, month) 
                                VALUES (@year, @month);";
            report.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> DeleteOne(int year, int month)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM monthly_reports WHERE year = @year AND month = @month";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@year",
                DbType = DbType.Int32,
                Value = year
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@month",
                DbType = DbType.Int32,
                Value = month
            });
            var result = await cmd.ExecuteNonQueryAsync();
            return new OkObjectResult(result);
        }

        public async Task<List<MonthlyReport>> ReadAll(DbDataReader reader)
        {
            var reports = new List<MonthlyReport>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var report = new MonthlyReport(Db)
                    {
                        Year = reader.GetInt32(0),
                        Month = reader.GetInt32(1),
                    };
                    reports.Add(report);
                }
            }
            return reports;
        }
    }
}
