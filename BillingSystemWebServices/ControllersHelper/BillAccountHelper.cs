using BillingSystemWebServices.Models;
using BillingSystemDataModel; // Make sure to import the correct namespace for your data model

namespace BillingSystemServices.ControllersHelper
{
    public class BillAccountHelper
    {
        // Method to convert ViewModelBillAccount to DataModelBillAccount
        public static BillAccount GetDataModelBillAccountFromViewModel(ViewModelBillAccount viewModel)
        {
            var dataModel = new BillAccount
            {
                // Map properties from ViewModel to DataModel
                BillAccountId = viewModel.BillAccountId,
                BillAccountNumber = viewModel.BillAccountNumber,
                BillingType = viewModel.BillingType,
                Status = viewModel.Status,
                PayorName = viewModel.PayorName,
                PayorAddress = viewModel.PayorAddress,
                PaymentMethod = viewModel.PaymentMethod,
                DueDay = viewModel.DueDay,
                AccountTotal = viewModel.AccountTotal,
                AccountPaid = viewModel.AccountPaid,
                AccountBalance = viewModel.AccountBalance,
                LastPaymentDate = viewModel.LastPaymentDate,
                LastPaymentAmount = viewModel.LastPaymentAmount,
                PastDue = viewModel.PastDue,
                FutureDue = viewModel.FutureDue
            };

            return dataModel;
        }

        // Method to convert DataModelBillAccount to ViewModelBillAccount
        public static ViewModelBillAccount GetViewModelBillAccountFromDataModel(BillAccount dataModel)
        {
            var viewModel = new ViewModelBillAccount
            {
                // Map properties from DataModel to ViewModel
                BillAccountId = dataModel.BillAccountId,
                BillAccountNumber = dataModel.BillAccountNumber,
                BillingType = dataModel.BillingType,
                Status = dataModel.Status,
                PayorName = dataModel.PayorName,
                PayorAddress = dataModel.PayorAddress,
                PaymentMethod = dataModel.PaymentMethod,
                DueDay = dataModel.DueDay,
                AccountTotal = dataModel.AccountTotal,
                AccountPaid = dataModel.AccountPaid,
                AccountBalance = dataModel.AccountBalance,
                LastPaymentDate = dataModel.LastPaymentDate,
                LastPaymentAmount = dataModel.LastPaymentAmount,
                PastDue = dataModel.PastDue,
                FutureDue = dataModel.FutureDue
            };

            return viewModel;
        }
    }
}
