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
    public class EmployeesHttpRepository : IEmployeesHttpRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public EmployeesHttpRepository(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Employee>> GetAll()
        {
            var result = await _client.GetAsync("v1/employees");
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var employees = JsonSerializer.Deserialize<List<Employee>>(content, _options);
            return employees;
        }

        public async Task<Employee> GetOne(int id)
        {
            var result = await _client.GetAsync("v1/employees/" + id);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var employee = JsonSerializer.Deserialize<Employee>(content, _options);
            return employee;
        }

        public async Task PostOne(Employee employee)
        {
            var json = JsonSerializer.Serialize(employee);
            var result = await _client.PostAsJsonAsync("v1/employees/new", json);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task DeleteOne(int id)
        {
            var result = await _client.DeleteAsync("v1/employees/delete/" + id);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }

        public async Task<int> GetLastId()
        {
            var response = await _client.GetAsync("v1/employees/lastid");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var lastId = JsonSerializer.Deserialize<int>(content, _options);
            int nextAvailableId = lastId + 1;
            return nextAvailableId;
        }
    }
}
