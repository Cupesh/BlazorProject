using BlazorProject.WebAPI.Models.Queries;
using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class ContractsController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<Contract> _contracts;
        private Contract _contract;

        public ContractsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/contracts")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ContractsQuery(Db);
            _contracts = await query.RetrieveAll();
            return Ok(_contracts);
        }

        [HttpGet("api/v1/contracts/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ContractsQuery(Db);
            _contract = await query.RetrieveOne(id);
            return Ok(_contract);
        }

        [HttpPost("api/v1/contracts/new")]
        public async Task<IActionResult> Post([FromBody] string contract)
        {
            _contract = JsonSerializer.Deserialize<Contract>(contract);
            await Db.Connection.OpenAsync();
            var query = new ContractsQuery(Db);
            var result = await query.CreateOne(_contract);
            return Ok(result);
        }

        [HttpPut("api/v1/contracts/update")]
        public async Task<IActionResult> Put([FromBody] string contract)
        {
            _contract = JsonSerializer.Deserialize<Contract>(contract);
            await Db.Connection.OpenAsync();
            var query = new ContractsQuery(Db);
            var result = await query.RetrieveOne(_contract.Id);

            result.Id = _contract.Id;
            result.Description = _contract.Description;
            result.Contractor = _contract.Contractor;
            result.Worker = _contract.Worker;
            result.HoursWorked = _contract.HoursWorked;
            result.DateRequested = _contract.DateRequested;
            result.Completed = _contract.Completed;

            await query.UpdateOne(result);
            return Ok(result);
        }

        [HttpDelete("api/v1/contracts/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ContractsQuery(Db);
            var lookup = await query.RetrieveOne(id);
            if (lookup is null)
                return new NotFoundResult();
            var result = await query.DeleteOne(id);
            return Ok(result);
        }

        [HttpGet("api/v1/contracts/lastid")]
        public async Task<IActionResult> GetLastId()
        {
            await Db.Connection.OpenAsync();
            var query = new EmployeesQuery(Db);
            var result = await query.RetrieveLastId();
            return Ok(result);
        }
    }
}
