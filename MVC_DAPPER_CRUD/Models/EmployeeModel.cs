using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC_DAPPER_CRUD.Models
{
    public class EmployeeModel
    {
        public int EMPID
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Name is Required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Invalid characters for a name.")]
        [Display(Name = "Employee Name:")]
        public string EMPNAME
        {
            get;
            set;
        }

        [Display(Name ="Employee Designation:")]
        [Required(ErrorMessage = "Designation is Required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Invalid characters for a Designation.")]
        public string EMP_DESIGNATION
        {
            get;
            set;
        }

        [Display(Name ="Employee Salary:")]
        [Required(ErrorMessage = "Salary is Required.")]
        public int EMP_SALARY
        {
            get;
            set;
        }

        [Display(Name ="Gender:")]
        [Required(ErrorMessage = "Gender is Required.")]
        public string EMP_GENDER
        {
            get;
            set;
        }

        [Display(Name ="Employee Email:")]
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string EMP_EMAIL
        {
            get;
            set;
        }

        [Display(Name ="Employee Age:")]
        [Required(ErrorMessage = "Age is Required.")]
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60.")]
        public int? EMP_AGE
        {
            get;
            set;
        }

        public string EMP_SKILLS { get; set; }
        public List<string> skills { get; set; } = new List<string>();
    }
}