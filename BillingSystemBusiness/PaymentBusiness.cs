// <copyright file="PaymentBusiness.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemBusiness
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BillingSystemDataAccess;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides business logic for applying payments.
    /// </summary>
    public class PaymentBusiness
    {
        /// <summary>
        /// Applies a payment to the billing system.
        /// </summary>
        /// <param name="payment">The payment to apply.</param>
        public void ApplyPayment(Payment payment)
        {
            try
            {
                new PaymentDataAccess().AddPayment(payment);

                BillAccount billAccount = new BillAccountDataAccess().GetBillAccountByNumber(payment.BillAccountNumber);
                Invoice invoice = new InvoiceDataAccess().GetInvoiceByNumber(payment.InvoiceNumber);

                if (billAccount != null)
                {
                    BillingTransaction billingTransaction = new BillingTransaction
                    {
                        BillAccountId = billAccount.BillAccountId,
                        ActivityDate = DateTime.Now.Date,
                        TransactionType = payment.PaymentMethod,
                        TransactionAmount = payment.Amount,
                        InvoiceId = invoice.InvoiceId,
                        PaymentId = payment.PaymentId,
                    };
                    new BillingTransactionDataAccess().AddBillingTransaction(billingTransaction);

                    this.UpdateBillAccount(payment, billAccount, invoice);

                    this.UpdateInstallments(payment);
                }
                else
                {
                    throw new Exception("BillAccount could not be found. Invalid BillAccount Number present on payment. This is a case of Unallocated Cash");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ApplyPaymentFailed: " + ex.Message);
                throw new Exception("ApplyPaymentFailed", ex);
            }
        }

        /// <summary>
        /// Updates the bill account after applying a payment.
        /// </summary>
        /// <param name="payment">The payment applied.</param>
        /// <param name="billAccount">The bill account to update.</param>
        /// <param name="invoice">The invoice associated with the payment.</param>
        private void UpdateBillAccount(Payment payment, BillAccount billAccount, Invoice invoice)
        {
            try
            {
                double newAmountPaid = (double)(billAccount.AccountPaid + payment.Amount);
                double newBalanceAmount = (double)(billAccount.AccountTotal - newAmountPaid);

                billAccount.LastPaymentDate = DateTime.Now;
                billAccount.LastPaymentAmount = payment.Amount;

                double pastDue = 0;
                if (newBalanceAmount > 0)
                {
                    pastDue = invoice.InvoiceAmount - newAmountPaid;
                }

                double futureDue = this.GetTotalFutureDueAmount(billAccount);

                billAccount.PastDue = pastDue;
                billAccount.FutureDue = futureDue;

                billAccount.AccountPaid = newAmountPaid;
                billAccount.AccountBalance = newBalanceAmount;

                new BillAccountDataAccess().UpdateBillAccount(billAccount);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating bill account.", ex);
            }
        }

        /// <summary>
        /// Updates the installments associated with the payment.
        /// </summary>
        /// <param name="payment">The payment applied.</param>
        private void UpdateInstallments(Payment payment)
        {
            try
            {
                List<Installment> installmentsToUpdate = new InstallmentDataAccess().GetInstallmentsByInvoiceNumber(payment.InvoiceNumber);

                double totalDueAmount = installmentsToUpdate.Sum(installment => installment.DueAmount);

                foreach (var installment in installmentsToUpdate)
                {
                    double ratio = installment.DueAmount / totalDueAmount;

                    double installmentPayment = payment.Amount * ratio;
                    double newPaidAmount = (double)(installment.PaidAmount + installmentPayment);
                    installment.PaidAmount = newPaidAmount;

                    double newBalanceAmount = installment.DueAmount - newPaidAmount;
                    installment.BalanceAmount = newBalanceAmount;

                    new InstallmentDataAccess().UpdateInstallment(installment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating installments.", ex);
            }
        }

        /// <summary>
        /// Calculates the total future due amount for the bill account.
        /// </summary>
        /// <param name="billAccount">The bill account to calculate the future due amount for.</param>
        /// <returns>The total future due amount.</returns>
        private double GetTotalFutureDueAmount(BillAccount billAccount)
        {
            try
            {
                List<Installment> futureInstallments = new InstallmentDataAccess().GetFutureInstallmentsByBillAccountId(billAccount.BillAccountId);

                double totalFutureDue = futureInstallments.Sum(installment => installment.DueAmount);

                return totalFutureDue;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while calculating total future due amount.", ex);
            }
        }
    }
}
