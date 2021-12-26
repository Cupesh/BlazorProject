using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal PayRate1 { get; set; }
        public decimal PayRate2 { get; set; }
        public DateTime Updated { get; set; }

        public Client()
        {
        }
    }
}
