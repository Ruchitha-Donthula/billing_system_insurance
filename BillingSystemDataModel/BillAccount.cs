//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BillingSystemDataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class BillAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillAccount()
        {
            this.BillAccountPolicies = new HashSet<BillAccountPolicy>();
            this.BillingTransactions = new HashSet<BillingTransaction>();
            this.InstallmentSummaries = new HashSet<InstallmentSummary>();
            this.Payments = new HashSet<Payment>();
        }
    
        public int BillAccountId { get; set; }
        public string BillAccountNumber { get; set; }
        public string BillingType { get; set; }
        public string Status { get; set; }
        public string PayorName { get; set; }
        public string PayorAddress { get; set; }
        public string PaymentMethod { get; set; }
        public int DueDay { get; set; }
        public Nullable<double> AccountTotal { get; set; }
        public Nullable<double> AccountPaid { get; set; }
        public Nullable<double> AccountBalance { get; set; }
        public Nullable<System.DateTime> LastPaymentDate { get; set; }
        public Nullable<double> LastPaymentAmount { get; set; }
        public Nullable<double> PastDue { get; set; }
        public Nullable<double> FutureDue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillAccountPolicy> BillAccountPolicies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingTransaction> BillingTransactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstallmentSummary> InstallmentSummaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
