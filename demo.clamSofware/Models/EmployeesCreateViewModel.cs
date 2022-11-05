using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace demo.clamSofware.Models
{
    public class EmployeesCreateViewModel
    {
        public int EmployeeId { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please Enter your full name")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please Enter your address")]
        public string Address { get; set; }
        public string Designation { get; set; }
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }

        [Display(Name = "Photo photograph portrait")]
        public List<IFormFile> Photos { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
