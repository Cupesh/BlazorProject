using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public interface IMonthlyReportsHttpRepository
    {
        Task<List<MonthlyReport>> GetAll();
        Task<MonthlyReport> GetOne(int year, int weekNumber);
        Task PostOne(MonthlyReport report);
    }
}
