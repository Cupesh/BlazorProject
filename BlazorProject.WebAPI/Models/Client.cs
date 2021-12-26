using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal PayRate1 { get; set; }
        public decimal PayRate2 { get; set; }
        public DateTime Updated {get;set;}
        internal AppDb Db { get; set; }

        public Client()
        {
        }

        public Client(AppDb db)
        {
            Db = db;
        }

        public void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = Name
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address",
                DbType = DbType.String,
                Value = Address
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pay_rate_1",
                DbType = DbType.Decimal,
                Value = PayRate1
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pay_rate_2",
                DbType = DbType.Decimal,
                Value = PayRate2
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@updated",
                DbType = DbType.DateTime,
                Value = Updated
            });
        }
    }
}
