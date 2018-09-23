using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lautech_Bank.Models.MyModels
{
    public class Transactions
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string accNo { get; set; }

        [Required]
        [Display(Name="Transaction Type")]
        public string trans_detail { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public string amount { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime trans_date { get; set; }
    }
}