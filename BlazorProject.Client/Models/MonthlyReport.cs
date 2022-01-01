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
        [Range(1, 52, ErrorMessage = "Enter a valid week number (1 - 52)")]
        public int Month { get; set; }

        public MonthlyReport()
        {

        }
    }
}
