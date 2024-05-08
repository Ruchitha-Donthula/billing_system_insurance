using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingSystemDataAccess;
using BillingSystemDataModel;

namespace BillingSystemBusinessTest
{
    public class BillingSystemBusinessTestMain
    {
        public static void Main(string[] args)
        {
            //new BillAccountBusinessTest().TestCreateBillAccount();
            //new BillAccountBusinessTest().TestAssociateBillAccountWithPolicy();
            //new BillAccountBusinessTest().TestGetBillAccountById();
            //new BillAccountBusinessTest().TestGetBillAccountByNumber();
            //new BillAccountBusinessTest().TestUpdateBillAccount();
            //new BillAccountBusinessTest().TestSuspendBillAccount();
            //new BillAccountBusinessTest().TestReleaseBillAccount();

            new InstallmentBusinessTest().CreateInstallmentSchedule();

            //new InvoiceBusinessTest().TestCreateInvoice();

            //new PaymentBusinessTest().TestApplyPayment();

            //new InstallmentBusinessTest().TestInstallmentRescheduling();
            //new InstallmentBusinessTest().TestOnChangeOfPayPlan();


            Console.ReadLine();
        }
    }
}
