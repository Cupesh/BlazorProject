using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Contractor { get; set; }
        public int Worker { get; set; }
        public int HoursWorked { get; set; }
        public DateTime DateRequested { get; set; }
        public bool Completed { get; set; }
        public DateTime Updated { get; set; }

        public Contract()
        {
        }

        // overrides for HashSet methods to handle object uniqness in hashset
        public override bool Equals(object obj)
        {
            Contract c = obj as Contract;
            return c != null && c.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
