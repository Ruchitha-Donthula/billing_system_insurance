using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemDataAccessTest
{
    public class PaymentDataAccessTest
    {

       public void TestGetPaymentById()
        {
            Console.WriteLine("Testing GetPaymentById:");

            // Assuming there's a Payment with Id = 1 in the database
            var payment = new PaymentDataAccess().GetPaymentById(1);

            if (payment != null)
            {
                Console.WriteLine($"Payment found: Id = {payment.PaymentId}, Amount = {payment.Amount}");
            }
            else
            {
                Console.WriteLine("Payment not found.");
            }
        }

       public void TestGetAllPayments()
        {
            Console.WriteLine("\nTesting GetAllPayments:");

            var payments = new PaymentDataAccess().GetAllPayments();

            if (payments.Count > 0)
            {
                Console.WriteLine("Payments found:");
                foreach (var payment in payments)
                {
                    Console.WriteLine($"Id = {payment.PaymentId}, Amount = {payment.Amount}");
                }
            }
            else
            {
                Console.WriteLine("No Payments found.");
            }
        }

       public void TestAddPayment()
        {
            Console.WriteLine("\nTesting AddPayment:");

            // Create a new Payment object
            var newPayment = new Payment
            {
                PaymentMethod =ApplicationConstants.BILL_ACCOUNT_CREDITCARD_PAYMENT_METHOD,
                PaymentFrom = "John Doe",
                Amount = 500.0,
                BillAccountNumber = "BA123456",
                PaymentDate = DateTime.Now,
                InvoiceNumber = "INV789",
                PaymentStatus = ApplicationConstants.PAYMENT_STATUS_SUCCESS,
                PaymentReference = "REF123456",
                BillAccountId = 3 // Assuming BillAccountId exists in the database
                // Add other properties as needed
            };

            // Add the new Payment
            new PaymentDataAccess().AddPayment(newPayment);
            Console.WriteLine("Payment added successfully.");
        }

        public void TestUpdatePayment()
        {
            Console.WriteLine("\nTesting UpdatePayment:");

            // Get an existing Payment by Id
            var payment = new PaymentDataAccess().GetPaymentById(1);

            if (payment != null)
            {
                // Update Payment properties
                payment.PaymentStatus = ApplicationConstants.PAYMENT_STATUS_PENDING;

                // Update the Payment
                new PaymentDataAccess().UpdatePayment(payment);
                Console.WriteLine("Payment updated successfully.");
            }
            else
            {
                Console.WriteLine("Payment not found.");
            }
        }

       public void TestDeletePayment()
        {
            Console.WriteLine("\nTesting DeletePayment:");

            // Assuming there's a Payment with Id = 1 in the database
            new PaymentDataAccess().DeletePayment(1);
            Console.WriteLine("Payment deleted successfully.");
        }
    }
}
