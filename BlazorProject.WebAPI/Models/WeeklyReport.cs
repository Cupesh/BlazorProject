using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class WeeklyReport
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int WeekNumber { get; set; }

        internal AppDb Db { get; set; }

        public WeeklyReport()
        {
        }

        public WeeklyReport(AppDb db)
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
                ParameterName = "@year",
                DbType = DbType.Int32,
                Value = Year
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@week_number",
                DbType = DbType.Int32,
                Value = WeekNumber
            });
        }
    }
}
