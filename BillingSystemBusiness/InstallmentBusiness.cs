// <copyright file="InstallmentBusiness.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BillingSystemBusiness
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using BillingSystemDataAccess;
    using BillingSystemDataModel;

    /// <summary>
    /// Provides business logic for managing installment schedules.
    /// </summary>
    public class InstallmentBusiness
    {
        /// <summary>
        /// Creates an installment schedule for a bill account based on the provided premium amount and payment plan.
        /// </summary>
        /// <param name="billAccount">The bill account for which to create the installment schedule.</param>
        /// <param name="billAccountPolicy">The policy associated with the bill account.</param>
        /// <param name="premium">The premium amount to be split into installments.</param>
        public void CreateInstallmentSchedule(BillAccount billAccount, BillAccountPolicy billAccountPolicy, double premium)
        {
            try
            {
                BillAccount billaccount = new BillAccountDataAccess().GetBillAccountById(billAccount.BillAccountId);
                if (billaccount == null)
                {
                    throw new Exception("Bill account not found.");
                }

                this.InitializeBillAccount(billaccount, premium);

                InstallmentSummary parentRecord = new InstallmentSummary
                {
                    PolicyNumber = billAccountPolicy.PolicyNumber,
                    Status = ApplicationConstants.INSTALLMENT_SUMMARY_ACTIVE_STATUS,
                    BillAccountId = billaccount.BillAccountId,
                };
                new InstallmentDataAccess().AddInstallmentSummary(parentRecord);

                List<Installment> installments = this.GenerateInstallments(parentRecord, billAccountPolicy.PayPlan, premium, billaccount.DueDay);
                foreach (var installment in installments)
                {
                    new InstallmentDataAccess().AddInstallment(installment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating installment schedule.", ex);
            }
        }

        /// <summary>
        /// Generates a list of installments based on the provided payment plan and premium amount.
        /// </summary>
        /// <param name="parentRecord">The parent installment summary record.</param>
        /// <param name="payPlan">The payment plan for the installments.</param>
        /// <param name="premium">The premium amount to be split into installments.</param>
        /// <param name="dueDay">The due day for each installment.</param>
        /// <param name="billedThisMonth">Billing done this month or not.</param>
        /// <returns>The list of generated installments.</returns>
        public List<Installment> GenerateInstallments(InstallmentSummary parentRecord, string payPlan, double premium, int dueDay, bool billedThisMonth = false)
        {
            try
            {
                List<Installment> installments = new List<Installment>();
                Installment installment;
                switch (payPlan)
                {
                    case "Monthly":
                        double monthlyPremium = premium / 12;
                        for (int installmentNumber = 1; installmentNumber <= 12; installmentNumber++)
                        {
                            installment = this.CreateInstallment(parentRecord, installmentNumber, monthlyPremium, payPlan, dueDay, billedThisMonth);
                            installments.Add(installment);
                        }

                        break;
                    case "Quarterly":
                        double quarterlyPremium = premium / 4;
                        for (int installmentNumber = 1; installmentNumber <= 4; installmentNumber++)
                        {
                            installment = this.CreateInstallment(parentRecord, installmentNumber, quarterlyPremium, payPlan, dueDay, billedThisMonth);
                            installments.Add(installment);
                        }

                        break;
                    case "Semiannual":
                        double semiannualPremium = premium / 2;
                        for (int installmentNumber = 1; installmentNumber <= 2; installmentNumber++)
                        {
                            installment = this.CreateInstallment(parentRecord, installmentNumber, semiannualPremium, payPlan, dueDay, billedThisMonth);
                            installments.Add(installment);
                        }

                        break;
                    case "Annual":
                        double annualPremium = premium;
                        installment = this.CreateInstallment(parentRecord, 1, annualPremium, payPlan, dueDay, billedThisMonth);
                        installments.Add(installment);
                        break;
                    default:
                        break;
                }

                return installments;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating installments.", ex);
            }
        }

        /// <summary>
        /// Calculates the send date for an installment based on its due date.
        /// </summary>
        /// <param name="installmentDueDate">The due date of the installment.</param>
        /// <returns>The calculated send date for the installment.</returns>
        public DateTime CalculateSendDate(DateTime installmentDueDate)
        {
            // senddate = due date - send date buffer days
            int send_date_buffer_days = Convert.ToInt32(ConfigurationManager.AppSettings["Send_date_buffer_days"]);
            return installmentDueDate.AddDays(-send_date_buffer_days);
        }

        /// <summary>
        /// Calculates the due date for an installment based on its sequence number, payment plan, and due day.
        /// </summary>
        /// <param name="installmentNumber">The sequence number of the installment.</param>
        /// <param name="payPlan">The payment plan for the installment.</param>
        /// <param name="dueDay">The due day for each installment.</param>
        /// <param name="billedThisMonth">Billing done this month or not.</param>
        /// <returns>The calculated due date for the installment.</returns>
        public DateTime CalculateDueDate(int installmentNumber, string payPlan, int dueDay, bool billedThisMonth = false)
        {
            try
            {
                DateTime dueDate;
                DateTime currentDate = DateTime.Now;


                // Check if the current date is after the due day
                if (currentDate.Day > dueDay || billedThisMonth)
                {
                    // If so, schedule the first installment for the next month
                    currentDate = currentDate.AddMonths(1);
                }

                // Calculate due date based on the adjusted current date
                switch (payPlan)
                {
                    case "Monthly":
                        dueDate = new DateTime(currentDate.Year, currentDate.Month, dueDay).AddMonths(installmentNumber - 1);
                        break;
                    case "Quarterly":
                        dueDate = new DateTime(currentDate.Year, currentDate.Month, dueDay).AddMonths((installmentNumber - 1) * 3);
                        break;
                    case "Semiannual":
                        dueDate = new DateTime(currentDate.Year, currentDate.Month, dueDay).AddMonths((installmentNumber - 1) * 6);
                        break;
                    case "Annual":
                        dueDate = new DateTime(currentDate.Year, currentDate.Month, dueDay).AddYears(installmentNumber - 1);
                        break;
                    default:
                        throw new ArgumentException("Invalid pay plan specified.");
                }

                return dueDate;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while calculating due date.", ex);
            }
        }

        /// <summary>
        /// Initializes the bill account with the premium amount.
        /// </summary>
        /// <param name="billAccount">The bill account to initialize.</param>
        /// <param name="premium">The premium amount to be added to the bill account.</param>
        public void InitializeBillAccount(BillAccount billAccount, double premium)
        {
            try
            {
                if (billAccount == null)
                {
                    throw new ArgumentNullException(nameof(billAccount), "Bill account cannot be null.");
                }

                double accountTotal = (double)(billAccount.AccountTotal + premium);
                billAccount.AccountTotal = accountTotal;
                billAccount.AccountBalance = billAccount.AccountTotal - billAccount.AccountPaid;
                new BillAccountDataAccess().UpdateBillAccount(billAccount);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while initializing bill account.", ex);
            }
        }

        /// <summary>
        /// Creates a single installment record.
        /// </summary>
        /// <param name="parentRecord">The parent installment summary record.</param>
        /// <param name="installmentNumber">The sequence number of the installment.</param>
        /// <param name="dueAmount">The due amount for the installment.</param>
        /// <param name="payPlan">The payment plan for the installment.</param>
        /// <param name="dueDay">The due day for the installment.</param>
        /// <returns>The created installment record.</returns>
        private Installment CreateInstallment(InstallmentSummary parentRecord, int installmentNumber, double dueAmount, string payPlan, int dueDay, bool billedThisMonth)
        {
            try
            {
                DateTime installmentDueDate = this.CalculateDueDate(installmentNumber, payPlan, dueDay, billedThisMonth);
                DateTime installmentSendDate = this.CalculateSendDate(installmentDueDate);
                return new Installment
                {
                    InstallmentSequenceNumber = installmentNumber,
                    InstallmentDueDate = installmentDueDate,
                    InstallmentSendDate = installmentSendDate,
                    DueAmount = dueAmount,
                    PaidAmount = 0.0,
                    BalanceAmount = dueAmount,
                    InvoiceStatus = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_PENDING,
                    InstallmentSummaryId = parentRecord.InstallmentSummaryId,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating installment.", ex);
            }
        }
     }
}
