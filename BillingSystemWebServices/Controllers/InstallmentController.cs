using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using BillingSystemWebServices.Models;
using BillingSystemServiceClient;
using BillingSystemDataModel;
using static BillingSystemServices.Controllers.InstallmentController;




namespace BillingSystemWebServices.Controllers
{
    public class InstallmentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateInstallmentForm()
        {
            ViewModelCreateInstallmentRequestData viewModelCreateInstallmentRequestData = new ViewModelCreateInstallmentRequestData();
            return View("CreateInstallmentsForm", viewModelCreateInstallmentRequestData);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInstallments(ViewModelCreateInstallmentRequestData formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var billAccountId = formData.BillAccountId;
                    var billAccountPolicyId = formData.BillAccountPolicyId;
                    BillAccount billAccount = await new BillingSystemServiceClientClass().GetBillAccountById(billAccountId);
                    BillAccountPolicy billAccountPolicy = await new BillAccountPolicyServiceClient().GetBillAccountPolicyById(billAccountPolicyId);

                    // Create the request object
                    var request = new CreateInstallmentScheduleRequest
                    {
                        BillAccount = billAccount,
                        BillAccountPolicy = billAccountPolicy,
                        Premium = formData.Premium
                    };

                    // Call the service client method to create installments
                    bool isSuccess = await new InstallmentServiceClient().CreateInstallmentSchedule(request);

                    if (isSuccess)
                    {
                        return Content("Installments created successfully");
                    }
                    else
                    {
                        // Handle if creating installments fails
                        return View("CreateInstallmentsForm", formData);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    return Content($"An error occurred while creating installments: {ex.Message}");
                }
            }

            // If model state is not valid, return to the CreateInstallmentsForm view
            return View("CreateInstallmentsForm", formData);
        }

    }
}
