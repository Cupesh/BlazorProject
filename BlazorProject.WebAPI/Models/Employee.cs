using BlazorProject.WebAPI.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.WebAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Position { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }

        internal AppDb Db { get; set; }

        public Employee()
        {
        }

        public Employee(AppDb db)
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
                ParameterName = "@first_name",
                DbType = DbType.String,
                Value = FirstName
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_name",
                DbType = DbType.String,
                Value = LastName
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date_of_birth",
                DbType = DbType.DateTime,
                Value = DateOfBirth
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@position",
                DbType = DbType.Int32,
                Value = Position
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date_joined",
                DbType = DbType.DateTime,
                Value = DateJoined
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
