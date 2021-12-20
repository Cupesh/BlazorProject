using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Position { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = new DateTime(1900, 01, 01);

        public Employee()
        {

        }
    }
}
