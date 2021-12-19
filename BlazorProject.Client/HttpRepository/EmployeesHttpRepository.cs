using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var response = await _client.GetAsync("v1/employees");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var employees = JsonSerializer.Deserialize<List<Employee>>(content, _options);
            return employees;
        }

        public async Task<Employee> GetOne(int id)
        {
            var response = await _client.GetAsync("v1/employees/" + id);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var employee = JsonSerializer.Deserialize<Employee>(content, _options);
            return employee;
        }
    }
}
