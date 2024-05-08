using System;
using System.Linq;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class InstallmentSummaryDataAccessTest
    {
        public void TestGetInstallmentSummaryById()
        {
            Console.WriteLine("Testing GetInstallmentSummaryById:");

            var installmentSummary = new InstallmentDataAccess().GetInstallmentSummaryById(1);

            if (installmentSummary != null)
            {
                Console.WriteLine($"InstallmentSummary found: Id = {installmentSummary.InstallmentSummaryId}, PolicyNumber = {installmentSummary.PolicyNumber}");
            }
            else
            {
                Console.WriteLine("InstallmentSummary not found.");
            }
        }

        public void TestGetAllInstallmentSummaries()
        {
            Console.WriteLine("\nTesting GetAllInstallmentSummaries:");

            var installmentSummaries = new InstallmentDataAccess().GetAllInstallmentSummaries();

            if (installmentSummaries.Count > 0)
            {
                Console.WriteLine("InstallmentSummaries found:");
                foreach (var installmentSummary in installmentSummaries)
                {
                    Console.WriteLine($"Id = {installmentSummary.InstallmentSummaryId}, PolicyNumber = {installmentSummary.PolicyNumber}");
                }
            }
            else
            {
                Console.WriteLine("No InstallmentSummaries found.");
            }
        }

        public void TestAddInstallmentSummary()
        {
            Console.WriteLine("\nTesting AddInstallmentSummary:");

            var newInstallmentSummary = new InstallmentSummary
            {
                BillAccountId = 3,
                PolicyNumber = "POL123456",
                Status = ApplicationConstants.INSTALLMENT_SUMMARY_ACTIVE_STATUS,
            };

            new InstallmentDataAccess().AddInstallmentSummary(newInstallmentSummary);
            Console.WriteLine("InstallmentSummary added successfully.");
        }

        public void TestUpdateInstallmentSummary()
        {
            Console.WriteLine("\nTesting UpdateInstallmentSummary:");

            var installmentSummary = new InstallmentDataAccess().GetInstallmentSummaryById(1);

            if (installmentSummary != null)
            {
                installmentSummary.Status = ApplicationConstants.INSTALLMENT_SUMMARY_INACTIVE_STATUS;

                new InstallmentDataAccess().UpdateInstallmentSummary(installmentSummary);
                Console.WriteLine("InstallmentSummary updated successfully.");
            }
            else
            {
                Console.WriteLine("InstallmentSummary not found.");
            }
        }

        public void TestDeleteInstallmentSummary()
        {
            Console.WriteLine("\nTesting DeleteInstallmentSummary:");

            new InstallmentDataAccess().DeleteInstallmentSummary(1);
            Console.WriteLine("InstallmentSummary deleted successfully.");
        }

        public void GetInstallmentSummariesByBillAccountId()
        {
            var summaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(5);

            if (summaries != null && summaries.Any())
            {
                foreach (var summary in summaries)
                {
                    Console.WriteLine($"Installment Summary ID: {summary.InstallmentSummaryId}, Policy Number: {summary.PolicyNumber}, Status: {summary.Status}");
                   
                }
            }
            else
            {
                Console.WriteLine("No installment summaries found for the provided BillAccountId.");
            }
        }
    }
}
