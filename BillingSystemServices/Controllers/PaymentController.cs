// <copyright file="PaymentController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemServices.Controllers
{
    using System;
    using System.Web.Http;
    using BillingSystemBusiness;
    using BillingSystemDataModel;
    using BillingSystemServices.Filters;
    using log4net;

    /// <summary>
    /// Controller for managing payments.
    /// </summary>
    public class PaymentController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PaymentController));

        /// <summary>
        /// Applies a payment to the specified account.
        /// </summary>
        /// <param name="payment">The payment to apply.</param>
        /// <returns>IHttpActionResult.</returns>
        [RequestResponseLoggingFilter]
        [Route("api/ApplyPayment")]
        [HttpPost]
        public IHttpActionResult ApplyPayment(Payment payment)
        {
            try
            {
                if (payment == null)
                {
                    return this.BadRequest("Invalid payment data");
                }

                new PaymentBusiness().ApplyPayment(payment);
                return this.Ok("Payment applied successfully");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                Log.Error("An error occurred while applying payment", ex);

                // Return an Internal Server Error response
                return this.InternalServerError(ex);
            }
        }
    }
}
