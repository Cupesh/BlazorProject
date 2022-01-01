using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlazorProject.Client.HttpRepository;
using System.Net.Http;

namespace BlazorProject.Client.Components
{
    public class WeeklyReportsGenerator
    {
        public WeeklyReport Report { get; set; }
        private HttpClient Http { get; set; }
        private ContractsHttpRepository ContractsHttpRepo { get; set; }
        private ClientsHttpRepository ClientsHttpRepo { get; set; }
        private EmployeesHttpRepository EmployeesHttpRepo { get; set; }



        public WeeklyReportsGenerator(int year, int weekNumber)
        {
            Http = new HttpClient();
            Http.BaseAddress = new Uri("https://localhost:44377/api/");
            ContractsHttpRepo = new ContractsHttpRepository(Http);
            ClientsHttpRepo = new ClientsHttpRepository(Http);
            EmployeesHttpRepo = new EmployeesHttpRepository(Http);

            Report = new WeeklyReport(year, weekNumber);
            Report.DatesRange = SetDateRange(year, weekNumber);
        }

        public async Task GetData()
        {
            Report.FilteredContracts = await GetContracts();
            Report.FilteredClients = await GetClients();
            Report.FilteredEmployees = await GetEmployees();
        }

        public async Task Generate()
        {
            await GetSumHours();
            await GetPayments();
        }

        private async Task GetSumHours()
        {
            foreach (var client in Report.FilteredClients)
            {
                int sum = await GetSumHoursByClient(client.Id);
                Report.SumHoursByClient.Add(client, sum);
            }

            foreach (var employee in Report.FilteredEmployees)
            {
                int sum = await GetSumHoursByEmployee(employee.Id);
                Report.SumHoursByEmployee.Add(employee, sum);
            }
        }

        private Task<int> GetSumHoursByEmployee(int id)
        {
            int sum = 0;

            foreach (var contract in Report.FilteredContracts)
            {
                if (contract.Worker == id)
                {
                    sum += contract.HoursWorked;
                }
            }
            return Task.FromResult(sum);
        }

        private Task<int> GetSumHoursByClient(int id)
        {
            int sum = 0;

            foreach (var contract in Report.FilteredContracts)
            {
                if (contract.Contractor == id)
                {
                    sum += contract.HoursWorked;
                }
            }
            return Task.FromResult(sum);
        }

        private async Task GetPayments()
        {
            foreach (var client in Report.FilteredClients)
            {
                Report.PaymentsPerClients.Add(client, 0m);
            }

            foreach (var employee in Report.FilteredEmployees)
            {
                Report.PaymentsPerEmployee.Add(employee, 0m);
            }

            foreach (var contract in Report.FilteredContracts)
            {
                var employee = await EmployeesHttpRepo.GetOne(contract.Worker);
                var client = await ClientsHttpRepo.GetOne(contract.Contractor);

                if (employee.Position == 1)
                {
                    decimal payment = client.PayRate1 * contract.HoursWorked;
                    Report.PaymentsPerContracts.Add(contract, payment);

                    Report.PaymentsPerClients[client] += payment;
                    Report.PaymentsPerEmployee[employee] += payment;
                }
                else
                {
                    decimal payment = client.PayRate2 * contract.HoursWorked;
                    Report.PaymentsPerContracts.Add(contract, payment);

                    Report.PaymentsPerClients[client] += payment;
                    Report.PaymentsPerEmployee[employee] += payment;
                }
            }
        }

        private async Task<List<Contract>> GetContracts()
        {
            List<Contract> allContracts = await ContractsHttpRepo.GetAll();
            List<Contract> filteredContracts = new();

            foreach (var date in Report.DatesRange)
            {
                foreach (var contract in allContracts)
                {
                    if (contract.DateRequested.ToString("dd-mm-yyyy") == date.ToString("dd-mm-yyyy") && contract.Completed == true)
                    {
                        filteredContracts.Add(contract);
                    }
                }
            }
            return filteredContracts;
        }

        private async Task<HashSet<Models.Client>> GetClients()
        {
            HashSet<Models.Client> clients = new HashSet<Models.Client>();

            foreach (var contract in Report.FilteredContracts)
            {
                var result = await ClientsHttpRepo.GetOne(contract.Contractor);
                clients.Add(result);
            }
            return clients;
        }

        private async Task<HashSet<Employee>> GetEmployees()
        {
            HashSet<Employee> employees = new HashSet<Employee>();

            foreach (var contract in Report.FilteredContracts)
            {
                var result = await EmployeesHttpRepo.GetOne(contract.Worker);
                employees.Add(result);
            }
            return employees;
        }

        private List<DateTime> SetDateRange(int year, int weekNumber)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime startDate = FirstDateOfWeekISO8601(year, weekNumber);
            dates.Add(startDate);

            for (int i = 0; i < 6; i++)
            {
                startDate = startDate.AddDays(1);
                dates.Add(startDate);
            }
            return dates;
        }

        private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
    }
}

