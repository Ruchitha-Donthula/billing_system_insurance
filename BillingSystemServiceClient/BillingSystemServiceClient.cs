using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataModel;
using Newtonsoft.Json;
using static BillingSystemServices.Controllers.BillAccountController;

namespace BillingSystemServiceClient
{
    public class BillingSystemServiceClientClass
    {

        public async Task<bool> CreateBillAccount(BillAccount billAccount)
        {
            try
            {
                // Define the endpoint URI for creating a bill account
                string endUri = "api/CreateBillAccount";

                // Delegate the actual HTTP request to ServiceClientHelper
                return await new BillingSystemServiceClientHelper().CreateBillAccount(billAccount, endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddBillAccountPolicy(BillAccountAndPolicyRequest request)
        {
            try
            {


                // Define the endpoint URI for adding a bill account policy
                string endUri = "api/AssociateBillAccountWithPolicy";

                // Delegate the actual HTTP request to ServiceClientHelper
                return await new BillingSystemServiceClientHelper().AddBillAccountPolicy(request, endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
        public async Task<BillAccount> GetBillAccountById(int billAccountId)
        {
            try
            {
                // Define the endpoint URI for getting a bill account by ID
                string endUri = $"api/GetBillAccountById?billAccountId={billAccountId}";

                // Delegate the actual HTTP request to ServiceClientHelper
                return await new BillingSystemServiceClientHelper().GetBillAccountById(endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
