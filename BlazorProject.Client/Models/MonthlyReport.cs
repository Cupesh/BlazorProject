using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Models
{
    public class MonthlyReport
    {
        [Required]
        [Range(2021, 2022, ErrorMessage = "Enter a valid year")]
        public int Year { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Enter a valid month number (1 - 12)")]
        public int Month { get; set; }

        public List<Contract> FilteredContracts = new();

        public HashSet<Client> FilteredClients = new();
        public HashSet<Employee> FilteredEmployees = new();

        public Dictionary<Client, int> SumHoursByClient = new();
        public Dictionary<Employee, int> SumHoursByEmployee = new();

        public Dictionary<Contract, decimal> PaymentsPerContracts = new();
        public Dictionary<Client, decimal> PaymentsPerClients = new();
        public Dictionary<Employee, decimal> PaymentsPerEmployee = new();

        public MonthlyReport()
        {
        }

        public MonthlyReport(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
