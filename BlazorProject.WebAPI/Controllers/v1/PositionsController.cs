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
    public class PositionsController : ControllerBase
    {
        private AppDb Db { get; set; }
        private List<Position> _positions;
        private Position _position;
        private int _id;

        public PositionsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("api/v1/positions")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new PositionsQuery(Db);
            _positions = await query.RetrieveAll();
            return Ok(_positions);
        }

        [HttpGet("api/v1/positions/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PositionsQuery(Db);
            _position = await query.RetrieveOne(id);
            return Ok(_position);
        }

        [HttpGet("api/v1/positions/getid/{name}")]
        public async Task<IActionResult> GetId(string name)
        {
            await Db.Connection.OpenAsync();
            var query = new PositionsQuery(Db);
            _id = await query.RetrieveId(name);
            return Ok(_id);
        }

        [HttpPost("api/v1/positions/new")]
        public async Task<IActionResult> Post(Position position)
        {
            await Db.Connection.OpenAsync();
            var query = new PositionsQuery(Db);
            var result = await query.CreateOne(position);
            return Ok(result);
        }

        [HttpPut("api/v1/positions/update")]
        public async Task<IActionResult> Put(Position position)
        {
            await Db.Connection.OpenAsync();
            var query = new PositionsQuery(Db);
            var result = await query.RetrieveOne(position.Id);

            result.Id = position.Id;
            result.Name = position.Name;
            result.Description = position.Description;
            result.Updated = position.Updated;

            await query.UpdateOne(result);
            return Ok(result);
        }

        [HttpDelete("api/v1/positions/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PositionsQuery(Db);
            var lookup = await query.RetrieveOne(id);
            if (lookup is null)
                return new NotFoundResult();
            var result = await query.DeleteOne(id);
            return Ok(result);

        }
    }
}
