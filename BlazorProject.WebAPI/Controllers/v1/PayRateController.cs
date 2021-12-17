using BlazorProject.WebAPI.Models;
using BlazorProject.WebAPI.Models.Queries;
using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Controllers
{
    public class PayRateController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<PayRate> _payRates;
        private PayRate _payRate;

        public PayRateController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/payrates")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new PayRatesQuery(Db);
            _payRates = await query.RetrieveAll();
            return Ok(_payRates);
        }

        [HttpGet("api/v1/payrates/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PayRatesQuery(Db);
            _payRate = await query.RetrieveOne(id);
            return Ok(_payRate);
        }

        [HttpPost("api/v1/payrates/new")]
        public async Task<IActionResult> Post(PayRate payRate)
        {
            await Db.Connection.OpenAsync();
            var query = new PayRatesQuery(Db);
            var result = await query.CreateOne(payRate);
            return Ok(result);
        }

        [HttpPut("api/v1/payrates/update")]
        public async Task<IActionResult> Put(PayRate payRate)
        {
            await Db.Connection.OpenAsync();
            var query = new PayRatesQuery(Db);
            var result = await query.RetrieveOne(payRate.Id);

            result.Id = payRate.Id;
            result.Description = payRate.Description;
            result.Rate = payRate.Rate;


            await query.UpdateOne(result);
            return Ok(result);
        }

        [HttpDelete("api/v1/payrates/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PayRatesQuery(Db);
            var lookup = await query.RetrieveOne(id);
            if (lookup is null)
                return new NotFoundResult();
            var result = await query.DeleteOne(id);
            return Ok(result);

        }
    }
}
