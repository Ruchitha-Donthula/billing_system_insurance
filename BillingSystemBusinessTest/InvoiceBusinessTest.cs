using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingSystemBusiness;
using BillingSystemDataModel;

namespace BillingSystemBusinessTest
{
    class InvoiceBusinessTest
    {
        public void TestCreateInvoice()
        {
            var billAccount = new BillAccount
            {
                BillAccountId = 26,
                DueDay = 27,
            };
            var invoice = new InvoiceBusiness().CreateInvoice(billAccount);
            if (invoice != null)
            {
                Console.WriteLine($"Invoice created successfully with ID: {invoice.InvoiceId}");
            }
            else
            {
                Console.WriteLine("No pending installments for today's date.");
            }
        }
    }
}
