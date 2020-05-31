using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.Stock;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    //TODO Add. Create Contract in PDF/Print format
    //TODO Add. Add Delivery to contract, Alternate address, Instructions - Printable

    public class AccountController : IbmsBaseController
    {
        private readonly CustomerAccountService _accountService;
        private readonly CustomerService _customerService;
        private readonly DocumentTemplateService _documentTemplateService;
        private readonly ItemChargeService _itemChargeService;
        private readonly ContractService _contractService;
        private readonly StockService _stockService;


        public AccountController(CustomerService customerService, CustomerAccountService accountService, StockService stockService, ContractService contractService, DocumentTemplateService documentTemplateService)
        
        {
            _customerService = customerService;
            _accountService = accountService;
            _stockService = stockService;
            //_itemChargeService = itemChargeService;
            _contractService = contractService;
            _documentTemplateService = documentTemplateService;
            InitialisePaymentPeriodViewBag(contractService);
            InitialiseContractTypeViewBag(contractService);
            InitialiseContractChangeableProductLifeCycleViewBag(stockService);
        }


        public ActionResult _DisplayCustomerAccounts(int customerId, int moveToEditAccount = -1)
        {
            Customer customer = _customerService.GetCustomer(customerId);
            ViewBag.MoveOnToEditAccount = moveToEditAccount;
            return PartialView(customer);
        }


        public ActionResult _GetModelInfoForStockId(int id)
        {
            Model model = _stockService.GetStock(id).Model;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[GridAction]
        //public ActionResult _DisplayServiceContracts(int modelId)
        //{
        //    return View(new GridModel(AutoMapperSetup.MapList<ServiceContract, ServiceContractVM>(TempAccount.AttachedContracts.ConvertAll(c=>(ServiceContract)c))));
        //}


        //public ActionResult _AddAccountEditServiceContractPaymentPeriod(int id)
        //{
        //    EditServiceContractVM cont =
        //        Mapper.Map<ServiceContract, EditServiceContractVM>(
        //            TempAccount.AttachedContracts.Single(c => c.ContractId == id) as ServiceContract);
        //    return PartialView("_EditServiceContractPaymentPeriod", cont);
        //}

        //[HttpPost]
        //public ActionResult _AddAccountEditServiceContractPaymentPeriod(EditServiceContractVM mEditServiceContractVm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ServiceContract contract = GetTempAccountServiceContractById(mEditServiceContractVm.ContractId);
        //        contract.PaymentPeriodId = mEditServiceContractVm.PaymentPeriodId;
        //        contract.PaymentPeriod = _contractService.GetPaymentPeriodType(contract.PaymentPeriodId);

        //        return ReturnJsonFormSuccess();
        //    }
        //    return PartialView("_EditServiceContractPaymentPeriod", mEditServiceContractVm);
        //}

        //[OutputCache(Duration = 0)]
        //public ActionResult _ChangeServiceContractPaymentPeriod(EditServiceContractVM mServiceContract)
        //{
        //    ServiceContract cur = GetTempAccountServiceContractById(mServiceContract.ContractId);
        //    cur.PaymentPeriod = _contractService.GetPaymentPeriodType(mServiceContract.PaymentPeriodId);

        //    return Json(Mapper.Map<ServiceContract, EditServiceContractVM>(cur), JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult _AddAccountDeleteServiceContract(int id)
        //{
        //    ContractType contract =
        //        (GetTempAccountServiceContractById(id)).ContractType;
        //    var promptVm = new PromptVM {RecordIndex = id};
        //    promptVm.AddPrompt(
        //        new DialogPrompt("Do you wish to remove contract '" + contract.Description + "' from this purchase",
        //            PromptMessageAlertLevel.Warning));
        //    return PartialView("_PromptDialog", promptVm);
        //}

        //[HttpPost]
        //public ActionResult _AddAccountDeleteServiceContract(PromptVM mPromptVm)
        //{
        //    TempAccount.AttachedContracts.Remove(GetTempAccountServiceContractById(mPromptVm.RecordIndex));
        //    return ReturnJsonFormSuccess();
        //}

        //private ServiceContract GetTempAccountServiceContractById(int id)
        //{
        //    return TempAccount.AttachedContracts.Single(c => c.ContractId == id) as ServiceContract;
        //}


        #region Documents

        //public ActionResult PrintViewPurchaseAccountDocument(int id)
        //{
        //    var purchaseAccount = _accountService.GetAccount(id) as PurchaseAccount;
        //    var doc = _documentTemplateService.CreatePurchaseContractDocumentTemplate().ReturnParsedDocument(purchaseAccount);
        //    return new HtmlToPdfAction().RenderToAction(doc, "PurchaseAccount");
        //}

        public ActionResult PrintViewPurchaseAccountDocument(int id)
        {
            var purchaseAccount = _accountService.GetAccount(id) as PurchaseAccount;
            string doc =
                _documentTemplateService.CreatePurchaseContractDocumentTemplate().ReturnParsedDocument(purchaseAccount);
            return PartialView("_DocumentTemplate", doc);
        }

        public ActionResult PrintViewRentAccountDocument(int id)
        {
            var rentalAccount = _accountService.GetAccount(id) as RentalAccount;
            string doc =
                _documentTemplateService.CreateRentContractDocumentTemplate().ReturnParsedDocument(rentalAccount);
            return PartialView("_DocumentTemplate", doc);
        }

        #endregion


        #region EditAccount

        public ActionResult _EditAccount(int accountId)
        {
            ActionResult result = null;
            //PurchaseUnitAndContractsGridClassFactory.UnitAndContractsClassToBuild =
            //    typeof (PermPurchaseUnitAndContracts);

            CustomerAccount account = _accountService.GetAccount(accountId);
            if (account is PurchaseAccount)
                result = PartialView("_EditPurchaseAccount", account as PurchaseAccount);
            if (account is RentalAccount)
                result = PartialView("_EditRentAccount", account as RentalAccount);

            return result;
        }

        #region Basket


        public ActionResult DeleteBasketForAccount(int customerAccountId)
        {
            var promptVm = new PromptVM { RecordIndex = _accountService.GetBaskets().Single(c => c.CustomerAccountId == customerAccountId).BasketId };
            promptVm.AddPrompt(
                new DialogPrompt("Do you wish to remove the current basket?",
                    PromptMessageAlertLevel.Warning));
            return PartialView("_PromptDialog", promptVm);
        }

        [HttpPost]
        public ActionResult DeleteBasketForAccount(PromptVM mPromptVM)
        {
            var basket = _accountService.GetBasket(mPromptVM.RecordIndex);
            _accountService.DeleteBasket(basket, true);
            _accountService.CommitChanges();
            return ReturnJsonFormSuccess();
        }

        [OutputCache(Duration = 0)]
        public ActionResult _CheckBasketExistsForAccount(int customerAccountId)
        {
            var basketExists = _accountService.GetBaskets().Any(c => c.CustomerAccountId == customerAccountId);
            return Json(basketExists, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Rental Account

        public ActionResult _DisplayRentalAccountItems(int id)
        {
            ViewBag.CustomerAccountId = id;
            return PartialView("_DisplayRentedUnits");
        }

        public ActionResult _EditRentalAccountContractsDisplay(int id)
        {
            ViewBag.CustomerAccountId = id;
            return PartialView("_DisplayRentalAttachedContractsForEdit");
        }

        //public ActionResult _AddRentalAccountItems(int id)
        //{
        //    var vm = new RentedUnitVM {CustomerAccountId = id, RentedDate = DateTime.Today};
        //    return PartialView("_EditRentedUnit", vm);
        //}

        //[HttpPost]
        //public ActionResult _AddRentalAccountItems(RentedUnitVM mRentedUnitVm)
        //{
        //    if (TryValidateModel(mRentedUnitVm))
        //    {
        //        var ru = new RentedUnit();
        //        if (TryUpdateModel(ru))
        //        {
        //            if (ExecuteRepositoryAction(() =>
        //            {
        //                _accountService.AddRentedUnit(ru.CustomerAccountId,ru);
        //                _customerService.CommitChanges();
        //            }))
        //                return ReturnJsonFormSuccess();
        //        }
        //    }

        //    return PartialView("_EditRentedUnit", mRentedUnitVm);
        //}

        public ActionResult _EditRentedItem(int id)
        {
            RentedUnitVM ru = Mapper.Map<RentedUnit, RentedUnitVM>(GetRentedUnit(id));
            ru.CanChangeRentedDate = false;
            ru.CanChangeDeposit = false;
            ru.OldStockId = -1; //to avoid validation where same stock is selected in other screens
            return PartialView("_EditRentedUnit", ru);
        }

        [HttpPost]
        public ActionResult _EditRentedItem(RentedUnitVM mRentedUnitVm)
        {
            var ru = GetRentedUnit(mRentedUnitVm.UnitId);
            if (TryUpdateModel(ru))
            {
                if (ExecuteRepositoryAction(() =>
                {
                    _accountService.UpdateUnit(ru);
                    _customerService.CommitChanges();
                }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_EditRentedUnit", mRentedUnitVm);
        }

        public ActionResult _EditRentalAccountStock(int id)
        {
            Unit cu = GetRentedUnit(id);
            ViewBag.ContractChangeableProductLifeCycle =
                _accountService.GetChangeableProductLifeCylesForAccount(cu.CustomerAccountId)
                    .Select(c => c.ProductLifeCycle);
            var conVm = EditCustomerAccountStockVM.CreateContractUnitStockVM(
                cu.CustomerAccountId, cu.StockId, cu.Stock.ProductLifeCycleId, cu.UnitId);
            return PartialView("_EditAccountStock", conVm);
        }

        [HttpPost]
        public ActionResult _EditRentalAccountStock(EditCustomerAccountStockVM mContractUnitStock)
        {
            var ru = GetRentedUnit(mContractUnitStock.UnitId);

            if (ExecuteRepositoryAction(() =>
            {
                _accountService.SwapRentedUnit(ru, mContractUnitStock.StockId, mContractUnitStock.OldStockProductCycleId);
                _customerService.CommitChanges();
            }))
            {
                return ReturnJsonFormSuccess();
            }

            return PartialView("_EditAccountStock", mContractUnitStock);
        }

        public ActionResult _DeleteAccountStock(int id)
        {
            var cu = GetRentedUnit(id);
            //var vm = DeleteRentedUnitVM.CreateDeleteRentedUnitVM(cu.UnitId, DateTime.Today,
            //                                                     cu.Stock.ProductLifeCycleId, cu.Stock.ManufacturerModel);
            DeleteRentedUnitVM vm = Mapper.Map<RentedUnit, DeleteRentedUnitVM>(cu);
            vm.ReturnDeposit = true;
            return PartialView(vm);
        }

        private RentedUnit GetRentedUnit(int id)
        {
            return _accountService.GetUnit(id) as RentedUnit;
        }

        [HttpPost]
        public ActionResult _DeleteAccountStock(DeleteRentedUnitVM mDeleteRentedUnitVm)
        {
            var cu = GetRentedUnit(mDeleteRentedUnitVm.UnitId);
            if (TryUpdateModel(cu))
            {
                if (TryValidateModel(cu))
                {
                    if (ExecuteRepositoryAction(() =>
                    {
                        _accountService.DeleteRentedUnit(cu, mDeleteRentedUnitVm.ProductCycleLifeId, mDeleteRentedUnitVm.ReturnDeposit);
                        _accountService.CommitChanges();
                    }))
                    {
                        return ReturnJsonFormSuccess();
                    }
                }
            }
            return PartialView(mDeleteRentedUnitVm);
        }

        [GridAction]
        public ActionResult _DisplayRentedUnits(int id, bool? showRemoved)
        {
            ViewBag.CustomerAccountId = id;
            var pred = (showRemoved != null && showRemoved.Value) ? new Func<RentedUnit, bool>(c => true) : (c => c.RemovalDate == null);
            var units = ((RentalAccount)_accountService.GetAccount(id)).RentedUnits.Where(pred);
            return View(new GridModel(AutoMapperSetup.MapList<RentedUnit, RentedUnitVM>(units).ToList()));
        }

        //public ActionResult _EditRentedUnit(int id)
        //{
        //    var ru = Mapper.Map<RentedUnit, RentedUnitVM>(GetRentedUnit(id));
        //    ru.OldStockId = -1; //to avoid validation where same stock is selected in other screens
        //    return PartialView("_EditRentedUnit", ru);
        //}

        //[HttpPost]
        //public ActionResult _EditRentedUnit(RentedUnitVM mRentedUnitVm)
        //{
        //    var ru = GetRentedUnit(mRentedUnitVm.UnitId);
        //    if (TryUpdateModel(ru))
        //    {
        //        if (ExecuteRepositoryAction(() => { _accountService.UpdateUnit(ru); _accountService.CommitChanges(); }))
        //            return ReturnJsonFormSuccess();
        //    }
        //    return PartialView("_EditRentedUnit", mRentedUnitVm);
        //}

        [GridAction]
        public ActionResult _DisplayRentalContracts(int id, bool? showExpired)
        {
            var cu = _contractService.GetCustomerAccount(id);
            var pred = (showExpired != null && showExpired.Value) ? new Func<Contract, bool>(c => true) : (c => c.ExpiryDate >= DateTime.Today || c.ExpiryDate == null);
            return
                View(
                    new GridModel(
                        AutoMapperSetup.MapList<RentalContract, EditRentedContractVM>(
                            cu.AttachedContracts.Where(pred).ToList().ConvertAll(c => (RentalContract)c))));
        }

        #endregion


        #region Purchase Account

        public ActionResult _EditPurchaseAccountContractsDisplay(int id)
        {
            ViewBag.CustomerAccountId = id;
            return PartialView("_EditPurchaseAccountContractsDisplay");
        }

        [GridAction]
        public ActionResult _GetOneOffItemsForGrid(int id)
        {
            var ofis = _accountService.GetAccount(id).OneOffItems.Where(c => c.RemovalDate == null);
            return View(new GridModel(ofis));
        }

        public ActionResult _DeleteOneOffItem(int id)
        {
            var ofis = _accountService.GetOneOffItem(id);
            var prompt = new DeleteOneOffItemsPromptVM
            {
                RecordIndex = ofis.OneOffItemId,
                Refunded = false,
                Amount = ofis.TotalCharge,
                Message = ofis.Description,
            };
            return PartialView(prompt);
        }

        [HttpPost]
        public ActionResult _DeleteOneOffItem(DeleteOneOffItemsPromptVM mDeleteOneOffItemsPromptVM)
        {
            var of = _accountService.GetOneOffItem(mDeleteOneOffItemsPromptVM.RecordIndex);
            if (TryUpdateModel(of))
            {
                if (ExecuteRepositoryAction(() =>
                {
                    _accountService.RemoveOneOffItem(of);
                    _accountService.CommitChanges();
                }))
                    return ReturnJsonFormSuccess();

            }
            return PartialView(mDeleteOneOffItemsPromptVM);
        }

        [HttpPost]
        public ActionResult _EditPurchaseAccountStock(EditCustomerAccountStockVM mCustomerAccount)
        {
            throw new NotImplementedException();
            //if (ModelState.IsValid && TryValidateModel(mCustomerAccount))
            //{
            //    var cu = _contractService.GetCustomerAccount(mCustomerAccount.CustomerAccountId) as PurchaseAccount;
            //    if (TryUpdateModel(cu))
            //    {
            //        if (ExecuteRepositoryAction(() =>
            //        {
            //            _customerService.UpdatePurchaseAccountStock(cu,
            //                mCustomerAccount.
            //                    OldStockId,
            //                mCustomerAccount.
            //                    OldStockProductCycleId);
            //            _customerService.CommitChanges();
            //        }))
            //        {
            //            return ReturnJsonFormSuccess();
            //        }
            //    }
            //}
            //return PartialView("_EditAccountStock", mCustomerAccount);
        }

        public ActionResult _AddContract(int id)
        {
            AddNonSelectableViewBagContractType();
            var con = new ServiceContract
            {
                StartDate = DateTime.Today,
                ContractTypeId = -1,
                PaymentPeriodId = 1,
                ContractLengthInMonths = 1,
                PaymentPeriod = _contractService.GetPaymentPeriodType(1),
                ExpiryDate = DateTime.Today.AddMonths(1)
            };
            return PartialView("_EditContract", con);
        }


        private void AddNonSelectableViewBagContractType()
        {
            var contractType = new ContractType { ContractTypeId = -1, Description = "----" };
            var contypes = (List<ContractType>)ViewBag.ContractTypes;
            contypes.Add(contractType);
            ViewBag.ContractTypes = contypes;
        }




        #endregion

        #region Shared Functions

        public ActionResult _DisplayAlternateAddress(int id)
        {
            var acc = _accountService.GetAccount(id);
            return PartialView("_AlternateAddress", acc);
        }

        public ActionResult _EditAlternateAddress(int id)
        {
            var altAddrVM = Mapper.Map<CustomerAccount, AlternateAddressVM>(_accountService.GetAccount(id));
            return PartialView(altAddrVM);
        }

        [HttpPost]
        public ActionResult _EditAlternateAddress(AlternateAddressVM mAlternateAddressVM)
        {
            var acc = _accountService.GetAccount(mAlternateAddressVM.CustomerAccountId);
            if (TryUpdateModel(acc))
            {
                if (ExecuteRepositoryAction(() =>
                {
                    _accountService.UpdateAccount(acc);
                    _accountService.CommitChanges();
                }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mAlternateAddressVM);
        }

        #endregion

        #endregion


        #region Stock Filter

        // public ActionResult _StockDetailsForFilter(int id)
        //{
        //    var stock = _customerService.GetContractUnit(id).Stock;
        //    return PartialView("_StockDetailsForFilter", stock);
        //}

        public ActionResult _GetStock(int id)
        {
            Stock rslt = _stockService.GetStock(id);
            return PartialView("_StockDetailsForFilter", rslt);
        }

        public ActionResult _IsStockInAccount(int id)
        {
            return Json((_stockService.GetStock(id).CustomerAccountId != null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetStockCustomerAccountId(int id)
        {
            return Json(_stockService.GetStock(id).CustomerAccountId, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _FindStockByIdentifierForAutoComplete(string text)
        {
            //Thread.Sleep(50);
            var rslt =
                _stockService.FindStock(s => s.Identifier.StartsWith(text)).Select(
                    m =>
                        new
                        {
                            m.StockId,
                            Identifier =
                                string.Format("{0}   ({1})", m.Identifier,
                                    m.ProductLifeCycleStatus.DescriptionAbbreviation)
                        });
            return new JsonResult { Data = new SelectList(rslt.ToList(), "StockId", "Identifier") };
        }

        [HttpPost]
        public ActionResult _FindStockBySerialForAutoComplete(string text)
        {
            //Thread.Sleep(1000);
            var rslt =
                _stockService.FindStock(s => s.SerialNumber.StartsWith(text)).Select(
                    m =>
                        new
                        {
                            m.StockId,
                            Serial =
                                string.Format("{0}   ({1})", m.SerialNumber,
                                    m.ProductLifeCycleStatus.DescriptionAbbreviation)
                        });
            return new JsonResult { Data = new SelectList(rslt.ToList(), "StockId", "Serial") };
        }

        #endregion
    }
}