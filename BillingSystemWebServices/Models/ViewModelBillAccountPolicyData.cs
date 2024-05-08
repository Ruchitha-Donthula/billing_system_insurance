using System.ComponentModel.DataAnnotations;

namespace BillingSystemWebServices.Models
{
    public class ViewModelBillAccountPolicyData
    {
        // Remove BillAccountPolicyId property
        // public int BillAccountPolicyId { get; set; }

        [Required(ErrorMessage = "Policy Number is required.")]
        public string PolicyNumber { get; set; }

        public int BillAccountId { get; set; }

        [Required(ErrorMessage = "Pay Plan is required.")]
        public string PayPlan { get; set; }
    }
}
