using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BillingSystemWebServices.Models;
using BillingSystemServiceClient;
using BillingSystemDataModel;

namespace BillingSystemWebServices.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateInvoiceForm()
        {
            ViewModelBillAccount viewModelBillAccount = new ViewModelBillAccount();
            return View("CreateInvoiceForm", viewModelBillAccount);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInvoice(BillAccount formData)
        {
            try
            {
                // Get the BillAccount by Id
                BillAccount billAccount = await new BillingSystemServiceClientClass().GetBillAccountById(formData.BillAccountId);

                // Call the method to create invoice from the InvoiceServiceClientClass
                bool isSuccess = await new InvoiceServiceClient().CreateInvoice(billAccount);

                if (isSuccess)
                {
                    // Call the method to get the invoice by bill account ID
                    Invoice invoice = await new InvoiceServiceClient().GetInvoiceByBillAccountId(billAccount.BillAccountId);

                    if (invoice != null)
                    {
                        // Pass the invoice number to the view
                        return View("InvoiceGenerated", model: invoice.InvoiceNumber);
                    }
                    else
                    {
                        // Handle if invoice retrieval fails
                        return Content("Failed to retrieve invoice");
                    }
                }
                else
                {
                    // Handle if creating invoice fails
                    return Content("Failed to create invoice");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return Content($"An error occurred while creating invoice: {ex.Message}");
            }
        }

        
        [Route("api/GenerateInvoicePDF")]
        [HttpGet]
        public async Task<ActionResult> GenerateInvoicePDF(string invoiceNumber)
        {
            try
            {
                // Call the GenerateInvoicePDF method from InvoiceServiceClient to get the PDF content
                string fileName = await new InvoiceServiceClient().GenerateInvoicePDF(invoiceNumber);

                // Check if file name is not null or empty
                if (!string.IsNullOrEmpty(fileName))
                {
                    // Remove illegal characters from the file name
                    string sanitizedFileName = RemoveIllegalCharacters(fileName);

                   

                    // Read the PDF file from the specified path
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(sanitizedFileName);

                    // Return the PDF content as a file
                    return File(pdfBytes, "application/pdf", "InvoicePDF");
                }
                else
                {
                    // Handle if the file name is null or empty
                    return Content("Failed to generate PDF invoice. File name is empty.");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return Content($"An error occurred while generating PDF: {ex.Message}");
            }
        }

        private string RemoveIllegalCharacters(string fileName)
        {
            // Define illegal characters
            string sanitizedFileName = fileName.Trim('"'); 
            sanitizedFileName = sanitizedFileName.TrimEnd('.'); // Remove trailing periods
                                                                // Add more sanitization if necessary

            return sanitizedFileName;
        }
    }

 }

