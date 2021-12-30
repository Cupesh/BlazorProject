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

        [HttpGet("api/v1/weeklyreports/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsQuery(Db);
            _report = await query.RetrieveOne(id);
            return Ok(_report);
        }

        [HttpDelete("api/v1/weeklyreports/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new WeeklyReportsQuery(Db);
            var lookup = await query.RetrieveOne(id);
            if (lookup is null)
                return new NotFoundResult();
            var result = await query.DeleteOne(id);
            return Ok(result);
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
