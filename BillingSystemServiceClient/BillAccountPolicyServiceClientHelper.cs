using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BillingSystemDataModel;
using Newtonsoft.Json;

namespace BillingSystemServiceClient
{
    class BillAccountPolicyServiceClientHelper
    {
        private readonly HttpClient _httpClient;

        public BillAccountPolicyServiceClientHelper()
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<BillAccountPolicy> GetBillAccountPolicyById(string endUri)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endUri);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to BillAccountPolicy object
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BillAccountPolicy>(json);
                }
                else
                {
                    Console.WriteLine($"Failed to get bill account policy. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
