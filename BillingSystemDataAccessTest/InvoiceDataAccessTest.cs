using System;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class InvoiceDataAccessTest
    {
        public void TestGetInvoiceById()
        {
            Console.WriteLine("Testing GetInvoiceById:");

            var invoice = new InvoiceDataAccess().GetInvoiceById(1);

            if (invoice != null)
            {
                Console.WriteLine($"Invoice found: Id = {invoice.InvoiceId}, InvoiceNumber = {invoice.InvoiceNumber}");
            }
            else
            {
                Console.WriteLine("Invoice not found.");
            }
        }

        public void TestGetAllInvoices()
        {
            Console.WriteLine("\nTesting GetAllInvoices:");

            var invoices = new InvoiceDataAccess().GetAllInvoices();

            if (invoices.Count > 0)
            {
                Console.WriteLine("Invoices found:");
                foreach (var invoice in invoices)
                {
                    Console.WriteLine($"Id = {invoice.InvoiceId}, InvoiceNumber = {invoice.InvoiceNumber}");
                }
            }
            else
            {
                Console.WriteLine("No Invoices found.");
            }
        }

        public void TestAddInvoice()
        {
            Console.WriteLine("\nTesting AddInvoice:");

            var newInvoice = new Invoice
            {
                InvoiceNumber = "INV123456",
                InvoiceDate = DateTime.Now,
                SendDate = DateTime.Now.AddDays(1),
                BillAccountId = 2,
                Status = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_PENDING,
                InvoiceAmount = 1000.0
            };

            new InvoiceDataAccess().AddInvoice(newInvoice);
            Console.WriteLine("Invoice added successfully.");
        }

        public void TestUpdateInvoice()
        {
            Console.WriteLine("\nTesting UpdateInvoice:");

            var invoice = new InvoiceDataAccess().GetInvoiceById(1);

            if (invoice != null)
            {
                invoice.Status = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_BILLED;

                new InvoiceDataAccess().UpdateInvoice(invoice);
                Console.WriteLine("Invoice updated successfully.");
            }
            else
            {
                Console.WriteLine("Invoice not found.");
            }
        }

        public void TestDeleteInvoice()
        {
            Console.WriteLine("\nTesting DeleteInvoice:");

            new InvoiceDataAccess().DeleteInvoice(1);
            Console.WriteLine("Invoice deleted successfully.");
        }
    }
}
