// <copyright file="BillAccountDataAccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides data access methods for interacting with BillAccount entities.
    /// </summary>
    public class BillAccountDataAccess
    {
        private readonly BillingSystemEDMContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BillAccountDataAccess"/> class.
        /// </summary>
        public BillAccountDataAccess()
        {
            this.context = new BillingSystemEDMContainer();
        }

        /// <summary>
        /// Retrieves a BillAccount entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the BillAccount to retrieve.</param>
        /// <returns>The BillAccount entity corresponding to the provided ID, or null if not found.</returns>
        public BillAccount GetBillAccountById(int id)
        {
            try
            {
                return this.context.BillAccounts.FirstOrDefault(b => b.BillAccountId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving BillAccount by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves all BillAccount entities.
        /// </summary>
        /// <returns>The BillAccount entity corresponding to the provided ID, or null if not found.</returns>
        public List<BillAccount> GetAllBillAccounts()
        {
            try
            {
                return this.context.BillAccounts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all BillAccounts.", ex);
            }
        }

        /// <summary>
        /// Retrieves a BillAccount entity by its account number.
        /// </summary>
        /// <param name="billAccountNumber">The account number of the BillAccount to retrieve.</param>
        /// <returns>The BillAccount entity corresponding to the provided account number, or null if not found.</returns>
        public BillAccount GetBillAccountByNumber(string billAccountNumber)
        {
            try
            {
                return this.context.BillAccounts.FirstOrDefault(b => b.BillAccountNumber == billAccountNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving BillAccount by Number.", ex);
            }
        }

        /// <summary>
        /// Adds a new BillAccount entity.
        /// </summary>
        /// <param name="billAccount">The BillAccount entity to add.</param>
        public void AddBillAccount(BillAccount billAccount)
        {
            try
            {
                this.context.BillAccounts.Add(billAccount);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding BillAccount.", ex);
            }
        }

        /// <summary>
        /// Updates an existing BillAccount entity.
        /// </summary>
        /// <param name="updatedBillAccount">The updated BillAccount entity.</param>
        public void UpdateBillAccount(BillAccount updatedBillAccount)
        {
            try
            {
                var billAccountToUpdate = this.GetBillAccountByNumber(updatedBillAccount.BillAccountNumber);
                if (billAccountToUpdate != null)
                {
                    // Update properties
                    billAccountToUpdate.BillingType = updatedBillAccount.BillingType;
                    billAccountToUpdate.Status = updatedBillAccount.Status;
                    billAccountToUpdate.PayorName = updatedBillAccount.PayorName;
                    billAccountToUpdate.PayorAddress = updatedBillAccount.PayorAddress;
                    billAccountToUpdate.PaymentMethod = updatedBillAccount.PaymentMethod;
                    billAccountToUpdate.DueDay = updatedBillAccount.DueDay;
                    billAccountToUpdate.AccountTotal = updatedBillAccount.AccountTotal;
                    billAccountToUpdate.AccountPaid = updatedBillAccount.AccountPaid;
                    billAccountToUpdate.AccountBalance = updatedBillAccount.AccountBalance;
                    billAccountToUpdate.LastPaymentDate = updatedBillAccount.LastPaymentDate;
                    billAccountToUpdate.LastPaymentAmount = updatedBillAccount.LastPaymentAmount;
                    billAccountToUpdate.PastDue = updatedBillAccount.PastDue;
                    billAccountToUpdate.FutureDue = updatedBillAccount.FutureDue;
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating BillAccount.", ex);
            }
        }

        /// <summary>
        /// Deletes a BillAccount entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the BillAccount to delete.</param>
        public void DeleteBillAccount(int id)
        {
            try
            {
                var billAccount = this.context.BillAccounts.Find(id);
                if (billAccount != null)
                {
                    this.context.BillAccounts.Remove(billAccount);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting BillAccount.", ex);
            }
        }

        /// <summary>
        /// Suspends a BillAccount entity.
        /// </summary>
        /// <param name="billAccount">The BillAccount entity to suspend.</param>
        public void SuspendBillAccount(BillAccount billAccount)
        {
            try
            {
                BillAccount billAccountToSuspend = this.GetBillAccountById(billAccount.BillAccountId);
                billAccountToSuspend.Status = ApplicationConstants.BILL_ACCOUNT_SUSPEND_STATUS;
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while suspending BillAccount.", ex);
            }
        }

        /// <summary>
        /// Releases a suspended BillAccount entity.
        /// </summary>
        /// <param name="billAccount">The suspended BillAccount entity to release.</param>
        public void ReleaseBillAccount(BillAccount billAccount)
        {
            try
            {
                BillAccount billAccountToRelease = this.GetBillAccountById(billAccount.BillAccountId);
                billAccountToRelease.Status = ApplicationConstants.BILL_ACCOUNT_ACTIVE_STATUS;
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while releasing BillAccount.", ex);
            }
        }

        /// <summary>
        /// To get next sequence number from billaccount table in database.
        /// </summary>
        /// <returns>NextSequenceNumber.</returns>
        public int GetNextSequenceNumber()
        {
            try
            {
                var context = new BillingSystemEDMContainer();
                int nextSequenceNumber = 1; // Default if no records exist
                var latestBillAccount = this.context.BillAccounts.OrderByDescending(b => b.BillAccountId).FirstOrDefault();
                if (latestBillAccount != null)
                {
                    // Extract the numeric part and increment by 1
                    string numericPart = latestBillAccount.BillAccountNumber.Substring(2);
                    if (int.TryParse(numericPart, out int numericValue))
                    {
                        nextSequenceNumber = numericValue + 1;
                    }
                }

                return nextSequenceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the next sequence number from the database.", ex);
            }
        }
    }
}
