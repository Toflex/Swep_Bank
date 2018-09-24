using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lautech_Bank.Models.Account
{
    public class LoanClass
    {
        [Required(AllowEmptyStrings=false,ErrorMessage="Enter amount")]
        public String amount { set; get; }

        public String duration { set; get; }
                
    }
}