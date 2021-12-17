using BlazorProject.WebAPI.Models;
using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorProject.WebAPI.Models.Queries;

namespace BlazorProject.WebAPI.Controllers.v1
{
    public class EmployeesController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<Employee> _employees;
        private Employee _employee;

        public EmployeesController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/employees")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new EmployeesQuery(Db);
            _employees = await query.RetrieveAll();
            return Ok(_employees);
        }

        [HttpGet("api/v1/employees/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new EmployeesQuery(Db);
            _employee = await query.RetrieveOne(id);
            return Ok(_employee);
        }

        [HttpPost("api/v1/employees/new")]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await Db.Connection.OpenAsync();
            var query = new EmployeesQuery(Db);
            var result = await query.CreateOne(employee);
            return Ok(result);
        }

        [HttpPut("api/v1/employees/update")]
        public async Task<IActionResult> Put([FromBody] Employee employee)
        {
            await Db.Connection.OpenAsync();
            var query = new EmployeesQuery(Db);
            var result = await query.RetrieveOne(employee.Id);

            result.Id = employee.Id;
            result.FirstName = employee.FirstName;
            result.LastName = employee.LastName;
            result.DateOfBirth = employee.DateOfBirth;
            result.Position = employee.Position;
            result.DateJoined = employee.DateJoined;

            await query.UpdateOne(result);
            return Ok(result);
        }

        [HttpDelete("api/v1/employees/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new EmployeesQuery(Db);
            var lookup = await query.RetrieveOne(id);
            if (lookup is null)
                return new NotFoundResult();
            var result = await query.DeleteOne(id);
            return Ok(result);

        }
    }
}
