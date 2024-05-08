using System;
using System.Web.Http;
using BillingSystemBusiness;
using BillingSystemDataModel;
using log4net;

namespace BillingSystemServices.Controllers
{
    /// <summary>
    /// Controller for managing installment rescheduling.
    /// </summary>
    public class InstallmentReschedulingController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InstallmentReschedulingController));

        /// <summary>
        /// Updates the due day of a bill account and reschedules installments accordingly.
        /// </summary>
        /// <param name="request">The request object containing the bill account and new due day.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [Route("api/OnChangeOfBillAccountDueDay")]
        public IHttpActionResult OnChangeOfDueDay(RescheduleInstallmentsDueDayRequest request)
        {
            try
            {
                if (request == null || request.BillAccount == null || request.NewDueDay == 0)
                {
                    return BadRequest("Invalid request data");
                }

                new InstallmentReschedulingBusiness().OnChangeOfBillAccountDueDay(request.BillAccount, request.NewDueDay);
                return Ok("Installments rescheduled successfully for due day change");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while rescheduling installments for due day change", ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Updates the pay plan of a bill account and reschedules installments accordingly.
        /// </summary>
        /// <param name="request">The request object containing the bill account and new pay plan.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [Route("api/OnChangeOfPolicyPayPlan")]
        public IHttpActionResult RescheduleInstallmentsPayPlan(RescheduleInstallmentsPayPlanRequest request)
        {
            try
            {
                if (request == null || request.BillAccount == null || request.BillAccountPolicy == null)
                {
                    return BadRequest("Invalid request data");
                }

                new InstallmentReschedulingBusiness().OnChangeOfPolicyPayPlan(request.BillAccount, request.BillAccountPolicy);
                return Ok("Installments rescheduled successfully for pay plan change");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while rescheduling installments for pay plan change", ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Represents a request object containing a bill account and new due day.
        /// </summary>
        public class RescheduleInstallmentsDueDayRequest
        {
            /// <summary>
            /// Gets or sets the bill account.
            /// </summary>
            public BillAccount BillAccount { get; set; }

            /// <summary>
            /// Gets or sets the new due day.
            /// </summary>
            public int NewDueDay { get; set; }
        }

        /// <summary>
        /// Represents a request object containing a bill account and new pay plan.
        /// </summary>
        public class RescheduleInstallmentsPayPlanRequest
        {
            /// <summary>
            /// Gets or sets the bill account.
            /// </summary>
            public BillAccount BillAccount { get; set; }

            /// <summary>
            /// Gets or sets the new pay plan.
            /// </summary>
            public BillAccountPolicy BillAccountPolicy { get; set; }
        }
    }
}
