using System;

namespace BillingSystemWebServices.Models
{
    public class ViewModelPayment
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentFrom { get; set; }
        public double Amount { get; set; }
        public string BillAccountNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentReference { get; set; }
        public int BillAccountId { get; set; }
    }
}
