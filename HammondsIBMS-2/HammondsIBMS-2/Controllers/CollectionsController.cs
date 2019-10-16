using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Collections;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Contracts;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class CollectionsController : IbmsBaseController
    {
        private readonly AccountTransactionService _accountTransactionService;
        private readonly CustomerAccountService _customerAccountService;
        private readonly CustomerService _customerService;

        public CollectionsController(AccountTransactionService accountTransactionService,
            CustomerService customerService, CustomerAccountService customerAccountService)
        {
            _accountTransactionService = accountTransactionService;
            _customerService = customerService;
            _customerAccountService = customerAccountService;

            InitialisePaymentSourcesViewBag(_accountTransactionService);
        }

        public ActionResult _DisplayCustomerCollections(int id)
        {
            var customer = _customerService.GetCustomer(id);
            var vm = new DisplayCollectionVM
            {
                CustomerId = id,
                HasRentals = customer.CustomerAccounts.Any(c => c is RentalAccount),
                HasAccounts = customer.CustomerAccounts != null && customer.CustomerAccounts.Count > 0
            };

            return PartialView(vm);
        }

        [GridAction]
        public ActionResult _DisplayOutstandingAmounts(int customerId)
        {
            IEnumerable<AccountTransactionService.RolledTransaction> rolledTrans = GetRolledTransactions(customerId);

            return View(new GridModel(MapToCollectionsVM(rolledTrans)));
        }

        private IEnumerable<AccountTransactionService.RolledTransaction> GetRolledTransactions(int customerId)
        {
            var rolledTrans = new List<AccountTransactionService.RolledTransaction>();
            foreach (CustomerAccount account in GetCustomerAccounts(customerId))
            {
                rolledTrans.AddRange(
                    _accountTransactionService.GetOutstandingTransactionsForAccount(account.CustomerAccountId).ToList());
            }
            return rolledTrans;
        }

        public ActionResult _ReceiveCollections(int customerId)
        {
            ViewBag.CustomerId = customerId;
            return PartialView();
        }

        [HttpPost]
        public ActionResult _ReceiveCollections(ReceiveCollectionsVM mReceiveCollectionsVM)
        {
            try
            {
                var collectionsWithPayments = mReceiveCollectionsVM.Collections.Where(c => c.Payment != 0);
                foreach (var collection in collectionsWithPayments)
                {
                    _accountTransactionService.AddAccountTransaction(collection.CustomerAccountId,
                        (AccountTransactionType) collection.AccountTransactionType, -(decimal) collection.Payment,
                        AccountTransactionInputType.Payment, collection.GroupId, collection.Note);
                }
                _accountTransactionService.CommitChanges();
                return ReturnJsonFormSuccess();
            }
            catch (Exception ex)
            {
                return ReturnJsonError(ex.Message);
            }
        }

        private IEnumerable<CollectionVM> MapToCollectionsVM(
            IEnumerable<AccountTransactionService.RolledTransaction> rolledTrans)
        {
            return
                rolledTrans.Where(c => c.Amount != 0)
                    .OrderBy(c => c.CustomerAccountId)
                    .ThenBy(c => c.GroupId)
                    .ThenBy(c => c.AccountTransactionType)
                    .Select(c => new CollectionVM
                    {
                        CustomerAccountId = c.CustomerAccountId,
                        AccountType = _customerAccountService.GetAccount(c.CustomerAccountId).AccountType.ToString(),
                        AccountTransactionType = (int) c.AccountTransactionType,
                        AccountTransactionTypeDescription = c.AccountTransactionType.GetDescription(),
                        Amount = (float) c.Amount,
                        GroupId = c.GroupId,
                        Note = c.Note
                    });
        }

        public ActionResult _GetReceiveCollectionVMasJson(int customerId)
        {
            var recieveCollectionVM = new ReceiveCollectionsVM
            {
                CustomerId = customerId,
                PreferredPaymentSourceId =
                    _customerAccountService.GetAccount(customerId).Customer.PreferredPaymentSourceId,
                Collections = MapToCollectionsVM(GetRolledTransactions(customerId)).ToList()
            };

            return Json(recieveCollectionVM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetCollectionVMasJson(int customerId)
        {
            List<CollectionVM> collsVM = MapToCollectionsVM(GetRolledTransactions(customerId)).ToList();
            return Json(collsVM, JsonRequestBehavior.AllowGet);
        }

        [GridAction]
        public ActionResult _GetPaidUntilForRentals(int customerId)
        {
            var rentedUnits = new List<RentedUnit>();
            var f = _customerService.GetCustomer(customerId).CustomerAccounts;
            var rentalAccounts = f.OfType<RentalAccount>().Select(customerAccount => customerAccount).ToList();
            foreach (var rentalAccount in rentalAccounts)
            {
                rentedUnits.AddRange(rentalAccount.RentedUnits.Where(c => c.RemovalDate == null));
            }

            return View(new GridModel(AutoMapperSetup.MapList<RentedUnit, RentedUnitVM>(rentedUnits)));
        }

        private IEnumerable<CustomerAccount> GetCustomerAccounts(int customerId)
        {
            return _customerService.GetCustomerAccounts(customerId);
        }

        

    }


}