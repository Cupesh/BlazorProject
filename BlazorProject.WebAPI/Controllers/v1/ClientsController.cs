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
    public class ClientsController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<Client> _clients;
        private Client _client;

        public ClientsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/clients")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ClientsQuery(Db);
            _clients = await query.RetrieveAll();
            return Ok(_clients);
        }

        [HttpGet("api/v1/clients/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ClientsQuery(Db);
            _client = await query.RetrieveOne(id);
            return Ok(_client);
        }

        [HttpPost("api/v1/clients/new")]
        public async Task<IActionResult> Post([FromBody] string client)
        {
            _client = JsonSerializer.Deserialize<Client>(client);
            await Db.Connection.OpenAsync();
            var query = new ClientsQuery(Db);
            var result = await query.CreateOne(_client);
            return Ok(result);
        }

        [HttpPut("api/v1/clients/update")]
        public async Task<IActionResult> Put([FromBody] string client)
        {
            _client = JsonSerializer.Deserialize<Client>(client);
            await Db.Connection.OpenAsync();
            var query = new ClientsQuery(Db);
            var result = await query.RetrieveOne(_client.Id);

            result.Id = _client.Id;
            result.Name = _client.Name;
            result.Address = _client.Address;
            result.PayRate1 = _client.PayRate1;
            result.PayRate2 = _client.PayRate2;

            await query.UpdateOne(result);
            return Ok(result);
        }

        [HttpDelete("api/v1/clients/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ClientsQuery(Db);
            var lookup = await query.RetrieveOne(id);
            if (lookup is null)
                return new NotFoundResult();
            var result = await query.DeleteOne(id);
            return Ok(result);
        }

        [HttpGet("api/v1/clients/lastid")]
        public async Task<IActionResult> GetLastId()
        {
            await Db.Connection.OpenAsync();
            var query = new ClientsQuery(Db);
            var result = await query.RetrieveLastId();
            return Ok(result);
        }
    }
}
