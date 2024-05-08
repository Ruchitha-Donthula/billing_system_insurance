// <copyright file="BillAccountController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using BillingSystemBusiness;
    using BillingSystemDataModel;
    using BillingSystemServices.Filters;
    using log4net;

    /// <summary>
    /// Controller for managing bill accounts.
    /// </summary>
    public class BillAccountController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BillAccountController));

        /// <summary>
        /// Creates a new bill account.
        /// </summary>
        /// <param name="billAccount">The bill account data to create.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [HttpPost]
        [Route("api/CreateBillAccount")]
        public IHttpActionResult CreateBillAccount(BillAccount billAccount)
        {
            try
            {
                if (billAccount == null)
                {
                    return this.BadRequest("Invalid bill account data");
                }

                new BillAccountBusiness().CreateBillAccount(billAccount);
                return this.Ok("BillAccount added successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error("An error occurred while creating bill account", ex);

                // Return an Internal Server Error response
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Associates a bill account with one or more policies.
        /// </summary>
        /// <param name="request">The request object containing bill account, policy numbers, and pay plan.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/AssociateBillAccountWithPolicy")]
        [HttpPost]
        public IHttpActionResult AssociateBillAccountWithPolicy(BillAccountAndPolicyRequest request)
        {
            if (request == null || request.BillAccount == null || request.PolicyNumbers == null || string.IsNullOrEmpty(request.Payplan))
            {
                return this.BadRequest("One or more parameters are null or empty.");
            }

            new BillAccountBusiness().AssociateBillAccountWithPolicy(request.BillAccount, request.PolicyNumbers, request.Payplan);
            return this.Ok("Bill account associated with policies successfully.");
        }

        /// <summary>
        /// Retrieves a bill account by its ID.
        /// </summary>
        /// <param name="billAccountId">The ID of the bill account to retrieve.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/GetBillAccountById")]
        [HttpGet]
        public IHttpActionResult GetBillAccountById(int billAccountId)
        {
            try
            {
                var billAccount = new BillAccountBusiness().GetBillAccountById(billAccountId);
                if (billAccount == null)
                {
                    return this.NotFound();
                }

                return this.Json(billAccount);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while retrieving the bill account by ID", ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Retrieves a bill account by its number.
        /// </summary>
        /// <param name="billAccountNumber">The number of the bill account to retrieve.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/GetBillAccountByNumber")]
        [HttpGet]
        public IHttpActionResult GetBillAccountByNumber(string billAccountNumber)
        {
            try
            {
                var billAccount = new BillAccountBusiness().GetBillAccountByNumber(billAccountNumber);
                if (billAccount == null)
                {
                    return this.NotFound();
                }

                return this.Json(billAccount);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while retrieving the bill account by number", ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Updates a bill account.
        /// </summary>
        /// <param name="billAccount">The bill account data to update.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/UpdateBillAccount")]
        [HttpPost]
        public IHttpActionResult UpdateBillAccount(BillAccount billAccount)
        {
            try
            {
                if (billAccount == null)
                {
                    return this.BadRequest("Invalid bill account data");
                }

                new BillAccountBusiness().UpdateBillAccount(billAccount);
                return this.Ok("Bill account updated successfully");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while updating the bill account", ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Suspends a bill account.
        /// </summary>
        /// <param name="billAccount">The bill account to suspend.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/SuspendBillAccount")]
        [HttpPost]
        public IHttpActionResult SuspendBillAccount(BillAccount billAccount)
        {
            try
            {
                if (billAccount == null)
                {
                    return this.BadRequest("Invalid bill account data");
                }

                new BillAccountBusiness().SuspendBillAccount(billAccount);
                return this.Ok("Bill account suspended successfully");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while suspending the bill account", ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Releases a suspended bill account.
        /// </summary>
        /// <param name="billAccount">The bill account to release.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/ReleaseBillAccount")]
        [HttpPost]
        public IHttpActionResult ReleaseBillAccount(BillAccount billAccount)
        {
            try
            {
                if (billAccount == null)
                {
                    return this.BadRequest("Invalid bill account data");
                }

                new BillAccountBusiness().ReleaseBillAccount(billAccount);
                return this.Ok("Bill account released successfully");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while releasing the bill account", ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Represents a request object containing a bill account, policy numbers, and pay plan.
        /// </summary>
        public class BillAccountAndPolicyRequest
        {
            /// <summary>
            /// Gets or sets billAccount.
            /// </summary>
            public BillAccount BillAccount { get; set; }

            /// <summary>
            /// Gets or sets policyNumbers.
            /// </summary>
            public List<string> PolicyNumbers { get; set; }

            /// <summary>
            /// Gets or sets payplan.
            /// </summary>
            public string Payplan { get; set; }
        }

        [Route("api/GetBillAccountPolicyById")]
        [HttpGet]
        public IHttpActionResult GetBillAccountPolicyById(int billAccountPolicyId)
        {
            var billAccountPolicy = new BillAccountBusiness().GetBillAccountPolicyById(billAccountPolicyId);
            if (billAccountPolicy == null)
            {
                return NotFound();
            }
            return Json(billAccountPolicy);
        }
    }
}
