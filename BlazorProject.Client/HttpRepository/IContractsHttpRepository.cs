using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public interface IContractsHttpRepository
    {
        Task<List<Contract>> GetAll();
        Task<Contract> GetOne(int id);
        Task PostOne(Contract contract);
        Task PutOne(Contract contract);
        Task DeleteOne(int id);
        Task<int> GetLastId();
    }
}
