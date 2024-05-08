using BillingSystemServices.ControllersHelper;
using BillingSystemWebServices.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillingSystemServiceClient;
using System.Web.Mvc;
using BillingSystemDataModel;
using static BillingSystemServices.Controllers.BillAccountController;

namespace BillingSystemWebServices.Controllers
{
    public class BillAccountController : Controller
    {
        // GET: BillAccount/Index
        public ActionResult Index()
        {
            return View("Index"); // Display the Index view
        }

        // GET: BillAccount/AddBillAccountForm
        public ActionResult AddBillAccountForm()
        {
            var billAccount = new ViewModelBillAccount();
            return View("AddBillAccountForm", billAccount); // Display the AddBillAccountForm view

        }
        public ActionResult AddBillAccountPolicyForm()
        {
            var viewModelBillAccountPolicy = new ViewModelBillAccountPolicyData
            {
                PolicyNumber = "123",
                BillAccountId = 0,
                PayPlan = "Write a PayPlan",
            };

            if (viewModelBillAccountPolicy != null)
            {
                return View("AddBillAccountPolicyForm", viewModelBillAccountPolicy);
            }
            else
            {
                // Handle the case where the view model is null
                // For example, you could return a different view or redirect to an error page
                return View("Error");
            }
        }



        // POST: BillAccount/CreateBillAccount
        [HttpPost]
        public async Task<ActionResult> CreateBillAccount(ViewModelBillAccount billAccount)
        {
            if (ModelState.IsValid)
            {
                billAccount.Status = "Pending";
                billAccount.AccountBalance = 0.0;
                billAccount.AccountPaid = 0.0;
                billAccount.AccountTotal = 0.0;
                billAccount.FutureDue = 0.0;
                billAccount.PastDue = 0.0;
                billAccount.LastPaymentAmount = 0.0;
                try
                {
                    // Convert ViewModelBillAccount to DataModelBillAccount using BillAccountHelper
                    var dataModelBillAccount = BillAccountHelper.GetDataModelBillAccountFromViewModel(billAccount);

                    // Call service client method to create bill account
                    bool isSuccess = await new BillingSystemServiceClientClass().CreateBillAccount(dataModelBillAccount);

                    if (isSuccess)
                    {
                        return Content("Bill account created successfully");
                    }
                    else
                    {
                        // Handle if creation fails
                        return View("AddBillAccountForm", billAccount);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    return Content($"An error occurred while creating the bill account: {ex.Message}");
                }
            }

            // If model state is not valid, return to the AddBillAccountForm view with the model
            return View("AddBillAccountForm", billAccount);
        }


        [HttpPost]
        public async Task<ActionResult> AddBillAccountPolicy(ViewModelBillAccountPolicyData formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Extract data from the form model
                    var policyNumber = formData.PolicyNumber;
                    var billAccountId = formData.BillAccountId;
                    var payPlan = formData.PayPlan;

                    // Convert the single policy number into a list internally
                    List<string> policyNumbers = new List<string> { policyNumber };

                    // Create a new BillAccountAndPolicyRequest object
                    var request = new BillAccountAndPolicyRequest
                    {
                        BillAccount = new BillAccount { BillAccountId = billAccountId },
                        PolicyNumbers = policyNumbers,
                        Payplan = payPlan
                    };

                    // Call the service client method to add the bill account policy
                    bool isSuccess = await new BillingSystemServiceClientClass().AddBillAccountPolicy(request);

                    if (isSuccess)
                    {
                        return Content("Bill account policy added successfully");
                    }
                    else
                    {
                        // Handle if adding fails
                        return View("AddBillAccountPolicyForm");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    return Content($"An error occurred while adding the bill account policy: {ex.Message}");
                }
            }

            // If model state is not valid, return to the AddBillAccountPolicyForm view
            return View("AddBillAccountPolicyForm");
        }



    }

}


