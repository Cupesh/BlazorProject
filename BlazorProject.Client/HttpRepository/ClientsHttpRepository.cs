using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorProject.Client.Models;

namespace BlazorProject.Client.HttpRepository
{
    public class ClientsHttpRepository : IClientsHttpRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ClientsHttpRepository(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Models.Client>> GetAll()
        {
            var result = await _client.GetAsync("v1/clients");
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var clients = JsonSerializer.Deserialize<List<Models.Client>>(content, _options);
            return clients;
        }

        public async Task<Models.Client> GetOne(int id)
        {
            var result = await _client.GetAsync("v1/clients/" + id);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var client = JsonSerializer.Deserialize<Models.Client>(content, _options);
            return client;
        }

        public async Task PostOne(Models.Client client)
        {
            var json = JsonSerializer.Serialize(client);
            var result = await _client.PostAsJsonAsync("v1/clients/new", json);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task PutOne(Models.Client client)
        {
            var json = JsonSerializer.Serialize(client);
            var result = await _client.PutAsJsonAsync("v1/clients/update", json);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task DeleteOne(int id)
        {
            var result = await _client.DeleteAsync("v1/clients/delete/" + id);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task<int> GetLastId()
        {
            var response = await _client.GetAsync("v1/clients/lastid");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var lastId = JsonSerializer.Deserialize<int>(content, _options);
            int nextavailableId = lastId + 1;
            return nextavailableId;
        }
    }
}
