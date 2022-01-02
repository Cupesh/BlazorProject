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
    public class MonthlyReportsHttpRepository : IMonthlyReportsHttpRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public MonthlyReportsHttpRepository(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<MonthlyReport>> GetAll()
        {
            var result = await _client.GetAsync("v1/monthlyreports");
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var reports = JsonSerializer.Deserialize<List<MonthlyReport>>(content, _options);
            return reports;
        }

        public async Task<MonthlyReport> GetOne(int year, int weekNumber)
        {
            var urlString = year.ToString() + 'a' + weekNumber.ToString();
            var result = await _client.GetAsync("v1/monthlyreports/" + urlString.ToString());
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var weeklyReport = JsonSerializer.Deserialize<MonthlyReport>(content, _options);
            return weeklyReport;
        }

        public async Task PostOne(MonthlyReport report)
        {
            var json = JsonSerializer.Serialize(report);
            var result = await _client.PostAsJsonAsync("v1/monthlyreports/new", json);
            var response = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(response);
            }
            return;
        }
    }
}
