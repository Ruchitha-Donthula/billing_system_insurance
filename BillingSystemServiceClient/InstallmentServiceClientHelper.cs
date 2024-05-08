using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static BillingSystemServices.Controllers.InstallmentController;

namespace BillingSystemServiceClient
{
    public class InstallmentServiceClientHelper
    {
        private readonly HttpClient _httpClient;

        public InstallmentServiceClientHelper()
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> CreateInstallmentSchedule(CreateInstallmentScheduleRequest request, string endUri)
        {
            try
            {
                // Serialize the request object to JSON
                var json = JsonConvert.SerializeObject(request);
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
    }
}
