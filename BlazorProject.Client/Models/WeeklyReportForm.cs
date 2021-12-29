using System.ComponentModel.DataAnnotations;

namespace BlazorProject.Client.Models
{
    public class WeeklyReportForm
    {
        [Required]
        [Range(2021, 2022, ErrorMessage ="Enter a valid year")]
        public int Year { get; set; }

        [Required]
        [Range(1, 52, ErrorMessage ="Enter a valid week number (1 - 52)")]
        public int WeekNumber { get; set; }

        public WeeklyReportForm()
        {

        }
        
    }
}
