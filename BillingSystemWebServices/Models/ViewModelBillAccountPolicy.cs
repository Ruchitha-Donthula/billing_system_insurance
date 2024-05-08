using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSystemWebServices.Models
{
    public class ViewModelBillAccountPolicy
    {
        public string PolicyNumber { get; set; }
        public int BillAccountId { get; set; }
        public string PayPlan { get; set; }

    }
}
