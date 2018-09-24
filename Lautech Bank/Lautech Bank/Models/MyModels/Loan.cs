using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lautech_Bank.Models.MyModels
{
    public class Loan
    {
        public long Id {get;set;}
        
        public string accNo { set; get; }

        public double amount { set; get; }

        public int duration { set; get; }

        public DateTime loanDate { set; get; }

        public bool Pending { set; get; }

    }
}