using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public interface IWeeklyReportsHttpRepository
    {
        Task<List<WeeklyReport>> GetAll();
        Task<WeeklyReport> GetOne(int year, int weekNumber);
        Task PostOne(WeeklyReport report);
    }
}
