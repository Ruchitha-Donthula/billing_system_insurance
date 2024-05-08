// <copyright file="InvoiceBusiness.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemBusiness
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BillingSystemDataAccess;
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using BillingSystemDataModel;
    using Paragraph = iTextSharp.text.Paragraph;
    using System.Linq;
    using Document = iText.Layout.Document;
    /// <summary>
    /// Provides business logic for creating invoices.
    /// </summary>
    public class InvoiceBusiness
    {
        /// <summary>
        /// Creates an invoice for the specified bill account.
        /// </summary>
        /// <param name="billAccount">The bill account for which to create the invoice.</param>
        /// <returns>The created invoice.</returns>
        public Invoice CreateInvoice(BillAccount billAccount)
        {
            try
            {
                var currentDate = DateTime.Now;

                var installmentSummaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(billAccount.BillAccountId);
                var pendingInstallments = new List<Installment>();

                foreach (var summary in installmentSummaries)
                {
                    pendingInstallments.AddRange(summary.Installments
                        .Where(installment => installment.InstallmentSendDate.Date == currentDate.Date));
                }

                if (!pendingInstallments.Any())
                {
                    return null;
                }

                Invoice invoice = new Invoice
                {
                    InvoiceNumber = this.GenerateInvoiceNumber(),
                    InvoiceDate = DateTime.Now.Date,
                    SendDate = DateTime.Now.Date,
                    InvoiceAmount = this.CalculateTotalAmount(pendingInstallments),
                    BillAccountId = billAccount.BillAccountId,
                    Status = ApplicationConstants.INVOICE_STATUS_SENT,
                };

                new InvoiceDataAccess().AddInvoice(invoice);

                foreach (var installment in pendingInstallments)
                {
                    new InstallmentDataAccess().ActivateInstallmentStatus(installment);
                    var invoiceInstallment = new InvoiceInstallment
                    {
                        InvoiceId = invoice.InvoiceId,
                        InstallmentId = installment.InstallmentId,
                    };

                    new InvoiceDataAccess().AddInvoiceInstallment(invoiceInstallment);
                }

                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating invoice.", ex);
            }
        }

        /// <summary>
        /// Generates a unique invoice number.
        /// </summary>
        /// <returns>The generated invoice number.</returns>
        private string GenerateInvoiceNumber()
        {
            try
            {
                int nextSequenceNumber = this.GetNextSequenceNumberFromDatabase();
                string invoiceNumber = $"IN{nextSequenceNumber:D6}";
                return invoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating invoice number.", ex);
            }
        }

        /// <summary>
        /// Retrieves the next sequence number for generating invoice numbers from the database.
        /// </summary>
        /// <returns>The next sequence number.</returns>
        private int GetNextSequenceNumberFromDatabase()
        {
            try
            {
                int nextSequenceNumberFromDatabase = new InvoiceDataAccess().GetNextSequenceNumber();
                return nextSequenceNumberFromDatabase;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving next sequence number from database.", ex);
            }
        }

        /// <summary>
        /// Calculates the total amount for the invoice based on the pending installments.
        /// </summary>
        /// <param name="pendingInstallments">The list of pending installments.</param>
        /// <returns>The total amount for the invoice.</returns>
        private double CalculateTotalAmount(List<Installment> pendingInstallments)
        {
            try
            {
                double totalAmount = (double)pendingInstallments.Sum(installment => (double)installment.BalanceAmount);
                return totalAmount;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while calculating total amount.", ex);
            }
        }
        public Invoice GetInvoiceByBillAccountId(int billAccountId)
        {
            return new InvoiceDataAccess().GetInvoiceByBillAccountId(billAccountId);
        }

        public string GenerateInvoicePDF(string invoiceNumber)
        {
            try
            {
                // Retrieve the Invoice object using the provided invoice number

                Invoice invoice = new InvoiceDataAccess().GetInvoiceByNumber(invoiceNumber);
                BillAccount billAccount = new BillAccountDataAccess().GetBillAccountById(invoice.BillAccountId);

                if (invoice == null)
                {
                    throw new Exception($"Invoice with number {invoiceNumber} not found.");
                }
                else
                {

                    var currentDate = DateTime.Now;
                    var installmentSummaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(invoice.BillAccountId);
                    var pendingInstallments = new List<Installment>();
                    var policyDetails = new Dictionary<string, double>(); // Changed to HashMap

                    foreach (var summary in installmentSummaries)
                    {
                        pendingInstallments.AddRange(summary.Installments
                            .Where(installment => installment.InstallmentSendDate.Date == currentDate.Date));
                    }

                    foreach (var pendingInstallment in pendingInstallments)
                    {
                        var installmentSummary = new InstallmentDataAccess().GetInstallmentSummaryById(pendingInstallment.InstallmentSummaryId);
                        // Add both the policy number and the installment due amount to the dictionary
                        policyDetails.Add(installmentSummary.PolicyNumber, pendingInstallment.DueAmount);
                    }


                    // Define the directory path where the PDF will be saved
                    string directoryPath = @"C:\inetpub\wwwroot";

                    // Construct the full file path
                    string fileName = $"Invoice_{invoiceNumber}.pdf";
                    string fullPath = Path.Combine(directoryPath, fileName);

                    // Create a new PDF document
                    iTextSharp.text.Document document = new iTextSharp.text.Document();

                    // Create a FileStream to write the PDF file
                    FileStream stm = new FileStream(fullPath, FileMode.Create);

                    // Initialize the PdfWriter to write to the FileStream
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stm);

                    // Open the document
                    document.Open();

                    // Add title
                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    Paragraph title = new Paragraph("\nInvoice\n", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    PdfContentByte canvas = writer.DirectContent;
                    Font headingFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK); // Set font color to blue
                    Phrase heading = new Phrase("ABC Insurance Services", headingFont);
                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_RIGHT, heading, document.Right, document.Top, 0);



                    PdfPTable addressTable = new PdfPTable(2);
                    addressTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    addressTable.WidthPercentage = 100; // Table width 100%

                    Font boldFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                    // Create a Chunk for the bold text
                    Chunk boldSendTo = new Chunk("Send To", boldFont1);





                    // Add Send To address
                    Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10); // Define bold font
                    Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10); // Define normal font
                    string sendToAddress = $"\nSend To:\n\n{billAccount.PayorName}\n{billAccount.PayorAddress}"; // Format the address
                    Paragraph sendToParagraph = new Paragraph(sendToAddress);
                    sendToParagraph.SpacingAfter = 5f; // Add some space after the paragraph
                    sendToParagraph.Alignment = Element.ALIGN_LEFT;
                    PdfPCell sendToCell = new PdfPCell(sendToParagraph);
                    sendToCell.Border = PdfPCell.NO_BORDER;
                    sendToCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    addressTable.AddCell(sendToCell);

                    // Calculate due date
                    DateTime dueDate = invoice.SendDate.AddDays(billAccount.DueDay);

                    // Add From Address information
                    string fromAddress = $"\n\nAgency ID: ABC012\nStatement Date: {invoice.SendDate.ToShortDateString()}\nDue Date: {dueDate.ToShortDateString()}";
                    Paragraph fromAddressParagraph = new Paragraph(fromAddress);
                    fromAddressParagraph.SpacingAfter = 5f; // Add some space after the paragraph
                    fromAddressParagraph.Alignment = Element.ALIGN_RIGHT;
                    PdfPCell fromAddressCell = new PdfPCell(fromAddressParagraph);
                    fromAddressCell.Border = PdfPCell.NO_BORDER;
                    fromAddressCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    addressTable.AddCell(fromAddressCell);

                    // Set the font of the first line (heading) to bold
                    // Set the font of the first line (heading) to bold
                    // Create a font for bold text

                    // Add space between addresses
                    addressTable.SpacingAfter = 20f;

                    // Add address table to the document
                    document.Add(addressTable);







                    // Create table for invoice details
                    PdfPTable table = new PdfPTable(4); // 5 columns
                    table.WidthPercentage = 100; // Table width 100%
                    table.SpacingBefore = 10f; // Space before table

                    // Add invoice details to the table
                    AddInvoiceDetails(table, invoice, policyDetails);

                    // Add table to document
                    document.Add(table);
                    PdfPTable statementTable = new PdfPTable(1); // One column
                    statementTable.WidthPercentage = 100; // Table width 100%

                    // Add a cell with your statement
                    Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    Paragraph footer = new Paragraph("For inquiries, please contact us at abcinsuranceservices.com\nphoneNumber:+91 7890123456\n services open From:Mon-Fri 10:00am-10:00pm", font);
                    footer.Alignment = Element.ALIGN_CENTER;

                    // Add the footer to the document
                    PdfPTable footertable = new PdfPTable(1);
                    footertable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                    footertable.DefaultCell.Border = 0;
                    footertable.AddCell(new PdfPCell(footer) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                    footertable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + footer.TotalLeading, writer.DirectContent);



                    // Close the document
                    document.Close();

                    // Return the file path of the generated PDF
                    return fullPath;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while generating the invoice PDF: {ex.Message}");
            }
        }

        // Method to add invoice details to the table
        private void AddInvoiceDetails(PdfPTable table, Invoice invoice, Dictionary<string, double> policyDetails)
        {
            // Add headers horizontally
            table.AddCell(GetHeaderCell("Invoice Date"));
            table.AddCell(GetHeaderCell("Invoice Number"));
            table.AddCell(GetHeaderCell("Policy Number"));
            table.AddCell(GetHeaderCell("Amount"));

            // Iterate over the policyDetails dictionary and add each policy number and due amount to the table
            foreach (var policyDetail in policyDetails)
            {
                // Add values vertically below the headers
                table.AddCell(new PdfPCell(new Phrase(invoice.InvoiceDate.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                table.AddCell(new PdfPCell(new Phrase(invoice.InvoiceNumber, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                table.AddCell(new PdfPCell(new Phrase(policyDetail.Key, FontFactory.GetFont(FontFactory.HELVETICA, 12)))); // Policy number
                table.AddCell(new PdfPCell(new Phrase($"${policyDetail.Value.ToString()}", FontFactory.GetFont(FontFactory.HELVETICA, 12))));
            }

            // Calculate total amount
            double totalAmount = policyDetails.Sum(x => x.Value);

            // Add a row for total amount
            PdfPCell emptyCell3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            emptyCell3.Border = PdfPCell.NO_BORDER;

            PdfPCell emptyCell4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            emptyCell4.Border = PdfPCell.NO_BORDER;

            table.AddCell(emptyCell4); // Empty cell for invoice number

            PdfPCell totalLabelCell = new PdfPCell(new Phrase("Total Amount", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))); // Bold text for total amount label
            totalLabelCell.Colspan = 2; // Span two columns
            table.AddCell(totalLabelCell);

            PdfPCell totalAmountCell = new PdfPCell(new Phrase($"${totalAmount.ToString()}", FontFactory.GetFont(FontFactory.HELVETICA, 12))); // Total amount
            totalAmountCell.Colspan = 2; // Span two columns
            table.AddCell(totalAmountCell);


        }

        private PdfPCell GetHeaderCell(string header)
        {
            PdfPCell headerCell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return headerCell;
        }


    }

}

