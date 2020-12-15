using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "name cannot exceed 30 chars")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage = "not valid email")]
        [Display(Name = "Office E-mail")]
        public string Email { get; set; }
        [Required]
        public Department? Department { get; set; }
    }
}
