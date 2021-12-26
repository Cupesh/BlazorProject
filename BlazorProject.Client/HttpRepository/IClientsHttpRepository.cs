using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.HttpRepository
{
    public interface IClientsHttpRepository
    {
        Task<List<Models.Client>> GetAll();
        Task<Models.Client> GetOne(int id);
        Task DeleteOne(int id);
        Task PostOne(Models.Client client);
        Task<int> GetLastId();
        Task PutOne(Models.Client client);
    }
}
