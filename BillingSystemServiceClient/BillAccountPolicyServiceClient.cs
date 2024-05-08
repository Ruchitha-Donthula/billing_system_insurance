using BillingSystemDataModel;
using System;
using System.Threading.Tasks;

namespace BillingSystemServiceClient
{
    public class BillAccountPolicyServiceClient
    {
        public async Task<BillAccountPolicy> GetBillAccountPolicyById(int billAccountPolicyId)
        {
            try
            {
                // Define the endpoint URI for getting a bill account policy by ID
                string endUri = $"/api/GetBillAccountPolicyById?billAccountPolicyId={billAccountPolicyId}";

                // Call the helper method to perform the HTTP GET request
                return await new BillAccountPolicyServiceClientHelper().GetBillAccountPolicyById(endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
