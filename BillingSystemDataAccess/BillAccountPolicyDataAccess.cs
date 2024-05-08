// <copyright file="BillAccountPolicyDataAccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides data access methods for interacting with BillAccountPolicy entities.
    /// </summary>
    public class BillAccountPolicyDataAccess
    {
        private readonly BillingSystemEDMContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BillAccountPolicyDataAccess"/> class.
        /// </summary>
        public BillAccountPolicyDataAccess()
        {
            this.context = new BillingSystemEDMContainer();
        }

        /// <summary>
        /// Retrieves a BillAccountPolicy entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the BillAccountPolicy to retrieve.</param>
        /// <returns>The BillAccountPolicy entity corresponding to the provided ID, or null if not found.</returns>
        public BillAccountPolicy GetBillAccountPolicyById(int id)
        {
            try
            {
                return this.context.BillAccountPolicies.FirstOrDefault(b => b.BillAccountPolicyId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving BillAccountPolicy by Id.", ex);
            }
        }

        /// <summary>
        /// Retrieves a BillAccountPolicy entity by its Number.
        /// </summary>
        /// <param name="policyNumber">The Number of the BillAccountPolicy to retrieve.</param>
        /// <returns>The BillAccountPolicy entity corresponding to the provided ID, or null if not found.</returns>
        public BillAccountPolicy GetBillAccountPolicyByNumber(string policyNumber)
        {
            try
            {
                return this.context.BillAccountPolicies.FirstOrDefault(b => b.PolicyNumber == policyNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving BillAccountPolicy by PolicyNumber.", ex);
            }
        }

        /// <summary>
        /// Retrieves all BillAccountPolicy entities.
        /// </summary>
        /// <returns>A list of all BillAccountPolicy entities.</returns>
        public List<BillAccountPolicy> GetAllBillAccountPolicies()
        {
            try
            {
                return this.context.BillAccountPolicies.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all BillAccountPolicies.", ex);
            }
        }

        /// <summary>
        /// Adds a new BillAccountPolicy entity.
        /// </summary>
        /// <param name="billAccountPolicy">The BillAccountPolicy entity to add.</param>
        public void AddBillAccountPolicy(BillAccountPolicy billAccountPolicy)
        {
            try
            {
                this.context.BillAccountPolicies.Add(billAccountPolicy);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding BillAccountPolicy.", ex);
            }
        }

        /// <summary>
        /// Deletes a BillAccountPolicy entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the BillAccountPolicy to delete.</param>
        public void DeleteBillAccountPolicy(int id)
        {
            try
            {
                var billAccountPolicy = this.context.BillAccountPolicies.Find(id);
                if (billAccountPolicy != null)
                {
                    this.context.BillAccountPolicies.Remove(billAccountPolicy);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting BillAccountPolicy.", ex);
            }
        }

        /// <summary>
        /// To get Payplan by PolicyNumber.
        /// </summary>
        /// <param name="policyNumber">PolicyNumber.</param>
        /// <returns>payplan.</returns>
        public string GetPayPlanByPolicyNumber(string policyNumber)
        {
            try
            {
                using (var context = new BillingSystemEDMContainer())
                {
                    var policy = context.BillAccountPolicies.FirstOrDefault(p => p.PolicyNumber == policyNumber);
                    if (policy != null)
                    {
                        return policy.PayPlan;
                    }
                    else
                    {
                        // Handle case where policy number is not found
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting pay plan by policy number.", ex);
            }
        }

        /// <summary>
        /// Updates an existing BillAccountPolicy entity.
        /// </summary>
        /// <param name="billAccountPolicy">The BillAccountPolicy entity with updated values.</param>
        /// <returns>The updated BillAccountPolicy entity.</returns>
        public BillAccountPolicy UpdateBillAccountPolicy(BillAccountPolicy billAccountPolicy)
        {
            try
            {
                var existingPolicy = this.context.BillAccountPolicies.Find(billAccountPolicy.BillAccountPolicyId);
                if (existingPolicy != null)
                {
                    // Update properties of the existing policy with values from the provided policy
                    existingPolicy.PolicyNumber = billAccountPolicy.PolicyNumber;
                    existingPolicy.PayPlan = billAccountPolicy.PayPlan;

                    // Update other properties as needed
                    this.context.SaveChanges(); // Save changes to the database
                }

                return existingPolicy;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating BillAccountPolicy.", ex);
            }
        }
    }
}
