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
    public class WeeklyReportsContractsMapQuery
    {
        public AppDb Db { get; }

        public WeeklyReportsContractsMapQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<WeeklyReportContractMap>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM weeklyreports_contracts";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<List<WeeklyReportContractMap>> RetrieveAllByReport(int report_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT contract_id FROM weeklyreports_contracts WHERE report_id = @report_id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@report_id",
                DbType = DbType.Int32,
                Value = report_id,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result;
        }

        public async Task<IActionResult> CreateOne(WeeklyReportContractMap reportContractMap)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO weeklyreports_contracts (report_id, contract_id) 
                                VALUES (@report_id, contract_id);";
            reportContractMap.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<List<WeeklyReportContractMap>> ReadAll(DbDataReader reader)
        {
            var reportContractMaps = new List<WeeklyReportContractMap>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var reportContractMap = new WeeklyReportContractMap(Db)
                    {
                        ReportId = reader.GetInt32(0),
                        ContractId = reader.GetInt32(1),
                    };
                    reportContractMaps.Add(reportContractMap);
                }
            }
            return reportContractMaps;
        }
    }
}
