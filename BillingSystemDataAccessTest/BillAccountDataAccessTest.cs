using System;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class BillAccountDataAccessTest
    {
        public void TestAddBillAccount()
        {
            Console.WriteLine("Testing AddBillAccount:");

            var newBillAccount = new BillAccount
            {
                BillingType = ApplicationConstants.BILLING_TYPE_DIRECT,
                Status = ApplicationConstants.BILL_ACCOUNT_ACTIVE_STATUS,
                PayorName = "Prakash",
                PayorAddress = "SubashNagar",
                PaymentMethod = ApplicationConstants.BILL_ACCOUNT_CREDITCARD_PAYMENT_METHOD,
                DueDay = 15,
                AccountTotal = 1000.0,
                AccountPaid = 0.0,
                AccountBalance = 0.0,
                LastPaymentDate = null,
                LastPaymentAmount = 0.0,
                PastDue = 0.0,
                FutureDue = 0.0
            };

            new BillAccountDataAccess().AddBillAccount(newBillAccount);
            Console.WriteLine("BillAccount added successfully.");
        }

        public void TestUpdateBillAccount()
        {
            Console.WriteLine("\nTesting UpdateBillAccount:");
            var billAccount = new BillAccountDataAccess().GetBillAccountByNumber("BA123457");
            if (billAccount != null)
            {
                billAccount.BillingType = ApplicationConstants.BILLING_TYPE_AGENT;

                new BillAccountDataAccess().UpdateBillAccount(billAccount);
                Console.WriteLine("BillAccount updated successfully.");
            }
            else
            {
                Console.WriteLine("BillAccount not found.");
            }
        }

        public void TestDeleteBillAccount()
        {
            Console.WriteLine("\nTesting DeleteBillAccount:");

            new BillAccountDataAccess().DeleteBillAccount(2);
            Console.WriteLine("BillAccount deleted successfully.");
        }

        public void TestGetBillAccountById()
        {
            Console.WriteLine("\nTesting GetBillAccountById:");

            var billAccount = new BillAccountDataAccess().GetBillAccountById(4);

            if (billAccount != null)
            {
                Console.WriteLine($"BillAccount found: Id = {billAccount.BillAccountId}, BillAccountNumber = {billAccount.BillAccountNumber}");
            }
            else
            {
                Console.WriteLine("BillAccount not found.");
            }
        }

        public void TestGetBillAccountByNumber()
        {
            Console.WriteLine("\nTesting GetBillAccountByNumber:");

            var billAccount = new BillAccountDataAccess().GetBillAccountByNumber("BA123457");

            if (billAccount != null)
            {
                Console.WriteLine($"BillAccount found: Id = {billAccount.BillAccountId}, BillAccountNumber = {billAccount.BillAccountNumber}");
            }
            else
            {
                Console.WriteLine("BillAccount not found.");
            }
        }

        public void TestSuspendBillAccount()
        {
            var billAccount = new BillAccount
            {
                BillAccountId = 5,
            };
            new BillAccountDataAccess().SuspendBillAccount(billAccount);
            Console.WriteLine("BillAccount suspended Successfully");
        }

        public void TestReleaseBillAccount()
        {
            var billAccount = new BillAccount
            {
                BillAccountId = 5,
            };
            new BillAccountDataAccess().SuspendBillAccount(billAccount);
            Console.WriteLine("BillAccount suspended Successfully");
        }
    }
}
