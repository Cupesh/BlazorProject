using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Client.Models
{
    public class QuarterlyReport
    {
        [Required]
        [Range(2021, 2022, ErrorMessage = "Enter a valid year")]
        public int Year { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Enter a valid quarter (1 - 4)")]
        public int Quarter { get; set; }

        public QuarterlyReport()
        {
        }

        public QuarterlyReport(int year, int quarter)
        {
            Year = year;
            Quarter = quarter;
        }
    }
}
