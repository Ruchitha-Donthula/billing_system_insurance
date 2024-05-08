using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSystemWebServices.Models
{
    public class ViewModelCreateInstallmentRequestData
    {
        public int BillAccountId { get; set; }
        public int BillAccountPolicyId { get; set; }
        public double Premium { get; set; }
    }
}
