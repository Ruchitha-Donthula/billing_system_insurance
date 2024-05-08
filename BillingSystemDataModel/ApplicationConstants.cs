namespace BillingSystemDataModel
{
    public class ApplicationConstants
    {
        public const string BILLING_TYPE_DIRECT = "Direct";
        public const string BILLING_TYPE_AGENT = "Agent";

        public const string BILL_ACCOUNT_ACTIVE_STATUS = "Active";
        public const string BILL_ACCOUNT_SUSPEND_STATUS = "Suspend";

        public const string BILL_ACCOUNT_CASH_PAYMENT_METHOD = "Cash";
        public const string BILL_ACCOUNT_CREDITCARD_PAYMENT_METHOD = "Credit card";
        public const string BILL_ACCOUNT_DEBITCARD_PAYMENT_METHOD = "Debit card";

        public const string POLICY_PAYPLAN_MONTHLY = "Monthly";
        public const string POLICY_PAYPLAN_QUARTERLY = "Quarterly";
        public const string POLICY_PAYPLAN_SEMIANNUAL = "Semiannual";
        public const string POLICY_PAYPLAN_ANNUAL = "Annual";

        public const string INSTALLMENT_SUMMARY_ACTIVE_STATUS = "Active";
        public const string INSTALLMENT_SUMMARY_INACTIVE_STATUS = "Inactive";

        public const string INVOICE_STATUS_NOTSENT = "Invoice not sent";
        public const string INVOICE_STATUS_SENT = "Invoice sent";

        public const string INSTALLMENT_INVOICE_STATUS_BILLED = "Billed";
        public const string INSTALLMENT_INVOICE_STATUS_PENDING = "Pending";

        public const string PAYMENT_STATUS_SUCCESS = "Success";
        public const string PAYMENT_STATUS_PENDING = "Pending";
        public const string PAYMENT_STATUS_FAILED = "Failed";
    }
}
