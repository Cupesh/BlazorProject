using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Contractor { get; set; }
        public int Worker { get; set; }
        public int HoursWorked { get; set; }
        public DateTime DateRequested { get; set; }
        public bool Completed { get; set; } = false;
        public DateTime Updated { get; set; }

        internal AppDb Db { get; set; }

        public Contract()
        {
        }

        public Contract(AppDb db)
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
                ParameterName = "@contractor",
                DbType = DbType.Int32,
                Value = Contractor
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@worker",
                DbType = DbType.Int32,
                Value = Worker
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hours_worked",
                DbType = DbType.Int32,
                Value = HoursWorked
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date_requested",
                DbType = DbType.DateTime,
                Value = DateRequested
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@completed",
                DbType = DbType.Boolean,
                Value = Completed
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
