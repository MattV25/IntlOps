using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntlOps.Models
{
    public class AccountViewModel
    {
        public class HomeViewModel
        {
            public LoginViewModel LoginModel { get; set; }
            public RegisterViewModel RegisterModel { get; set; }
        }
        public class ApproveAccModel
        {
            public int Id { get; set; }
            public int ApplicationId { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public DateTime ApplicationDate { get; set; }
            public string ApplicationStatus { get; set; }
            public decimal Income { get; set; }
            public decimal CreditRequested { get; set; }
        }
        public class LoginViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string Firstname { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string Lastname { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            public string Birthdate { get; set; }
       
            [Required]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Required]
            [Display(Name = "Marital Status")]
            public string MaritalStatus { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Phone Number")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            public string AccountName { get; set; }
            [Required]
            public string JobTitle { get; set; }
            [Required]
            public string JobType { get; set; }

            [Required]
            public string Street1 { get; set; }

            [Required]
            public string Street2 { get; set; }

            [Required]
            public string City { get; set; }

            public IEnumerable<SelectListItem> States { get; set; }

            [Required]
            public string State { get; set; }

            [Required]
            public string Zipcode { get; set; }
        }
        public class ApplicationsViewModel
        {
            [Required]
            [Display(Name = "Application Date")]
            [DataType(DataType.Date)]
            public DateTime ApplicationDate { get; set; }
            [Required]
            [Display(Name = "Income")]
            [DataType(DataType.Currency)]
            public decimal Income { get; set; }
            [Required]
            [Display(Name = "Credit Limit")]
            [DataType(DataType.Currency)]
            public decimal CreditRequested { get; set; }
        }
        public class RolesViewModel
        {
            public bool Selected { get; set; }

            public List<string> Names { get; set; }
            public IEnumerable<RolesViewModel> rolesViewModel { get; set; }
            public IEnumerable<string> RoleNames { get; set; }
            public IEnumerable<SelectListItem> RolesList { get; set; }

            public int Id { get; set; }
            public string UserName { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Name { get; set; }
        }
        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        public class ResetPasswordViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
    }
}
