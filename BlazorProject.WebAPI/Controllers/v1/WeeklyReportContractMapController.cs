using BlazorProject.WebAPI.Models;
using BlazorProject.WebAPI.Models.Queries;
using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Controllers.v1
{
    public class WeeklyReportContractMapController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<WeeklyReportContractMap> _reports;

        public WeeklyReportContractMapController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/weeklyreportscontracts")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsContractsMapQuery(Db);
            _reports = await query.RetrieveAll();
            return Ok(_reports);
        }

        [HttpPost("api/v1/weeklyreportscontracts/new")]
        public async Task<IActionResult> Post([FromBody] string weeklyReportContractMap)
        {
            // how to make it aoutmatically bind to make the method accept an object and not a string?
            WeeklyReportContractMap reportContractMap = JsonSerializer.Deserialize<WeeklyReportContractMap>(weeklyReportContractMap);
            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsContractsMapQuery(Db);
            var result = await query.CreateOne(reportContractMap);
            return Ok(result);
        }
    }
}
