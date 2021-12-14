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
    }
}
