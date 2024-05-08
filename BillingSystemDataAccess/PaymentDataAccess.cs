// <copyright file="PaymentDataAccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides data access methods for interacting with Payment entities.
    /// </summary>
    public class PaymentDataAccess
    {
        private readonly BillingSystemEDMContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentDataAccess"/> class.
        /// </summary>
        public PaymentDataAccess()
        {
            this.context = new BillingSystemEDMContainer();
        }

        /// <summary>
        /// Retrieves a Payment entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the Payment to retrieve.</param>
        /// <returns>The Payment entity corresponding to the provided ID, or null if not found.</returns>
        public Payment GetPaymentById(int id)
        {
            try
            {
                return this.context.Payments.FirstOrDefault(p => p.PaymentId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Payment by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Payment entities.
        /// </summary>
        /// <returns>A list of all Payment entities.</returns>
        public List<Payment> GetAllPayments()
        {
            try
            {
                return this.context.Payments.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all Payments.", ex);
            }
        }

        /// <summary>
        /// Adds a new Payment entity.
        /// </summary>
        /// <param name="payment">The Payment entity to add.</param>
        public void AddPayment(Payment payment)
        {
            try
            {
                this.context.Payments.Add(payment);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Payment.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Payment entity.
        /// </summary>
        /// <param name="payment">The updated Payment entity.</param>
        public void UpdatePayment(Payment payment)
        {
            try
            {
                var existingPayment = this.context.Payments.Find(payment.PaymentId);
                if (existingPayment != null)
                {
                    existingPayment.PaymentMethod = payment.PaymentMethod;
                    existingPayment.PaymentFrom = payment.PaymentFrom;
                    existingPayment.Amount = payment.Amount;
                    existingPayment.BillAccountNumber = payment.BillAccountNumber;
                    existingPayment.PaymentDate = payment.PaymentDate;
                    existingPayment.InvoiceNumber = payment.InvoiceNumber;
                    existingPayment.PaymentStatus = payment.PaymentStatus;
                    existingPayment.PaymentReference = payment.PaymentReference;
                    existingPayment.BillAccountId = payment.BillAccountId;
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Payment.", ex);
            }
        }

        /// <summary>
        /// Deletes a Payment entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the Payment to delete.</param>
        public void DeletePayment(int id)
        {
            try
            {
                var payment = this.context.Payments.Find(id);
                if (payment != null)
                {
                    this.context.Payments.Remove(payment);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Payment.", ex);
            }
        }
    }
}
