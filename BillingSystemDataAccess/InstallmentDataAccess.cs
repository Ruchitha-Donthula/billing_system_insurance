// <copyright file="InstallmentDataAccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides data access methods for interacting with Installment entities.
    /// </summary>
    public class InstallmentDataAccess
    {
        private readonly BillingSystemEDMContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallmentDataAccess"/> class.
        /// </summary>
        public InstallmentDataAccess()
        {
            this.context = new BillingSystemEDMContainer();
        }

        /// <summary>
        /// Adds a new Installment entity.
        /// </summary>
        /// <param name="installment">The Installment entity to add.</param>
        public void AddInstallment(Installment installment)
        {
            try
            {
                this.context.Installments.Add(installment);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding an installment.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Installment entity.
        /// </summary>
        /// <param name="installment">The updated Installment entity.</param>
        public void UpdateInstallment(Installment installment)
        {
            try
            {
                var existingInstallment = this.context.Installments.Find(installment.InstallmentId);
                if (existingInstallment != null)
                {
                    // Update properties
                    existingInstallment.InstallmentSequenceNumber = installment.InstallmentSequenceNumber;
                    existingInstallment.InstallmentSendDate = installment.InstallmentSendDate;
                    existingInstallment.InstallmentDueDate = installment.InstallmentDueDate;
                    existingInstallment.DueAmount = installment.DueAmount;
                    existingInstallment.PaidAmount = installment.PaidAmount;
                    existingInstallment.BalanceAmount = installment.BalanceAmount;
                    existingInstallment.InvoiceStatus = installment.InvoiceStatus;
                    existingInstallment.InstallmentSummaryId = installment.InstallmentSummaryId;

                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating an installment.", ex);
            }
        }

        /// <summary>
        /// Deletes an Installment entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the Installment to delete.</param>
        public void DeleteInstallmentById(int id)
        {
            try
            {
                var installment = this.context.Installments.Find(id);
                if (installment != null)
                {
                    this.context.Installments.Remove(installment);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting an installment.", ex);
            }
        }

        /// <summary>
        /// Retrieves an Installment entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the Installment to retrieve.</param>
        /// <returns>The Installment entity corresponding to the provided ID, or null if not found.</returns>
        public Installment GetInstallmentById(int id)
        {
            try
            {
                return this.context.Installments.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving an installment by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Installment entities.
        /// </summary>
        /// <returns>A list of all Installment entities.</returns>
        public List<Installment> GetAllInstallments()
        {
            try
            {
                return this.context.Installments.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all installments.", ex);
            }
        }

        /// <summary>
        /// Activates the status of an Installment entity to "Billed".
        /// </summary>
        /// <param name="installment">The Installment entity to activate.</param>
        public void ActivateInstallmentStatus(Installment installment)
        {
            try
            {
                Installment installmentToBeActivated = this.GetInstallmentById(installment.InstallmentId);
                installmentToBeActivated.InvoiceStatus = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_BILLED;
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while activating installment status.", ex);
            }
        }

        /// <summary>
        /// Retrieves a list of Installment entities associated with a specific invoice number.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number to retrieve installments for.</param>
        /// <returns>A list of Installment entities associated with the provided invoice number.</returns>
        public List<Installment> GetInstallmentsByInvoiceNumber(string invoiceNumber)
        {
            try
            {
                List<Installment> installments = new List<Installment>();

                using (var dbContext = new BillingSystemEDMContainer())
                {
                    var query = from invoiceInstallment in dbContext.InvoiceInstallments
                                join invoice in dbContext.Invoices
                                on invoiceInstallment.InvoiceId equals invoice.InvoiceId
                                join installment in dbContext.Installments
                                on invoiceInstallment.InstallmentId equals installment.InstallmentId
                                where invoice.InvoiceNumber == invoiceNumber
                                select installment;

                    installments = query.ToList();
                }

                return installments;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving installments by invoice number.", ex);
            }
        }

        /// <summary>
        /// Retrieves future Installment entities associated with a specific bill account ID.
        /// </summary>
        /// <param name="billAccountId">The ID of the bill account to retrieve future installments for.</param>
        /// <returns>A list of future Installment entities associated with the provided bill account ID.</returns>
        public List<Installment> GetFutureInstallmentsByBillAccountId(int billAccountId)
        {
            try
            {
                using (var dbContext = new BillingSystemEDMContainer())
                {
                    var installmentSummaries = dbContext.InstallmentSummaries
                        .Where(summary => summary.BillAccountId == billAccountId)
                        .ToList();

                    var futureInstallments = new List<Installment>();
                    foreach (var summary in installmentSummaries)
                    {
                        var futureInstallmentsForSummary = dbContext.Installments
                            .Where(installment => installment.InstallmentSummaryId == summary.InstallmentSummaryId && installment.InstallmentSendDate > DateTime.Now)
                            .ToList();

                        futureInstallments.AddRange(futureInstallmentsForSummary);
                    }

                    return futureInstallments;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving future installments by BillAccountId.", ex);
            }
        }

        /// <summary>
        /// Retrieves an InstallmentSummary entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the InstallmentSummary to retrieve.</param>
        /// <returns>The InstallmentSummary entity corresponding to the provided ID, or null if not found.</returns>
        public InstallmentSummary GetInstallmentSummaryById(int id)
        {
            try
            {
                return this.context.InstallmentSummaries.FirstOrDefault(i => i.InstallmentSummaryId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving InstallmentSummary by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves all InstallmentSummary entities.
        /// </summary>
        /// <returns>A list of all InstallmentSummary entities.</returns>
        public List<InstallmentSummary> GetAllInstallmentSummaries()
        {
            try
            {
                return this.context.InstallmentSummaries.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all InstallmentSummaries.", ex);
            }
        }

        /// <summary>
        /// Adds a new InstallmentSummary entity.
        /// </summary>
        /// <param name="installmentSummary">The InstallmentSummary entity to add.</param>
        public void AddInstallmentSummary(InstallmentSummary installmentSummary)
        {
            try
            {
                this.context.InstallmentSummaries.Add(installmentSummary);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding InstallmentSummary.", ex);
            }
        }

        /// <summary>
        /// Updates an existing InstallmentSummary entity.
        /// </summary>
        /// <param name="installmentSummary">The updated InstallmentSummary entity.</param>
        public void UpdateInstallmentSummary(InstallmentSummary installmentSummary)
        {
            try
            {
                var existingInstallmentSummary = this.context.InstallmentSummaries.Find(installmentSummary.InstallmentSummaryId);
                if (existingInstallmentSummary != null)
                {
                    existingInstallmentSummary.PolicyNumber = installmentSummary.PolicyNumber;
                    existingInstallmentSummary.Status = installmentSummary.Status;
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating InstallmentSummary.", ex);
            }
        }

        /// <summary>
        /// Deletes an InstallmentSummary entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the InstallmentSummary to delete.</param>
        public void DeleteInstallmentSummary(int id)
        {
            try
            {
                var installmentSummary = this.context.InstallmentSummaries.Find(id);
                if (installmentSummary != null)
                {
                    this.context.InstallmentSummaries.Remove(installmentSummary);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting InstallmentSummary.", ex);
            }
        }

        /// <summary>
        /// Retrieves a list of InstallmentSummary entities associated with a specific bill account ID.
        /// </summary>
        /// <param name="billAccountId">The ID of the bill account to retrieve installment summaries for.</param>
        /// <returns>A list of InstallmentSummary entities associated with the provided bill account ID.</returns>
        public List<InstallmentSummary> GetInstallmentSummariesByBillAccountId(int billAccountId)
        {
            try
            {
                // Filter InstallmentSummaries by BillAccountId
                var summaries = this.context.InstallmentSummaries.Where(summary => summary.BillAccountId == billAccountId).ToList();

                return summaries;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching installment summaries by BillAccountId.", ex);
            }
        }

        /// <summary>
        /// Get Billed Installments By SummaryId.
        /// </summary>
        /// <param name="summaryId">InstallmentSummaryId.</param>
        /// <returns>List of Installments.</returns>
        public List<Installment> GetBilledInstallmentsBySummaryId(int summaryId)
        {
            using (var context = new BillingSystemEDMContainer())
            {
                // Retrieve billed Installments by SummaryId
                return context.Installments
                    .Where(installment => installment.InstallmentSummaryId == summaryId && installment.InvoiceStatus == ApplicationConstants.INSTALLMENT_INVOICE_STATUS_BILLED)
                    .ToList();
            }
        }

        /// <summary>
        /// Get Pending Installments By SummaryId.
        /// </summary>
        /// <param name="summaryId">InstallmentSummaryId.</param>
        /// <returns>List of Installments.</returns>
        public List<Installment> GetPendingInstallmentsBySummaryId(int summaryId)
        {
            using (var context = new BillingSystemEDMContainer())
            {
                // Retrieve pending Installments by SummaryId
                return context.Installments
                    .Where(installment => installment.InstallmentSummaryId == summaryId && installment.InvoiceStatus == ApplicationConstants.INSTALLMENT_INVOICE_STATUS_PENDING)
                    .ToList();
            }
        }
    }
}
