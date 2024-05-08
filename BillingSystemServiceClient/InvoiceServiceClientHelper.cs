using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataModel;
using Newtonsoft.Json;
using static BillingSystemServices.Controllers.InstallmentController;

namespace BillingSystemServiceClient
{
    public class InvoiceServiceClientHelper
    {
        private readonly HttpClient _httpClient;

        public InvoiceServiceClientHelper()
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> CreateInvoice(BillAccount billAccount, string endUri)
        {
            try
            {
                // Serialize the request object to JSON
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

        public async Task<string> GenerateInvoicePDF(string endUri)
        {
            try
            {
                // Send HTTP GET request to generate PDF invoice
                var response = await _httpClient.GetAsync(endUri);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string (file path)
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle non-successful response
                    Console.WriteLine($"Failed to generate PDF invoice. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<Invoice> GetInvoiceByBillAccountId(string endUri)
        {
            try
            {
                // Send HTTP GET request to get the invoice by bill account ID
                var response = await _httpClient.GetAsync(endUri);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to Invoice object
                    var json = await response.Content.ReadAsStringAsync();
                    var invoice = JsonConvert.DeserializeObject<Invoice>(json);
                    return invoice;
                }
                else
                {
                    // Handle non-successful response
                    Console.WriteLine($"Failed to retrieve invoice by bill account ID. Status code: {response.StatusCode}");
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
