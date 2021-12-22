using BlazorProject.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public interface IPositionsHttpRepository
    {
        Task<List<Position>> GetAll();
        Task<int> GetIdFromName(string name);
    }
}