using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataModel;
using Newtonsoft.Json;

namespace BillingSystemServiceClient
{
    public class PaymentServiceClient
    {
        public async Task<bool> ApplyPayment(Payment payment)
        {
            try
            {
                // Define the endpoint URI for applying payment
                string endUri = "api/ApplyPayment";

                // Delegate the actual HTTP request to PaymentServiceClientHelper
                return await new PaymentServiceClientHelper().ApplyPayment(payment, endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
