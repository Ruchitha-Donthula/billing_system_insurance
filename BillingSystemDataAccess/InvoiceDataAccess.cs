// <copyright file="InvoiceDataAccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides data access methods for interacting with Invoice entities.
    /// </summary>
    public class InvoiceDataAccess
    {
        private readonly BillingSystemEDMContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceDataAccess"/> class.
        /// </summary>
        public InvoiceDataAccess()
        {
            this.context = new BillingSystemEDMContainer();
        }

        /// <summary>
        /// Retrieves an Invoice entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the Invoice to retrieve.</param>
        /// <returns>The Invoice entity corresponding to the provided ID, or null if not found.</returns>
        public Invoice GetInvoiceById(int id)
        {
            try
            {
                return this.context.Invoices.FirstOrDefault(i => i.InvoiceId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Invoice by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves an Invoice entity by its number.
        /// </summary>
        /// <param name="number">The number of the Invoice to retrieve.</param>
        /// <returns>The Invoice entity corresponding to the provided number, or null if not found.</returns>
        public Invoice GetInvoiceByNumber(string number)
        {
            try
            {
                return this.context.Invoices.FirstOrDefault(i => i.InvoiceNumber == number);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Invoice by Number.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Invoice entities.
        /// </summary>
        /// <returns>A list of all Invoice entities.</returns>
        public List<Invoice> GetAllInvoices()
        {
            try
            {
                return this.context.Invoices.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all Invoices.", ex);
            }
        }

        /// <summary>
        /// Adds a new Invoice entity.
        /// </summary>
        /// <param name="invoice">The Invoice entity to add.</param>
        public void AddInvoice(Invoice invoice)
        {
            try
            {
                this.context.Invoices.Add(invoice);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Invoice.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Invoice entity.
        /// </summary>
        /// <param name="invoice">The updated Invoice entity.</param>
        public void UpdateInvoice(Invoice invoice)
        {
            try
            {
                var existingInvoice = this.context.Invoices.Find(invoice.InvoiceId);
                if (existingInvoice != null)
                {
                    existingInvoice.InvoiceNumber = invoice.InvoiceNumber;
                    existingInvoice.InvoiceDate = invoice.InvoiceDate;
                    existingInvoice.SendDate = invoice.SendDate;
                    existingInvoice.BillAccountId = invoice.BillAccountId;
                    existingInvoice.Status = invoice.Status;
                    existingInvoice.InvoiceAmount = invoice.InvoiceAmount;
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Invoice.", ex);
            }
        }

        /// <summary>
        /// Deletes an Invoice entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the Invoice to delete.</param>
        public void DeleteInvoice(int id)
        {
            try
            {
                var invoice = this.context.Invoices.Find(id);
                if (invoice != null)
                {
                    this.context.Invoices.Remove(invoice);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Invoice.", ex);
            }
        }

        /// <summary>
        /// Gets the next sequence number for creating a new invoice number.
        /// </summary>
        /// <returns>The next sequence number for invoice number generation.</returns>
        public int GetNextSequenceNumber()
        {
            try
            {
                int nextSequenceNumber = 1; // Default if no records exist
                var latestInvoiceNumber = this.context.Invoices.OrderByDescending(b => b.BillAccountId).FirstOrDefault();
                if (latestInvoiceNumber != null)
                {
                    // Extract the numeric part and increment by 1
                    string numericPart = latestInvoiceNumber.InvoiceNumber.Substring(2);
                    if (int.TryParse(numericPart, out int numericValue))
                    {
                        nextSequenceNumber = numericValue + 1;
                    }
                }

                return nextSequenceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the next sequence number.", ex);
            }
        }

        public Invoice GetInvoiceByBillAccountId(int billAccountId)
        {
            try
            {
                return context.Invoices
                    .Where(i => i.BillAccountId == billAccountId)
                    .OrderBy(i => i.InvoiceDate) // Order by invoice date or any other appropriate property
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while retrieving the first invoice by BillAccount ID: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves an InvoiceInstallment entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the InvoiceInstallment to retrieve.</param>
        /// <returns>The InvoiceInstallment entity corresponding to the provided ID, or null if not found.</returns>
        public InvoiceInstallment GetInvoiceInstallmentById(int id)
        {
            try
            {
                return this.context.InvoiceInstallments.FirstOrDefault(i => i.InvoiceInstallmentId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving InvoiceInstallment by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves all InvoiceInstallment entities.
        /// </summary>
        /// <returns>A list of all InvoiceInstallment entities.</returns>
        public List<InvoiceInstallment> GetAllInvoiceInstallments()
        {
            try
            {
                return this.context.InvoiceInstallments.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all InvoiceInstallments.", ex);
            }
        }

        /// <summary>
        /// Adds a new InvoiceInstallment entity.
        /// </summary>
        /// <param name="invoiceInstallment">The InvoiceInstallment entity to add.</param>
        public void AddInvoiceInstallment(InvoiceInstallment invoiceInstallment)
        {
            try
            {
                this.context.InvoiceInstallments.Add(invoiceInstallment);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding InvoiceInstallment.", ex);
            }
        }

        /// <summary>
        /// Deletes an InvoiceInstallment entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the InvoiceInstallment to delete.</param>
        public void DeleteInvoiceInstallment(int id)
        {
            try
            {
                var invoiceInstallment = this.context.InvoiceInstallments.Find(id);
                if (invoiceInstallment != null)
                {
                    this.context.InvoiceInstallments.Remove(invoiceInstallment);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting InvoiceInstallment.", ex);
            }
        }
    }
}
