using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class PayRate
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public DateTime Updated { get; set; }

        internal AppDb Db { get; set; }

        public PayRate()
        {
        }

        public PayRate(AppDb db)
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
                ParameterName = "@description",
                DbType = DbType.String,
                Value = Description
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rate",
                DbType = DbType.Decimal,
                Value = Rate
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
