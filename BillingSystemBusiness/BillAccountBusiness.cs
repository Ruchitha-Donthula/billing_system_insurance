// <copyright file="BillAccountBusiness.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemBusiness
{
    using System;
    using System.Collections.Generic;
    using BillingSystemDataAccess;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides business logic for managing BillAccount entities.
    /// </summary>
    public class BillAccountBusiness
    {
        /// <summary>
        /// Creates a new BillAccount entity.
        /// </summary>
        /// <param name="billAccount">The BillAccount entity to create.</param>
        /// <returns>The created BillAccount entity.</returns>
        public BillAccount CreateBillAccount(BillAccount billAccount)
        {
            try
            {
                string billAccountNumber = this.GenerateBillAccountNumber();
                billAccount.BillAccountNumber = billAccountNumber;
                new BillAccountDataAccess().AddBillAccount(billAccount);
                return billAccount;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Associates a BillAccount entity with one or more policy numbers and a payment plan.
        /// </summary>
        /// <param name="billAccount">The BillAccount entity to associate.</param>
        /// <param name="policyNumbers">The list of policy numbers to associate with the BillAccount.</param>
        /// <param name="payplan">The payment plan.</param>
        public void AssociateBillAccountWithPolicy(BillAccount billAccount, List<string> policyNumbers, string payplan)
        {
            try
            {
                foreach (var policyNumber in policyNumbers)
                {
                    BillAccountPolicy billAccountPolicy = new BillAccountPolicy
                    {
                        BillAccountId = billAccount.BillAccountId,
                        PolicyNumber = policyNumber,
                        PayPlan = payplan,
                    };
                    new BillAccountPolicyDataAccess().AddBillAccountPolicy(billAccountPolicy);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves a BillAccount entity by its ID.
        /// </summary>
        /// <param name="billAccountId">The ID of the BillAccount to retrieve.</param>
        /// <returns>The BillAccount entity corresponding to the provided ID.</returns>
        public BillAccount GetBillAccountById(int billAccountId)
        {
            try
            {
                return new BillAccountDataAccess().GetBillAccountById(billAccountId);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves a BillAccount entity by its number.
        /// </summary>
        /// <param name="billAccountNumber">The number of the BillAccount to retrieve.</param>
        /// <returns>The BillAccount entity corresponding to the provided number.</returns>
        public BillAccount GetBillAccountByNumber(string billAccountNumber)
        {
            try
            {
                return new BillAccountDataAccess().GetBillAccountByNumber(billAccountNumber);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Updates a BillAccount entity.
        /// </summary>
        /// <param name="billAccount">The BillAccount entity to update.</param>
        public void UpdateBillAccount(BillAccount billAccount)
        {
            try
            {
                new BillAccountDataAccess().UpdateBillAccount(billAccount);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Suspends a BillAccount.
        /// </summary>
        /// <param name="billAccount">The BillAccount to suspend.</param>
        public void SuspendBillAccount(BillAccount billAccount)
        {
            try
            {
                new BillAccountDataAccess().SuspendBillAccount(billAccount);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Releases a suspended BillAccount.
        /// </summary>
        /// <param name="billAccount">The BillAccount to release.</param>
        public void ReleaseBillAccount(BillAccount billAccount)
        {
            try
            {
                new BillAccountDataAccess().ReleaseBillAccount(billAccount);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Gets the next sequence number from the database for generating bill account numbers.
        /// </summary>
        /// <returns>The next sequence number from the database.</returns>
        private int GetNextSequenceNumberFromDataBase()
        {
            try
            {
                int nextSequenceNumberFromDataBase = new BillAccountDataAccess().GetNextSequenceNumber();
                return nextSequenceNumberFromDataBase;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }

        /// <summary>
        /// Generates a new bill account number.
        /// </summary>
        /// <returns>The generated bill account number.</returns>
        private string GenerateBillAccountNumber()
        {
            try
            {
                int nextSequenceNumber = this.GetNextSequenceNumberFromDataBase();
                string billAccountNumber = $"BA{nextSequenceNumber:D6}";
                return billAccountNumber;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }
        public BillAccountPolicy GetBillAccountPolicyById(int billAccountPolicyId)
        {
            return new BillAccountPolicyDataAccess().GetBillAccountPolicyById(billAccountPolicyId);
        }
    }
}
