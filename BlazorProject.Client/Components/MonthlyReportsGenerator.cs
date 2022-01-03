using BlazorProject.Client.HttpRepository;
using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorProject.Client.Components
{
    public class MonthlyReportsGenerator
    {
        public MonthlyReport Report { get; set; }
        private HttpClient Http { get; set; }
        private ContractsHttpRepository ContractsHttpRepo { get; set; }
        private ClientsHttpRepository ClientsHttpRepo { get; set; }
        private EmployeesHttpRepository EmployeesHttpRepo { get; set; }

        public MonthlyReportsGenerator(int year, int month)
        {
            Http = new HttpClient();
            Http.BaseAddress = new Uri("https://localhost:44377/api/");
            ContractsHttpRepo = new ContractsHttpRepository(Http);
            ClientsHttpRepo = new ClientsHttpRepository(Http);
            EmployeesHttpRepo = new EmployeesHttpRepository(Http);

            Report = new MonthlyReport(year, month);
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

            Report.PaymentsTotal = Report.PaymentsPerContracts.Sum(x => x.Value);
            Report.HoursTotal = Report.SumHoursByClient.Sum(x => x.Value);
        }

        private async Task<List<Contract>> GetContracts()
        {
            List<Contract> allContracts = await ContractsHttpRepo.GetAll();
            List<Contract> filteredContracts = new();

            foreach (var contract in allContracts)
            {
                if (contract.DateRequested.Year == Report.Year)
                {
                    if (contract.DateRequested.Month == Report.Month && contract.Completed == true)
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
    }
}
