using BlazorProject.WebAPI.Models;
using BlazorProject.WebAPI.Models.Queries;
using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Controllers.v1
{
    public class WeeklyReportsController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<WeeklyReport> _reports;
        private WeeklyReport _report;

        public WeeklyReportsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/weeklyreports")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsQuery(Db);
            _reports = await query.RetrieveAll();
            return Ok(_reports);
        }

        [HttpGet("api/v1/weeklyreports/{urlString}")]
        public async Task<IActionResult> GetOne(string urlString)
        {
            string[] idsStrings = urlString.Split('a');
            int year = Int32.Parse(idsStrings.First());
            int weekNumber = Int32.Parse(idsStrings.Last());

            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsQuery(Db);
            _report = await query.RetrieveOne(year, weekNumber);
            return Ok(_report);
        }

        [HttpGet("api/v1/weeklyreports/lastid")]
        public async Task<IActionResult> GetLastId()
        {
            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsQuery(Db);
            var result = await query.RetrieveLastId();
            return Ok(result);
        }
    }
}
