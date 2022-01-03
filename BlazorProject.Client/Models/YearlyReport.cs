using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Models
{
    public class YearlyReport
    {
        [Required]
        public int Year { get; set; }

        public YearlyReport()
        {
        }

        public YearlyReport(int year)
        {
            Year = year;
        }
    }
}
