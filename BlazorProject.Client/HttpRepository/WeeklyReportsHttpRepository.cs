using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public class WeeklyReportsHttpRepository : IWeeklyReportsHttRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public WeeklyReportsHttpRepository(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<WeeklyReport> GetOne(int year, int weekNumber)
        {
            var urlString = year.ToString() + 'a' + weekNumber.ToString();
            var result = await _client.GetAsync("v1/weeklyreports/" + urlString.ToString());
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var weeklyReport = JsonSerializer.Deserialize<WeeklyReport>(content, _options);
            return weeklyReport;
        }
    }
}
