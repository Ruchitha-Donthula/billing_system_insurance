using System;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class InstallmentDataAccessTest
    {
        public void TestAddInstallment()
        {
            Console.WriteLine("Testing AddInstallment:");

            var newInstallment = new Installment
            {
                InstallmentId = 1,
                InstallmentSequenceNumber = 1,
                InstallmentSendDate = DateTime.Now,
                InstallmentDueDate = DateTime.Now.AddDays(30),
                DueAmount = 500.0,
                PaidAmount = null,
                BalanceAmount = null,
                InvoiceStatus = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_PENDING,
                InstallmentSummaryId = 8,
            };

            new InstallmentDataAccess().AddInstallment(newInstallment);
            Console.WriteLine("Installment added successfully.");
        }

        public void TestGetInstallmentById()
        {
            Console.WriteLine("\nTesting GetInstallmentById:");

            int installmentId = 12;

            var installment = new InstallmentDataAccess().GetInstallmentById(installmentId);

            if (installment != null)
            {
                Console.WriteLine($"Installment found: Id = {installment.InstallmentId}, SequenceNumber = {installment.InstallmentSequenceNumber}");
                Console.WriteLine($"SendDate = {installment.InstallmentSendDate}, DueDate = {installment.InstallmentDueDate}");
                Console.WriteLine($"DueAmount = {installment.DueAmount}, PaidAmount = {installment.PaidAmount}, BalanceAmount = {installment.BalanceAmount}");
                Console.WriteLine($"InvoiceStatus = {installment.InvoiceStatus}");
            }
            else
            {
                Console.WriteLine("Installment not found.");
            }
        }

        public void TestUpdateInstallment()
        {
            Console.WriteLine("\nTesting UpdateInstallment:");

            int installmentId = 12;
            var installment = new InstallmentDataAccess().GetInstallmentById(installmentId);

            if (installment != null)
            {
                installment.DueAmount = 600.0;
                installment.InvoiceStatus = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_BILLED;

                new InstallmentDataAccess().UpdateInstallment(installment);
                Console.WriteLine("Installment updated successfully.");
            }
            else
            {
                Console.WriteLine("Installment not found.");
            }
        }

        public void TestDeleteInstallment()
        {
            Console.WriteLine("\nTesting DeleteInstallment:");

            int installmentId = 1;
            new InstallmentDataAccess().DeleteInstallmentById(installmentId);
            Console.WriteLine("Installment deleted successfully.");
        }
    }
}
