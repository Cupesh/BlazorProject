using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorProject.Client.Models
{
    public class WeeklyReport
    {
        public int Id { get; set; }

        [Required]
        [Range(2021, 2022, ErrorMessage ="Enter a valid year")]
        public int Year { get; set; }

        [Required]
        [Range(1, 52, ErrorMessage ="Enter a valid week number (1 - 52)")]
        public int WeekNumber { get; set; }

        public List<DateTime> DatesRange = new();
        public List<Contract> FilteredContracts = new();

        public HashSet<Client> FilteredClients = new();
        public HashSet<Employee> FilteredEmployees = new();

        public Dictionary<Client, int> SumHoursByClient = new();
        public Dictionary<Employee, int> SumHoursByEmployee = new();

        public Dictionary<Contract, decimal> PaymentsPerContracts = new();
        public Dictionary<Client, decimal> PaymentsPerClients = new();
        public Dictionary<Employee, decimal> PaymentsPerEmployee = new();

        public WeeklyReport()
        {
        }
        
        public WeeklyReport(int year, int weekNumber)
        {
            Year = year;
            WeekNumber = weekNumber;
        }
        
    }
}
