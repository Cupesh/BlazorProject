using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public class ContractsHttpRepository : IContractsHttpRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ContractsHttpRepository(HttpClient client)
        {
            _client = client;
            Console.WriteLine(client.BaseAddress);
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Contract>> GetAll()
        {
            var result = await _client.GetAsync("v1/contracts");
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var contracts = JsonSerializer.Deserialize<List<Contract>>(content, _options);
            return contracts;
        }

        public async Task<Contract> GetOne(int id)
        {
            var result = await _client.GetAsync("v1/contracts/" + id);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var contract = JsonSerializer.Deserialize<Contract>(content, _options);
            return contract;
        }

        public async Task PostOne(Contract contract)
        {
            var json = JsonSerializer.Serialize(contract);
            var result = await _client.PostAsJsonAsync("v1/contracts/new", json);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task PutOne(Contract contract)
        {
            var json = JsonSerializer.Serialize(contract);
            var result = await _client.PutAsJsonAsync("v1/contracts/update", json);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task DeleteOne(int id)
        {
            var result = await _client.DeleteAsync("v1/contracts/delete/" + id);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task<int> GetLastId()
        {
            var response = await _client.GetAsync("v1/contracts/lastid");
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
