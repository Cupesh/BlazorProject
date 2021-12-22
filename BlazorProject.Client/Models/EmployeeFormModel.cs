using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlazorProject.Client.Models
{
    public class EmployeeFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength=2, ErrorMessage = "First Name has to be between 2 - 20 characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last Name has to be between 2 - 20 characters")]
        public string LastName{ get; set;}

        [Required]
        public DateTime DateOfBirth { get; set; } = new DateTime(1900,01,01);

        [Required]
        public string Position { get; set; }

        [Required]
        public DateTime DateJoined { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }
    }
}
