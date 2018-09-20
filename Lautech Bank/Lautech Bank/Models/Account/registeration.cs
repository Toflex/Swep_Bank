using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lautech_Bank.Models
{
    
    public class registeration
    {
        [Required(ErrorMessage = "Please enter first name", AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string fName { get; set; }

        [Required(ErrorMessage = "Please enter your Last name", AllowEmptyStrings = false)]
        [Display(Name = "Last name")]
        public string lName { get; set; }

        [Required(ErrorMessage = "Please enter your password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MaxLength(15)]
        public string password { get; set; }

        [Display(Name = "Account Type")]
        public string accountType { get; set; }

        [Required(ErrorMessage = "Email address is required", AllowEmptyStrings = false)]
        [EmailAddress]
        [StringLength(50)]
        [MaxLength(50)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm your password")]
        [Display(Name = "Confirm Password")]
        [StringLength(15)]
        [MaxLength(15)]
        public string confirmpassword { get; set; }
    }   
}