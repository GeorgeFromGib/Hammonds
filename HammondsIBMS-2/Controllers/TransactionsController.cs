using System.Collections.Generic;
using System.Web.Mvc;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.IBMSAccounts;
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
        //
        // GET: /IBMSAccounts/
        public TransactionsController(AccountTransactionService accountsService)
        {
            _accountsService = accountsService;
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
