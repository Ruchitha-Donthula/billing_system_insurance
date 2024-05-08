// <copyright file="InstallmentController.cs" company="PlaceholderCompany">
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
    /// Controller for managing installments.
    /// </summary>
    public class InstallmentController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InstallmentController));

        /// <summary>
        /// Creates installment schedule.
        /// </summary>
        /// <param name="request">The request object containing bill account, bill account policy, and premium.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/CreateInstallmentSchedule")]
        [HttpPost]
        public IHttpActionResult CreateInstallmentSchedule(CreateInstallmentScheduleRequest request)
        {
            try
            {
                if (request == null || request.BillAccount == null || request.BillAccountPolicy == null || request.Premium == 0.0)
                {
                    return this.BadRequest("Invalid bill account data");
                }

                new InstallmentBusiness().CreateInstallmentSchedule(request.BillAccount, request.BillAccountPolicy, request.Premium);
                return this.Ok("Installments scheduled successfully");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while creating installment schedule", ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Represents a request object containing a bill account, bill account policy, and premium.
        /// </summary>
        public class CreateInstallmentScheduleRequest
        {
            /// <summary>
            /// Gets or sets the bill account.
            /// </summary>
            public BillAccount BillAccount { get; set; }

            /// <summary>
            /// Gets or sets the bill account policy.
            /// </summary>
            public BillAccountPolicy BillAccountPolicy { get; set; }

            /// <summary>
            /// Gets or sets the premium.
            /// </summary>
            public double Premium { get; set; }
        }
    }
}
