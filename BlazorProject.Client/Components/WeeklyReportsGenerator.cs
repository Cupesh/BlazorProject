using BlazorProject.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Components
{
    public class WeeklyReportsGenerator
    {
        private int Year { get; set; }
        private int WeekNumber { get; set; }

        private List<DateTime> DatesRange = new List<DateTime>();
        private List<Contract> AllContracts = new List<Contract>();
        private List<Contract> FilteredContracts = new List<Contract>();

        HashSet<Models.Client> FilteredClients = new HashSet<Models.Client>();
        HashSet<Employee> FilteredEmployees = new HashSet<Employee>();

        private Dictionary<Models.Client, int> SumHoursByClient = new Dictionary<Models.Client, int>();
        private Dictionary<Employee, int> SumHoursByEmployee = new Dictionary<Employee, int>();

        private Dictionary<Contract, decimal> PaymentsPerContracts = new Dictionary<Contract, decimal>();
        private Dictionary<Models.Client, decimal> PaymentsPerClients = new Dictionary<Models.Client, decimal>();
        private Dictionary<Employee, decimal> PaymentsPerEmployee = new Dictionary<Employee, decimal>();

        public WeeklyReportsGenerator(int year, int weekNumber)
        {
            Year = year;
            WeekNumber = weekNumber;
            DatesRange = SetDateRange();
        }

        public void GenerateReport()
        {

        }

        private List<DateTime> SetDateRange()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime startDate = FirstDateOfWeekISO8601(Year, WeekNumber);
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
    //[Parameter]
    //public int year { get; set; }
    //[Parameter]
    //public int weeknumber { get; set; }

    //private bool isWorking { get; set; }

    //private List<DateTime> dates = new List<DateTime>();

    //private List<Models.Contract> allContracts = new List<Models.Contract>();
    //private List<Models.Contract> filteredContracts = new List<Models.Contract>();

    //HashSet<Models.Client> filteredClients = new HashSet<Models.Client>();
    //HashSet<Models.Employee> filteredEmployees = new HashSet<Models.Employee>();

    //private Dictionary<Models.Client, int> sumHoursByClient = new Dictionary<Models.Client, int>();
    //private Dictionary<Models.Employee, int> sumHoursByEmployee = new Dictionary<Models.Employee, int>();

    //private Dictionary<Models.Contract, decimal> paymentsPerContracts = new Dictionary<Models.Contract, decimal>();
    //private Dictionary<Models.Client, decimal> paymentsPerClients = new Dictionary<Models.Client, decimal>();
    //private Dictionary<Models.Employee, decimal> paymentsPerEmployee = new Dictionary<Models.Employee, decimal>();

    //protected override async Task OnInitializedAsync()
    //{
    //    isWorking = true;
    //    dates = SetDateRange();

    //    allContracts = await ContractsHttpRepo.GetAll();
    //    GetContracts();
    //    if (filteredContracts.Count == 0)
    //    {
    //        isWorking = false;
    //        return;
    //    }
    //    await GetClients();
    //    await GetEmployees();

    //    await GenerateReport();

    //    isWorking = false;
    //}

    //private async Task GenerateReport()
    //{
    //    foreach (var client in filteredClients)
    //    {
    //        int sum = GetSumHoursByClient(client.Id);
    //        sumHoursByClient.Add(client, sum);
    //    }

    //    foreach (var employee in filteredEmployees)
    //    {
    //        int sum = GetSumHoursByEmployee(employee.Id);
    //        sumHoursByEmployee.Add(employee, sum);
    //    }

    //    await GetPayments();

    //    return;
    //}

    //private async Task GetPayments()
    //{
    //    foreach (var client in filteredClients)
    //    {
    //        paymentsPerClients.Add(client, 0m);
    //    }

    //    foreach (var employee in filteredEmployees)
    //    {
    //        paymentsPerEmployee.Add(employee, 0m);
    //    }

    //    foreach (var contract in filteredContracts)
    //    {
    //        var employee = await EmployeeHttRepo.GetOne(contract.Worker);
    //        var client = await ClientsHttpRepo.GetOne(contract.Contractor);

    //        if (employee.Position == 1)
    //        {
    //            decimal payment = client.PayRate1 * contract.HoursWorked;
    //            paymentsPerContracts.Add(contract, payment);

    //            paymentsPerClients[client] += payment;
    //            paymentsPerEmployee[employee] += payment;
    //        }
    //        else
    //        {
    //            decimal payment = client.PayRate2 * contract.HoursWorked;
    //            paymentsPerContracts.Add(contract, payment);

    //            paymentsPerClients[client] += payment;
    //            paymentsPerEmployee[employee] += payment;
    //        }
    //    }
    //}

    //private int GetSumHoursByEmployee(int id)
    //{
    //    int sum = 0;

    //    foreach (var contract in filteredContracts)
    //    {
    //        if (contract.Worker == id)
    //        {
    //            sum += contract.HoursWorked;
    //        }
    //    }
    //    return sum;
    //}

    //private int GetSumHoursByClient(int id)
    //{
    //    int sum = 0;

    //    foreach (var contract in filteredContracts)
    //    {
    //        if (contract.Contractor == id)
    //        {
    //            sum += contract.HoursWorked;
    //        }
    //    }
    //    return sum;
    //}

    //private async Task GetEmployees()
    //{
    //    List<Models.Employee> employees = new List<Models.Employee>();

    //    foreach (var contract in filteredContracts)
    //    {
    //        var result = await EmployeeHttRepo.GetOne(contract.Worker);
    //        employees.Add(result);
    //    }

    //    foreach (var employee in employees)
    //    {
    //        filteredEmployees.Add(employee);
    //    }
    //}

    //private async Task GetClients()
    //{
    //    List<Models.Client> clients = new List<Models.Client>();

    //    foreach (var contract in filteredContracts)
    //    {
    //        var result = await ClientsHttpRepo.GetOne(contract.Contractor);
    //        clients.Add(result);
    //    }

    //    foreach (var client in clients)
    //    {
    //        filteredClients.Add(client);
    //    }
    //}

    //private void GetContracts()
    //{
    //    foreach (var date in dates)
    //    {
    //        foreach (var contract in allContracts)
    //        {
    //            if (contract.DateRequested.ToString("dd-MM-yyyy") == date.ToString("dd-MM-yyyy") && contract.Completed == true)
    //            {
    //                filteredContracts.Add(contract);
    //            }
    //        }
    //    }
    //}

    //private List<DateTime> SetDateRange()
    //{
    //    List<DateTime> dates = new List<DateTime>();
    //    DateTime startDate = FirstDateOfWeekISO8601(year, weeknumber);
    //    dates.Add(startDate);

    //    for (int i = 0; i < 6; i++)
    //    {
    //        startDate = startDate.AddDays(1);
    //        dates.Add(startDate);
    //    }
    //    return dates;
    //}

    //private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
    //{
    //    DateTime jan1 = new DateTime(year, 1, 1);
    //    int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

    //    // Use first Thursday in January to get first week of the year as
    //    // it will never be in Week 52/53
    //    DateTime firstThursday = jan1.AddDays(daysOffset);
    //    var cal = CultureInfo.CurrentCulture.Calendar;
    //    int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

    //    var weekNum = weekOfYear;
    //    // As we're adding days to a date in Week 1,
    //    // we need to subtract 1 in order to get the right date for week #1
    //    if (firstWeek == 1)
    //    {
    //        weekNum -= 1;
    //    }

    //    // Using the first Thursday as starting week ensures that we are starting in the right year
    //    // then we add number of weeks multiplied with days
    //    var result = firstThursday.AddDays(weekNum * 7);

    //    // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
    //    return result.AddDays(-3);
    //}
