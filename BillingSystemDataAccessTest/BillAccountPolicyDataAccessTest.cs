using System;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class BillAccountPolicyDataAccessTest
    {
        public void TestGetBillAccountPolicyById()
        {
            Console.WriteLine("Testing GetBillAccountPolicyById:");

            var billAccountPolicy = new BillAccountPolicyDataAccess().GetBillAccountPolicyById(2);

            if (billAccountPolicy != null)
            {
                Console.WriteLine($"BillAccountPolicy found: Id = {billAccountPolicy.BillAccountPolicyId}, PolicyNumber = {billAccountPolicy.PolicyNumber}");
            }
            else
            {
                Console.WriteLine("BillAccountPolicy not found.");
            }
        }

        public void TestGetAllBillAccountPolicies()
        {
            Console.WriteLine("\nTesting GetAllBillAccountPolicies:");

            var billAccountPolicies = new BillAccountPolicyDataAccess().GetAllBillAccountPolicies();

            if (billAccountPolicies.Count > 0)
            {
                Console.WriteLine("BillAccountPolicies found:");
                foreach (var billAccountPolicy in billAccountPolicies)
                {
                    Console.WriteLine($"Id = {billAccountPolicy.BillAccountPolicyId}, PolicyNumber = {billAccountPolicy.PolicyNumber}");
                }
            }
            else
            {
                Console.WriteLine("No BillAccountPolicies found.");
            }
        }

        public void TestAddBillAccountPolicy()
        {
            Console.WriteLine("\nTesting AddBillAccountPolicy:");

            var newBillAccountPolicy = new BillAccountPolicy
            {
                PolicyNumber = "POL123456",
                BillAccountId = 3,
            };
            new BillAccountPolicyDataAccess().AddBillAccountPolicy(newBillAccountPolicy);
            Console.WriteLine("BillAccountPolicy added successfully.");
        }

        public void TestDeleteBillAccountPolicy()
        {
            Console.WriteLine("\nTesting DeleteBillAccountPolicy:");

            new BillAccountPolicyDataAccess().DeleteBillAccountPolicy(2);
            Console.WriteLine("BillAccountPolicy deleted successfully.");
        }
    }
}
