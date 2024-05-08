using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataModel;
using Newtonsoft.Json;

namespace BillingSystemServiceClient
{
    public class InvoiceServiceClient
    {
        public async Task<string> GenerateInvoicePDF(string invoiceNumber)
        {
            try
            {
                // Define the endpoint URI for generating PDF invoices
                string endUri = $"api/GenerateInvoicePDF?invoiceNumber={invoiceNumber}";

                // Delegate the actual HTTP request to InvoiceServiceClientHelper
                return await new InvoiceServiceClientHelper().GenerateInvoicePDF(endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateInvoice(BillAccount billAccount)
        {
            try
            {
                // Define the endpoint URI for creating an invoice
                string endUri = "api/CreateInvoice";

                // Delegate the actual HTTP request to InvoiceServiceClientHelper
                return await new InvoiceServiceClientHelper().CreateInvoice(billAccount, endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<Invoice> GetInvoiceByBillAccountId(int billAccountId)
        {
            try
            {
                // Define the endpoint URI for getting invoice by bill account ID
                string endUri = $"api/GetInvoiceByBillAccountId?billAccountId={billAccountId}";

                // Delegate the actual HTTP request to InvoiceServiceClientHelper
                return await new InvoiceServiceClientHelper().GetInvoiceByBillAccountId(endUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
