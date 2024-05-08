using BillingSystemWebServices.Models;
using BillingSystemWebServices.ControllersHelper;
using BillingSystemServiceClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using BillingSystemDataModel;

namespace BillingSystemWebServices.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Payment/PaymentForm
        public ActionResult PaymentForm()
        {
            ViewModelPayment viewModelPayment = new ViewModelPayment();
            return View(viewModelPayment);
        }

        // POST: Payment/ApplyPayment
        [HttpPost]
        public async Task<ActionResult> ApplyPayment(ViewModelPayment viewModelPayment)
        {
            if (ModelState.IsValid)
            {
                // Convert ViewModelPayment to Payment model
                Payment payment = PaymentHelper.ConvertToPayment(viewModelPayment);

                // Call the ApplyPayment method from the service client
                bool isSuccess = await new PaymentServiceClient().ApplyPayment(payment);

                if (isSuccess)
                {
                    // Payment applied successfully
                    return Content("Payment applied successfully");
                }
                else
                {
                    // Payment application failed
                    ModelState.AddModelError("", "Failed to apply payment");
                    return View("PaymentForm", viewModelPayment);
                }
            }
            else
            {
                // Model state is not valid, return to the PaymentForm view with validation errors
                return View("PaymentForm", viewModelPayment);
            }
        }
    }
}
