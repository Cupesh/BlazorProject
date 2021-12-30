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

        // overrides for HashSet methods to handle object uniqness in hashset
        public override bool Equals(object obj)
        {
            Client c = obj as Client;
            return c != null && c.Id == this.Id && c.Name == this.Name;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.Name.GetHashCode();
        }
    }
}
