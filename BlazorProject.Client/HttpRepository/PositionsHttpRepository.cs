using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public class PositionsHttpRepository : IPositionsHttpRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public PositionsHttpRepository(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Position>> GetAll()
        {
            var result = await _client.GetAsync("v1/positions");
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var positions = JsonSerializer.Deserialize<List<Position>>(content, _options);
            return positions;
        }

        public async Task<int> GetIdFromName(string name)
        {
            var response = await _client.GetAsync("v1/positions/getid/" + name);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var id = JsonSerializer.Deserialize<int>(content, _options);

            return id;
        }
    }
}
