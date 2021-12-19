using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public interface IEmployeesHttpRepository
    {
        Task<List<Employee>> GetAll();
        Task<Employee> GetOne(int id);
    }
}
