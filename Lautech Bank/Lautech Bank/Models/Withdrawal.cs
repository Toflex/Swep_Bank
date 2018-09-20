using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lautech_Bank.Models
{
    public class Withdrawal
    {
        [Required(ErrorMessage="Enter the amount you want to withdraw",AllowEmptyStrings=false)]
        public string withdraw { set; get; }

    }
}