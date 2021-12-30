using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class WeeklyReportContractMap
    {
        public int ReportId { get; set; }
        public int ContractId { get; set; }

        internal AppDb Db { get; set; }

        public WeeklyReportContractMap()
        {
        }

        public WeeklyReportContractMap(AppDb db)
        {
            Db = db;
        }

        public void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@weekly_report_id",
                DbType = DbType.Int32,
                Value = ReportId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@contract_id",
                DbType = DbType.Int32,
                Value = ContractId
            });
        }
    }
}
