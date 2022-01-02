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
    public class MonthlyReportsController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<MonthlyReport> _reports;
        private MonthlyReport _report;

        public MonthlyReportsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/monthlyreports")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new MonthlyReportsQuery(Db);
            _reports = await query.RetrieveAll();
            return Ok(_reports);
        }

        [HttpGet("api/v1/monthlyreports/{urlString}")]
        public async Task<IActionResult> GetOne(string urlString)
        {
            string[] idsStrings = urlString.Split('a');
            int year = Int32.Parse(idsStrings.First());
            int month = Int32.Parse(idsStrings.Last());

            await Db.Connection.OpenAsync();
            var query = new MonthlyReportsQuery(Db);
            _report = await query.RetrieveOne(year, month);
            return Ok(_report);
        }

        [HttpPost("api/v1/monthlyreports/new")]
        public async Task<IActionResult> Post([FromBody] string report)
        {
            MonthlyReport _report = JsonSerializer.Deserialize<MonthlyReport>(report);
            await Db.Connection.OpenAsync();
            var query = new MonthlyReportsQuery(Db);
            var result = await query.CreateOne(_report);
            return Ok(result);
        }
    }
}
