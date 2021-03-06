using BlazorProject.Client.HttpRepository;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorProject.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44377/api/") });
            builder.Services.AddScoped<IEmployeesHttpRepository, EmployeesHttpRepository>();
            builder.Services.AddScoped<IPositionsHttpRepository, PositionsHttpRepository>();
            builder.Services.AddScoped<IClientsHttpRepository, ClientsHttpRepository>();
            builder.Services.AddScoped<IContractsHttpRepository, ContractsHttpRepository>();
            builder.Services.AddScoped<IWeeklyReportsHttpRepository, WeeklyReportsHttpRepository>();
            builder.Services.AddScoped<IMonthlyReportsHttpRepository, MonthlyReportsHttpRepository>();

            //builder.Services.AddOidcAuthentication(options =>
            //{
            //    // Configure your authentication provider options here.
            //    // For more information, see https://aka.ms/blazor-standalone-auth
            //    builder.Configuration.Bind("Local", options.ProviderOptions);
            //});

            await builder.Build().RunAsync();
        }
    }
}
