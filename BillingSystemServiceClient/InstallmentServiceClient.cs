using System;
using System.Net.Http;
using System.Threading.Tasks;
using static BillingSystemServices.Controllers.InstallmentController;

namespace BillingSystemServiceClient
{
    public class InstallmentServiceClient
    {
        public async Task<bool> CreateInstallmentSchedule(CreateInstallmentScheduleRequest request)
        {
            try
            {
                // Define the endpoint URI for creating installments
                string endUri = "api/CreateInstallmentSchedule";

                // Delegate the actual HTTP request to InstallmentServiceClientHelper
                return await new InstallmentServiceClientHelper().CreateInstallmentSchedule(request, endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GenerateInvoicePDF(string invoiceNumber)
        {
            try
            {
                // Define the endpoint URI for generating PDF invoices with the invoiceNumber parameter
                string endUri = $"api/GenerateInvoicePDF?invoiceNumber={invoiceNumber}";

                // Delegate the actual HTTP request to InstallmentServiceClientHelper
                return await new InvoiceServiceClientHelper().GenerateInvoicePDF(endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

    }
}
