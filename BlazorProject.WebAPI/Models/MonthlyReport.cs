using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class MonthlyReport
    {
        public int Year { get; set; }
        public int Month { get; set; }

        internal AppDb Db { get; set; }

        public MonthlyReport()
        {
        }

        public MonthlyReport(AppDb db)
        {
            Db = db;
        }

        public void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@year",
                DbType = DbType.Int32,
                Value = Year
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@month",
                DbType = DbType.Int32,
                Value = Month
            });
        }
    }
}
