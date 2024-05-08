using System;
using System.ComponentModel.DataAnnotations;

namespace BillingSystemWebServices.Models
{
    public class ViewModelBillAccount
    {
        public int BillAccountId { get; set; }

        public string BillAccountNumber { get; set; }

        [Required(ErrorMessage = "Billing Type is required.")]
        public string BillingType { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Payor Name is required.")]
        public string PayorName { get; set; }

        [Required(ErrorMessage = "Payor Address is required.")]
        public string PayorAddress { get; set; }

        [Required(ErrorMessage = "Payment Method is required.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Due Day is required.")]
        public int DueDay { get; set; }

      
        public double? AccountTotal { get; set; }

        
        public double? AccountPaid { get; set; }

       
        public double? AccountBalance { get; set; }

        public DateTime? LastPaymentDate { get; set; }

        public double? LastPaymentAmount { get; set; }

        public double? PastDue { get; set; }

        public double? FutureDue { get; set; }
    }
}
