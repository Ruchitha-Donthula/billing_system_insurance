using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class InvoiceInstallmentDataAccessTest
    {
        public void TestGetInvoiceInstallmentById()
        {
            Console.WriteLine("Testing GetInvoiceInstallmentById:");

            var invoiceInstallment = new InvoiceDataAccess().GetInvoiceInstallmentById(1);

            if (invoiceInstallment != null)
            {
                Console.WriteLine($"InvoiceInstallment found: Id = {invoiceInstallment.InvoiceInstallmentId}, InvoiceId = {invoiceInstallment.InvoiceId}, InstallmentId = {invoiceInstallment.InstallmentId}");
            }
            else
            {
                Console.WriteLine("InvoiceInstallment not found.");
            }
        }

        public void TestGetAllInvoiceInstallments()
        {
            Console.WriteLine("\nTesting GetAllInvoiceInstallments:");

            var invoiceInstallments = new InvoiceDataAccess().GetAllInvoiceInstallments();

            if (invoiceInstallments.Count > 0)
            {
                Console.WriteLine("InvoiceInstallments found:");
                foreach (var invoiceInstallment in invoiceInstallments)
                {
                    Console.WriteLine($"Id = {invoiceInstallment.InvoiceInstallmentId}, InvoiceId = {invoiceInstallment.InvoiceId}, InstallmentId = {invoiceInstallment.InstallmentId}");
                }
            }
            else
            {
                Console.WriteLine("No InvoiceInstallments found.");
            }
        }

        public void TestAddInvoiceInstallment()
        {
            Console.WriteLine("\nTesting AddInvoiceInstallment:");

            var newInvoiceInstallment = new InvoiceInstallment
            {
                InvoiceId = 562,
                InstallmentId = 172
            };

            new InvoiceDataAccess().AddInvoiceInstallment(newInvoiceInstallment);
            Console.WriteLine("InvoiceInstallment added successfully.");
        }

        public void TestDeleteInvoiceInstallment()
        {
            Console.WriteLine("\nTesting DeleteInvoiceInstallment:");

            new InvoiceDataAccess().DeleteInvoiceInstallment(1);
            Console.WriteLine("InvoiceInstallment deleted successfully.");
        }
    }
}
