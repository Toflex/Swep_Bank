using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lautech_Bank.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage="Enter Account Number",AllowEmptyStrings=false)]
        [Display(Name="Account Number")]
        public string Accno { get; set; }

        [Required(ErrorMessage = "Enter password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}