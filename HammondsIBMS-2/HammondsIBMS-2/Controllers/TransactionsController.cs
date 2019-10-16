using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Collections;
using HammondsIBMS_2.ViewModels.IBMSAccounts;
using HammondsIBMS_2.ViewModels.Transactions;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Invoicing;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace HammondsIBMS_2.Controllers
{
    public class TransactionsController : IbmsBaseController
    {
        private readonly AccountTransactionService _accountsService;
        private readonly CustomerService _customerService;
        //
        // GET: /IBMSAccounts/
        public TransactionsController(AccountTransactionService accountsService,CustomerService customerService)
        {
            _accountsService = accountsService;
            _customerService = customerService;
            InitialisePaymentSourcesViewBag(accountsService);
        }


        public ActionResult _DisplayCustomerAccountTransactions(int id)
        {
            ViewBag.CustomerId = id;
            return PartialView(MapCustomerAccountsToViewModel(id));
        }

        private  IEnumerable<AccountTransactionVM> MapCustomerAccountsToViewModel(int id)
        {
            var accs = _accountsService.GetTransactionsForCustomer(id);
            return AutoMapperSetup.MapList<AccountTransaction,AccountTransactionVM>(accs);
        }

        [GridAction]
        public ActionResult _CustomerAccountTransactionsGridBind(int id)
        {
            ViewBag.CustomerId = id;
            return View(new GridModel(MapCustomerAccountsToViewModel(id)));
        }


        public ActionResult _CustomerAccountsBalance(int id)
        {
            var vm = new CustomerBalanceVM { Amount = _accountsService.GetCustomerBalance(id) };
            return PartialView(vm);
        }

        public ActionResult _MakeAdjustment(int id)
        {
            var transaction = _accountsService.GetTransaction(id);
            var ad = new AdjustmentVM()
            {
                CustomerId = transaction.CustomerAccount.CustomerId,
                CustomerAccountId = transaction.CustomerAccountId,
                CustomerAccount = string.Format("{0} - {1}",transaction.CustomerAccountId,transaction.CustomerAccount.AccountType),
                TransactionDate = DateTime.Today,
                AccountTransactionTypeId = (int)transaction.AccountTransactionType,
                AccountTransactionType = transaction.AccountTransactionType.GetDescription(),
                Amount = transaction.Amount,
                GroupId=transaction.GroupId,

            };
            return PartialView(ad);
        }

        [HttpPost]
        public ActionResult _MakeAdjustment(AdjustmentVM mAdjustmentVM)
        {
            if (TryValidateModel(mAdjustmentVM))
            {
                if (ExecuteRepositoryAction(() =>
                {
                    _accountsService.AddAccountTransaction(
                        mAdjustmentVM.CustomerAccountId,
                        (AccountTransactionType)mAdjustmentVM.AccountTransactionTypeId, mAdjustmentVM.Amount,
                        AccountTransactionInputType.Adjustment, mAdjustmentVM.GroupId, mAdjustmentVM.Note);
                    _accountsService.CommitChanges();
                }))
                    return ReturnJsonFormSuccess();
            }

            return PartialView(mAdjustmentVM);
        }

        //private void InitialiseCustomerAccountsViewBagAndAddSelector(int customerId)
        //{
        //    InitialiseCustomerAccountsViewBag(customerId);
        //    ViewBag.CustomerAccounts = AddAllSelector<dynamic>(new { CustomerAccountId = -1, Description = "-----" },
        //        ViewBag.CustomerAccounts);
        //}

        //private void InitialiseCustomerAccountsViewBag(int customerId)
        //{
        //    ViewBag.CustomerAccounts =
        //        _customerService.GetCustomerAccounts(customerId)
        //            .Select(
        //                c =>
        //                    new
        //                    {
        //                        c.CustomerAccountId,
        //                        Description = string.Format("{0} - {1}", c.CustomerAccountId, c.AccountType.ToString())
        //                    });
        //}

        //private void InitialiseTransactionTypeViewBag()
        //{

        //    ViewBag.TransactionTypes =
        //        Enum.GetValues(typeof(AccountTransactionType))
        //            .Cast<AccountTransactionType>()
        //            .Select(
        //                c =>
        //                    new
        //                    {
        //                        AccountTransactionId = (int)c,
        //                        Description = c.GetDescription()
        //                    });
        //}

        //private void InitialiseTransactionTypeViewBagAndAddSelector()
        //{
        //    InitialiseTransactionTypeViewBag();
        //    ViewBag.TransactionTypes = AddAllSelector<dynamic>(new { AccountTransactionId = -1, Description = "-----" },
        //        ViewBag.TransactionTypes);
        //}

        //#region Payment Source

        //public ActionResult DisplayPaymentSources()
        //{
        //    return View(_accountsService.GetPaymentSources());
        //}

        //public ActionResult AddPaymentSource(PaymentSource mPaymentSource)
        //{
        //    if (TryValidateModel(mPaymentSource))
        //    {
        //        if (ExecuteRepositoryAction(() =>
        //                                        {
        //                                            _accountsService.AddPaymentSource(mPaymentSource);
        //                                            _accountsService.CommitChanges();
        //                                        }))
        //            return RedirectToAction("DisplayPaymentSources", this.GridRouteValues());
        //    }
        //    return View("DisplayPaymentSources", _accountsService.GetPaymentSources());
        //}

        //public ActionResult UpdatePaymentSource(PaymentSource mPaymentSource)
        //{
        //    var pm = _accountsService.GetPaymentSource(mPaymentSource.PaymentSourceId);
        //    if (TryUpdateModel(pm))
        //    {
        //        if (ExecuteRepositoryAction(() => { _accountsService.UpdatePaymentSource(pm); _accountsService.CommitChanges(); }))
        //            return RedirectToAction("DisplayPaymentSources", this.GridRouteValues());
        //    }
        //    return View("DisplayPaymentSources", _accountsService.GetPaymentSources());
        //}
        
        //#endregion
    }
}
