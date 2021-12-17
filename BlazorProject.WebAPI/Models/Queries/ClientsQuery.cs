using BlazorProject.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models.Queries
{
    public class ClientsQuery
    {
        public AppDb Db { get; }

        public ClientsQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Client>> RetrieveAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM clients";

            var result = await ReadAll(await cmd.ExecuteReaderAsync());

            return result;
        }

        public async Task<Client> RetrieveOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, name, address, pay_rate_1, pay_rate_2, updated 
                                FROM clients WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAll(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IActionResult> CreateOne(Client client)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO clients (id, name, address, pay_rate_1, pay_rate_2) 
                                VALUES (@id, @name, @address, @pay_rate_1, @pay_rate_2)";
            client.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> UpdateOne(Client client)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE clients SET name = @name, address = @address, 
                                pay_rate_1 = @pay_rate_1, pay_rate_2 = @pay_rate_2 WHERE id = @id";
            client.BindParams(cmd);
            var result = await cmd.ExecuteNonQueryAsync();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> DeleteOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM clients WHERE id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id
            });
            var result = await cmd.ExecuteNonQueryAsync();
            return new OkObjectResult(result);
        }

        public async Task<List<Client>> ReadAll(DbDataReader reader)
        {
            var clients = new List<Client>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var client = new Client(Db)
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        PayRate1 = reader.GetInt32(3),
                        PayRate2 = reader.GetInt32(4),
                        Updated = reader.GetDateTime(5)
                    };
                    clients.Add(client);
                }
            }
            return clients;
        }
    }
}
