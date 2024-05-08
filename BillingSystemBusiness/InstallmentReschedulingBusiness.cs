// <copyright file="InstallmentReschedulingBusiness.cs" company="PlaceholderCompany">
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
    /// Provides methods for rescheduling installments when there are changes to a bill account.
    /// </summary>
    public class InstallmentReschedulingBusiness
    {
        /// <summary>
        /// Updates the due day of a bill account and reschedules installments accordingly.
        /// </summary>
        /// <param name="billAccount">The bill account for which to update the due day.</param>
        /// <param name="newDueDay">The new due day to be set.</param>
        public void OnChangeOfBillAccountDueDay(BillAccount billAccount, int newDueDay)
        {
            try
            {
                // Update the due day in the BillAccount
                BillAccount updatedBillAccount = new BillAccountDataAccess().GetBillAccountById(billAccount.BillAccountId);
                updatedBillAccount.DueDay = newDueDay;
                new BillAccountDataAccess().UpdateBillAccount(updatedBillAccount);

                // Retrieve all old InstallmentSummaries associated with the given BillAccount
                List<InstallmentSummary> oldSummaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(updatedBillAccount.BillAccountId);

                // Mark old InstallmentSummaries as inactive and create new summaries with the updated due day
                foreach (var oldSummary in oldSummaries)
                {
                    oldSummary.Status = ApplicationConstants.INSTALLMENT_SUMMARY_INACTIVE_STATUS;
                    new InstallmentDataAccess().UpdateInstallmentSummary(oldSummary);

                    // Create new InstallmentSummary
                    InstallmentSummary newSummary = new InstallmentSummary
                    {
                        PolicyNumber = oldSummary.PolicyNumber,
                        Status = ApplicationConstants.INSTALLMENT_SUMMARY_ACTIVE_STATUS,
                        BillAccountId = oldSummary.BillAccountId,
                    };
                    new InstallmentDataAccess().AddInstallmentSummary(newSummary);

                    // Copy billed Installments to the new InstallmentSummary and reschedule pending Installments
                    this.RescheduleInstallments(oldSummary, newSummary, newDueDay);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during the change of BillAccount due day.", ex);
            }
        }

        /// <summary>
        /// Reschedules installments based on a change in pay plan for a bill account.
        /// </summary>
        /// <param name="billAccount">The bill account for which the pay plan is changing.</param>
        /// <param name="billAccountPolicy">The new pay plan to be applied.</param>
        public void OnChangeOfPolicyPayPlan(BillAccount billAccount, BillAccountPolicy billAccountPolicy)
        {
            new BillAccountPolicyDataAccess().UpdateBillAccountPolicy(billAccountPolicy);
            InstallmentSummary newInstallmentSummary = this.CreateNewInstallmentSummary(billAccount, billAccountPolicy);

            new InstallmentDataAccess().AddInstallmentSummary(newInstallmentSummary);

            this.CopyPaidInstallmentsToNewSchedule(billAccount, billAccountPolicy, newInstallmentSummary);

            double updatedPremium = this.GetRemainingPremium(billAccount, billAccountPolicy);

            // Add remaining installments based on the new pay plan
            this.AddRemainingInstallmentsToNewSchedule(newInstallmentSummary, billAccountPolicy, updatedPremium, billAccount.DueDay);

            // Mark the previous installment schedule as 'Expired'
            this.MarkPreviousScheduleAsExpired(billAccount, billAccountPolicy);
        }

        /// <summary>
        /// Calculates the premium amount remaining to be scheduled based on the new pay plan.
        /// </summary>
        /// <param name="billAccount">The bill account associated with the pay plan.</param>
        /// <param name="newPayPlan">The new pay plan to be applied.</param>
        /// <returns>The remaining premium amount.</returns>
        private double GetRemainingPremium(BillAccount billAccount, BillAccountPolicy newPayPlan)
        {
            List<InstallmentSummary> installmentSummaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(billAccount.BillAccountId);
            InstallmentSummary oldSummary = installmentSummaries.FirstOrDefault(summary => summary.PolicyNumber == newPayPlan.PolicyNumber);
            List<Installment> pendingInstallments = new InstallmentDataAccess().GetPendingInstallmentsBySummaryId(oldSummary.InstallmentSummaryId);
            double updatedPremium = 0.0;
            foreach (var pendingInstallment in pendingInstallments)
            {
                updatedPremium += (double)pendingInstallment.BalanceAmount;
            }

            return updatedPremium;
        }

        /// <summary>
        /// Creates a new installment summary based on the updated pay plan.
        /// </summary>
        /// <param name="billAccount">The bill account associated with the pay plan.</param>
        /// <param name="newPayPlan">The new pay plan to be applied.</param>
        /// <returns>The new installment summary.</returns>
        private InstallmentSummary CreateNewInstallmentSummary(BillAccount billAccount, BillAccountPolicy newPayPlan)
        {
            return new InstallmentSummary
            {
                PolicyNumber = newPayPlan.PolicyNumber,
                Status = ApplicationConstants.INSTALLMENT_SUMMARY_ACTIVE_STATUS,
                BillAccountId = billAccount.BillAccountId,
            };
        }

        /// <summary>
        /// Copies paid installments from the old schedule to the new schedule.
        /// </summary>
        /// <param name="billAccount">The bill account associated with the old schedule.</param>
        /// <param name="newPayPlan">The new pay plan to be applied.</param>
        /// <param name="newInstallmentSummary">The new installment summary to copy installments to.</param>
        private void CopyPaidInstallmentsToNewSchedule(BillAccount billAccount, BillAccountPolicy newPayPlan, InstallmentSummary newInstallmentSummary)
        {
            // Retrieve paid installments from the old schedule
            List<InstallmentSummary> installmentSummaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(billAccount.BillAccountId);
            InstallmentSummary oldSummary = installmentSummaries.FirstOrDefault(summary => summary.PolicyNumber == newPayPlan.PolicyNumber);

            List<Installment> billedInstallments = new InstallmentDataAccess().GetBilledInstallmentsBySummaryId(oldSummary.InstallmentSummaryId);

            // Copy paid installments to the new schedule
            foreach (var billedInstallment in billedInstallments)
            {
                billedInstallment.InstallmentSummaryId = newInstallmentSummary.InstallmentSummaryId;
                new InstallmentDataAccess().AddInstallment(billedInstallment);
            }
        }

        /// <summary>
        /// Adds remaining installments based on the new pay plan to the new schedule.
        /// </summary>
        /// <param name="newInstallmentSummary">The new installment summary to add installments to.</param>
        /// <param name="premium">The remaining premium amount.</param>
        /// <param name="dueDay">The due day for the new installments.</param>
        private void AddRemainingInstallmentsToNewSchedule(InstallmentSummary newInstallmentSummary, BillAccountPolicy billAccountPolicy, double premium, int dueDay)
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Check if there are any billed installments for the current month
            bool billedThisMonth = new InstallmentDataAccess().GetBilledInstallmentsBySummaryId(newInstallmentSummary.InstallmentSummaryId)
                .Any(installment => installment.InstallmentDueDate.Year == currentDate.Year && installment.InstallmentDueDate.Month == currentDate.Month);

            // Generate and add remaining installments based on the new pay plan
            List<Installment> remainingInstallments = new InstallmentBusiness().GenerateInstallments(newInstallmentSummary, billAccountPolicy.PayPlan, premium, dueDay, billedThisMonth);
            foreach (var installment in remainingInstallments)
            {
                new InstallmentDataAccess().AddInstallment(installment);
            }
        }

        /// <summary>
        /// Marks the previous installment schedule as 'Expired'.
        /// </summary>
        /// <param name="billAccount">The bill account associated with the old schedule.</param>
        /// <param name="newPayPlan">The new pay plan to be applied.</param>
        private void MarkPreviousScheduleAsExpired(BillAccount billAccount, BillAccountPolicy newPayPlan)
        {
            // Update the status of the previous installment summary to 'Expired'
            List<InstallmentSummary> installmentSummaries = new InstallmentDataAccess().GetInstallmentSummariesByBillAccountId(billAccount.BillAccountId);
            InstallmentSummary oldSummary = installmentSummaries.FirstOrDefault(summary => summary.PolicyNumber == newPayPlan.PolicyNumber);

            oldSummary.Status = ApplicationConstants.INSTALLMENT_SUMMARY_INACTIVE_STATUS;
            new InstallmentDataAccess().UpdateInstallmentSummary(oldSummary);
        }

        private void RescheduleInstallments(InstallmentSummary oldSummary, InstallmentSummary newSummary, int newDueDay)
        {
            // Retrieve billed Installments associated with the old InstallmentSummary
            List<Installment> billedInstallments = new InstallmentDataAccess().GetBilledInstallmentsBySummaryId(oldSummary.InstallmentSummaryId);

            // Copy billed Installments to the new InstallmentSummary
            foreach (var billedInstallment in billedInstallments)
            {
                billedInstallment.InstallmentSummaryId = newSummary.InstallmentSummaryId;
                new InstallmentDataAccess().AddInstallment(billedInstallment);
            }

            // Retrieve pending Installments associated with the old InstallmentSummary
            List<Installment> pendingInstallments = new InstallmentDataAccess().GetPendingInstallmentsBySummaryId(oldSummary.InstallmentSummaryId);

            // Reschedule pending Installments with the new due day
            foreach (var pendingInstallment in pendingInstallments)
            {
                string payplan = new BillAccountPolicyDataAccess().GetPayPlanByPolicyNumber(oldSummary.PolicyNumber);
                pendingInstallment.InstallmentDueDate = new InstallmentBusiness().CalculateDueDate(pendingInstallment.InstallmentSequenceNumber, payplan, newDueDay);
                pendingInstallment.InstallmentSendDate = new InstallmentBusiness().CalculateSendDate(pendingInstallment.InstallmentDueDate);
                pendingInstallment.InvoiceStatus = ApplicationConstants.INSTALLMENT_INVOICE_STATUS_PENDING;
                pendingInstallment.InstallmentSummaryId = newSummary.InstallmentSummaryId;
                new InstallmentDataAccess().AddInstallment(pendingInstallment);
            }
        }
    }
}
