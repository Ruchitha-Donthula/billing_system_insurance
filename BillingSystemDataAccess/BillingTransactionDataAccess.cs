// <copyright file="BillingTransactionDataAccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides data access methods for interacting with BillingTransaction entities.
    /// </summary>
    public class BillingTransactionDataAccess
    {
        private readonly BillingSystemEDMContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BillingTransactionDataAccess"/> class.
        /// </summary>
        public BillingTransactionDataAccess()
        {
            this.context = new BillingSystemEDMContainer();
        }

        /// <summary>
        /// Retrieves a BillingTransaction entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the BillingTransaction to retrieve.</param>
        /// <returns>The BillingTransaction entity corresponding to the provided ID, or null if not found.</returns>
        public BillingTransaction GetBillingTransactionById(int id)
        {
            try
            {
                return this.context.BillingTransactions.FirstOrDefault(b => b.BillingTransactionId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving BillingTransaction by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves all BillingTransaction entities.
        /// </summary>
        /// <returns>A list of all BillingTransaction entities.</returns>
        public List<BillingTransaction> GetAllBillingTransactions()
        {
            try
            {
                return this.context.BillingTransactions.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all BillingTransactions.", ex);
            }
        }

        /// <summary>
        /// Adds a new BillingTransaction entity.
        /// </summary>
        /// <param name="billingTransaction">The BillingTransaction entity to add.</param>
        public void AddBillingTransaction(BillingTransaction billingTransaction)
        {
            try
            {
                this.context.BillingTransactions.Add(billingTransaction);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding BillingTransaction.", ex);
            }
        }

        /// <summary>
        /// Deletes a BillingTransaction entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the BillingTransaction to delete.</param>
        public void DeleteBillingTransaction(int id)
        {
            try
            {
                var billingTransaction = this.context.BillingTransactions.Find(id);
                if (billingTransaction != null)
                {
                    this.context.BillingTransactions.Remove(billingTransaction);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting BillingTransaction.", ex);
            }
        }
    }
}
