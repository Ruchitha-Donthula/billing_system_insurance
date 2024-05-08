using BillingSystemWebServices.Models;
using BillingSystemDataModel;
using System;

namespace BillingSystemWebServices.ControllersHelper
{
    public class PaymentHelper
    {
        public static ViewModelPayment ConvertToViewModelPayment(Payment payment)
        {
            return new ViewModelPayment
            {
                PaymentId = payment.PaymentId,
                PaymentMethod = payment.PaymentMethod,
                PaymentFrom = payment.PaymentFrom,
                Amount = payment.Amount,
                BillAccountNumber = payment.BillAccountNumber,
                PaymentDate = payment.PaymentDate,
                InvoiceNumber = payment.InvoiceNumber,
                PaymentStatus = payment.PaymentStatus,
                PaymentReference = payment.PaymentReference,
                BillAccountId = payment.BillAccountId
            };
        }

        public static Payment ConvertToPayment(ViewModelPayment viewModelPayment)
        {
            return new Payment
            {
                PaymentId = viewModelPayment.PaymentId,
                PaymentMethod = viewModelPayment.PaymentMethod,
                PaymentFrom = viewModelPayment.PaymentFrom,
                Amount = viewModelPayment.Amount,
                BillAccountNumber = viewModelPayment.BillAccountNumber,
                PaymentDate = viewModelPayment.PaymentDate,
                InvoiceNumber = viewModelPayment.InvoiceNumber,
                PaymentStatus = viewModelPayment.PaymentStatus,
                PaymentReference = viewModelPayment.PaymentReference,
                BillAccountId = viewModelPayment.BillAccountId
            };
        }
    }
}
