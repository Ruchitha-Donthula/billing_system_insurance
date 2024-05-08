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
    class BillingSystemServiceClientHelper
    {
        private readonly HttpClient _httpClient;

        public BillingSystemServiceClientHelper()
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> CreateBillAccount(BillAccount billAccount, string endUri)
        {
            try
            {
                // Serialize the bill account object to JSON
                var json = JsonConvert.SerializeObject(billAccount);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send HTTP POST request
                var response = await _httpClient.PostAsync(endUri, content);

                // Check if the request was successful
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddBillAccountPolicy(object requestData, string endUri)
        {
            try
            {
                // Serialize the request data to JSON
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send HTTP POST request
                var response = await _httpClient.PostAsync(endUri, content);

                // Check if the request was successful
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<BillAccount> GetBillAccountById(string endUri)
        {
            try
            {
                // Send HTTP GET request
                HttpResponseMessage response = await _httpClient.GetAsync(endUri);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to BillAccount object
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BillAccount>(json);
                }
                else
                {
                    Console.WriteLine($"Failed to get bill account. Status code: {response.StatusCode}");
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
