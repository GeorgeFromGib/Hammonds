using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_Application;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Values;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class NewAccountController : IbmsBaseController
    {
        private readonly CustomerService _customerService;
        private readonly CustomerAccountService _accountService;
        private readonly ItemChargeService _itemChargeService;
        private readonly StockService _stockService;
        private readonly ContractService _contractService;
        private readonly MiscService _miscService;
        private readonly AccountTransactionService _accountTransactionService;

        public NewAccountController(CustomerService customerService, CustomerAccountService accountService, ItemChargeService itemChargeService, StockService stockService,ContractService contractService,MiscService miscService,AccountTransactionService accountTransactionService)
        {
            _customerService = customerService;
            _accountService = accountService;
            _itemChargeService = itemChargeService;
            _stockService = stockService;
            _contractService = contractService;
            _miscService = miscService;
            _accountTransactionService = accountTransactionService;
        }

        #region Add New Account Stepper

        public ActionResult AddNewAccount(int customerId)
        {
            TempRepository<NewAccountVM>.Entity = new NewAccountVM { CustomerId = customerId, StartDate = DateTime.Today };
            var header = string.Format("Customer {0} - Add Account",
                                       _customerService.GetCustomer(customerId).SurnameFirstName);
            Stepper2.CreatePageStepper("CancelStepper", header);
            Stepper2.AddStep("AddAccount");
            TempAccount = null;
            return RedirectToAction(Stepper2.NextStep());
        }

        public ActionResult AddAccount()
        {
            return View(TempRepository<NewAccountVM>.Entity);
        }

        [HttpPost]
        public ActionResult AddAccount(NewAccountVM mNewAccountVm)
        {
            if (!ModelState.IsValid) return View(mNewAccountVm);
            
            var newAccVm = TempRepository<NewAccountVM>.Entity;
            if (TryUpdateModel(newAccVm))
            {
                CustomerAccount account = null;
                switch (newAccVm.AccountType)
                {
                    case AccountType.Purchase:
                        account = new PurchaseAccount();
                        break;
                    case AccountType.Rent:
                        account = new RentalAccount();
                        break;
                }
                account.AlternateAddressInstructions = newAccVm.AlternateAddressInstructions;
                account.AlternateAddress = newAccVm.AlternateAddress;
                account.CustomerId = newAccVm.CustomerId;
                account.OpenedDate = newAccVm.StartDate;

                _accountService.AddAccount(account);
                _accountService.CommitChanges();
                return RedirectToAction("Edit", "Customer",
                                            new
                                            {
                                                id = account.CustomerId,
                                                moveOnToEditAccount = account.CustomerAccountId
                                            });
            }
            //if (TryUpdateModel(newAccVm))
            //{
            //    if (TempAccount == null || TempAccount.AccountType != newAccVm.AccountType)
            //    {
            //        TempAccount = _accountService.AccountBuilder(newAccVm.AccountType, newAccVm.CustomerId);
            //        //TempAccount.OneOffItems = InitiateOneOffItems();
            //    }
            //    switch (TempAccount.AccountType)
            //    {
            //        case AccountType.Purchase:
            //            if ((TempAccount as AddPurchaseAccountVM) == null)
            //            {
            //                TempAccount = Mapper.Map<PurchaseAccount, AddPurchaseAccountVM>(TempAccount as PurchaseAccount);
            //                //TempAccount.OneOffItems = InitiateOneOffItems(c => c.PurchaseDefault);
            //                (TempAccountAsAddPurchaseAccountVM()).StartDate = mNewAccountVm.StartDate;
            //            }
            //            else
            //            {
            //                if (mNewAccountVm.StartDate != TempAccountAsAddPurchaseAccountVM().StartDate)
            //                {
            //                    var newStartDate = mNewAccountVm.StartDate;
            //                    TempAccountAsAddPurchaseAccountVM().StartDate = newStartDate;
            //                    TempAccountAsAddPurchaseAccountVM().PurchasedUnits.ForEach(u => u.PurchasedDate = newStartDate);
            //                    TempAccountAsAddPurchaseAccountVM().PurchasedUnits.ForEach(u => u.Contracts.ForEach(c => c.SetStartDate(newStartDate)));
            //                }
            //            }
            //            BuildPurchaseAccountStepper();
            //            return RedirectToAction(Stepper2.NextStep());
            //        case AccountType.Rent:
            //            TempAccount = (TempAccount is AddRentAccountVM) ? TempAccount : Mapper.Map<RentalAccount, AddRentAccountVM>(TempAccount as RentalAccount);
            //            TempAccount.OneOffItems = InitiateOneOffItems(c => c.RentalDefault);
            //            (TempAccountAsAddRentAccountVM()).StartDate = mNewAccountVm.StartDate;
            //            BuildRentAccountStepper();
            //            return RedirectToAction(Stepper2.NextStep());
            //    }
            //}
            return View(mNewAccountVm);
        }

        private void BuildRentAccountStepper()
        {
            Stepper2.Header = string.Format("Customer {0} - Add Rent Account",
                _customerService.GetCustomer(TempAccount.CustomerId).SurnameFirstName);
            Stepper2.ClearSteps();
            Stepper2.AddStep("AddAccountStep1");
            Stepper2.AddStep("AddRentAccountStep2");
            Stepper2.AddStep("AddRentAccountStep3");
            Stepper2.AddStep("AddRentAccountStep4");
            Stepper2.AddStep("AddRentAccountStep5");
            Stepper2.AddStep("AddRentAccountStep6");
        }

        private void BuildPurchaseAccountStepper()
        {
            Stepper2.Header = string.Format("Customer {0} - Add Purchase Account",
                _customerService.GetCustomer(TempAccount.CustomerId).SurnameFirstName);
            Stepper2.ClearSteps();
            Stepper2.AddStep("AddAccountStep1");
            Stepper2.AddStep("AddPurchaseAccountStep3");
            Stepper2.AddStep("AddPurchaseAccountStep4");
            Stepper2.AddStep("AddPurchaseAccountStep5");
            Stepper2.AddStep("AddPurchaseAccountStep6");
            Stepper2.AddStep("AddPurchaseAccountStep7");
        }

        private List<OneOffItem> InitiateOneOffItems(Func<ItemCharge, bool> where, int quantity = 1)
        {
            var oneOffItems = new List<OneOffItem>();
            foreach (var itemCharge in _itemChargeService.GetItemCharges().Where(where))
            {
                var ofi = CreateOneOfItemFromPresetOneOffCharges(itemCharge, quantity);
                ofi.OneOffItemId = oneOffItems.NextIndex(c => c.OneOffItemId);
                oneOffItems.Add(ofi);
            }

            return oneOffItems;
        }

        private static OneOffItem CreateOneOfItemFromPresetOneOffCharges(ItemCharge itemCharge, int quantity = 1)
        {
            return new OneOffItem
            {
                Date = DateTime.Today,
                Description = itemCharge.Description,
                Charge = itemCharge.Amount,
                Quantity = quantity,
                OneOffTypeId = (int)OneOffTypes.Work
            };
        }

        #region Add Rent Account Steps

        public ActionResult AddRentAccountStep2()
        {
            return View(TempAccountAsAddRentAccountVM());
        }

        [HttpPost]
        public ActionResult AddRentAccountStep2(AddRentAccountVM mAddRentAccountVm)
        {

            var rentAcc = TempAccountAsAddRentAccountVM();
            if (rentAcc.RentedUnits == null || rentAcc.RentedUnits.Count == 0)
            {
                ModelState.AddModelError("", "No Unit has been selected!");
                return View(mAddRentAccountVm);
            }

            rentAcc.AttachedContracts = new List<Contract>();
            rentAcc.OneOffItems.ForEach(c => c.Quantity = rentAcc.RentedUnits.Count);
            return RedirectToAction(Stepper2.NextStep());
        }

        public ActionResult AddRentAccountStep3() // Add One Off Items
        {
            var rentAcc = TempAccountAsAddRentAccountVM();

            return View(rentAcc);
        }

        [HttpPost]
        public ActionResult AddRentAccountStep3(AddRentAccountVM mAddRentAccountVm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mAddRentAccountVm);
        }

        public ActionResult AddRentAccountStep4() //Alternate address and instructions
        {
            var purchaseAcc = TempAccountAsAddRentAccountVM();
            return View(purchaseAcc);
        }

        [HttpPost]
        public ActionResult AddRentAccountStep4(AddRentAccountVM mAddPurchaseAccountVm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mAddPurchaseAccountVm);
        }

        public ActionResult AddRentAccountStep5() // Payment and finish
        {
            var rentAcc = TempAccountAsAddRentAccountVM();
            rentAcc.Payment = rentAcc.TotalToPay;
            return View(rentAcc);
        }

        [HttpPost]
        public ActionResult AddRentAccountStep5(AddRentAccountVM mAddRentAccountVm)
        {
            if (ModelState.IsValid)
            {
                var rentAcc = TempAccountAsAddRentAccountVM();
                if (TryUpdateModel(rentAcc))
                {
                    var newAccount = new RentalAccount
                    {
                        AlternateAddressInstructions = rentAcc.AlternateAddressInstructions,
                        AlternateAddress = rentAcc.AlternateAddress,
                        CustomerId = rentAcc.CustomerId,
                        AttachedContracts = new List<Contract>(),
                        RentedUnits = new List<RentedUnit>(),
                        OneOffItems = new List<OneOffItem>(),
                    };
                    foreach (RentedUnit mRentedUnit in rentAcc.RentedUnits)
                    {
                        var rentedUnit = new RentedUnit
                        {
                            Amount = mRentedUnit.Amount,
                            RentedDate = mRentedUnit.RentedDate,
                            StockId = mRentedUnit.StockId,
                            Stock = _stockService.GetStock(mRentedUnit.StockId),
                            Deposit = mRentedUnit.Deposit,
                        };
                        newAccount.AddRentedUnitAndUpdateContract(rentedUnit);
                    }
                    foreach (var oneOffItem in rentAcc.OneOffItems)
                    {
                        var newOneOffItem = new OneOffItem
                        {
                            Description = oneOffItem.Description,
                            Charge = oneOffItem.Charge,
                            Date = rentAcc.StartDate,
                            Quantity = oneOffItem.Quantity,
                            OneOffTypeId = oneOffItem.OneOffTypeId
                        };
                        newAccount.OneOffItems.Add(newOneOffItem);
                    }

                    if (ExecuteRepositoryAction(() =>
                    {
                        _accountService.AddTempAccount(newAccount);
                        _accountService.CommitChanges();
                    }))
                    {
                        var acceptAccount = new AcceptAccountVM
                        {
                            CustomerAccountId = newAccount.CustomerAccountId,
                            FullPayment = (mAddRentAccountVm.Payment > 0)
                        };
                        Session["_accountId"] = acceptAccount;
                        return RedirectToAction(Stepper2.NextStep());
                    }
                }
            }
            return View(mAddRentAccountVm);
        }

        public ActionResult AddRentAccountStep6() // Accept Contract View/Print and finish
        {
            var account = (AcceptAccountVM)Session["_accountId"];
            return View(account);
        }

        [HttpPost]
        public ActionResult AddRentAccountStep6(AcceptAccountVM mAcceptAccountVm) // Accept Contract View/Print and finish
        {
            if (ModelState.IsValid)
            {
                var newAccount = _accountService.GetAccount(mAcceptAccountVm.CustomerAccountId) as RentalAccount;
                if (ExecuteRepositoryAction(() =>
                {
                    newAccount.IsTemp = false;
                    _accountService.UpdateAccount(newAccount);
                    foreach (var rental in newAccount.RentedUnits)
                    {
                        _accountTransactionService.AddAccountTransaction(newAccount.CustomerAccountId, AccountTransactionType.Rental, rental.Amount,AccountTransactionInputType.Charge,rental.StockId,rental.Stock.ManufacturerModel);
                        _accountTransactionService.AddAccountTransaction(newAccount.CustomerAccountId, AccountTransactionType.Deposit, rental.Deposit, AccountTransactionInputType.Charge, rental.StockId, rental.Stock.ManufacturerModel);
                        if (mAcceptAccountVm.FullPayment)
                        {
                            _accountTransactionService.AddAccountTransaction(newAccount.CustomerAccountId, AccountTransactionType.Rental, -rental.Amount, AccountTransactionInputType.Payment, rental.StockId, rental.Stock.ManufacturerModel);
                            _accountTransactionService.AddAccountTransaction(newAccount.CustomerAccountId, AccountTransactionType.Deposit, -rental.Deposit, AccountTransactionInputType.Payment, rental.StockId, rental.Stock.ManufacturerModel);
                        }
                    }
                    _accountTransactionService.AddAccountTransaction(newAccount.CustomerAccountId, AccountTransactionType.OneOff, newAccount.TotalOneOffItems,AccountTransactionInputType.Charge);
                    if (mAcceptAccountVm.FullPayment)
                    {
                        _accountTransactionService.AddAccountPaymentTransaction(newAccount.CustomerAccountId, AccountTransactionType.OneOff, -newAccount.TotalOneOffItems);
                    }
                    _accountService.CommitChanges();
                }))
                    return RedirectToAction("Edit", "Customer",
                                            new
                                            {
                                                id = newAccount.CustomerId,
                                                moveOnToEditAccount = newAccount.CustomerAccountId
                                            });
            }
            return View(mAcceptAccountVm);
        }

        #region Add Rent Account Helpers

        [GridAction]
        public ActionResult _DisplayRentedUnits()
        {
            var rentedUnits = TempAccountAsAddRentAccountVM().RentedUnits;
            return View(new GridModel(AutoMapperSetup.MapList<RentedUnit, RentedUnitVM>(rentedUnits)));
        }

        public ActionResult _AddRentedUnits()
        {
            var vm = new RentedUnitVM { RentedDate = DateTime.Today, CanChangeRentedDate = false };
            return PartialView("_EditRentedUnit", vm);
        }

        [HttpPost]
        public ActionResult _AddRentedUnit(int id)
        {
            var ru = new RentedUnit();
            ru.Stock = _stockService.GetStock(id);
            ru.StockId = id;
            ru.RentedDate = TempAccountAsAddRentAccountVM().StartDate;
            ru.UnitId = TempAccountAsAddRentAccountVM().RentedUnits.NextIndex(c => c.UnitId);
            ru.Amount = ru.Stock.Model.RentalPrice;
            ru.Deposit = _miscService.GetDefaultDepositForRentalUnits();
            if (ru.Stock.CustomerAccountId > 0)
                return ReturnJsonError("Unit selected is attached to another account!");
            if (TempAccountAsAddRentAccountVM().RentedUnits.Any(u => u.StockId == id))
                return ReturnJsonFormSuccess();
            TempAccountAsAddRentAccountVM().RentedUnits.Add(ru);
            return ReturnJsonFormSuccess();
        }

        [HttpPost]
        public ActionResult _AddRentedUnits(RentedUnitVM mRentedUnitVm)
        {
            if (TryValidateModel(mRentedUnitVm))
            {
                var ru = new RentedUnit();
                if (TryUpdateModel(ru))
                {
                    ru.Stock = _stockService.GetStock(ru.StockId);
                    ru.UnitId = TempAccountAsAddRentAccountVM().RentedUnits.NextIndex(c => c.UnitId);
                    TempAccountAsAddRentAccountVM().RentedUnits.Add(ru);
                    return ReturnJsonFormSuccess();
                }
            }
            return PartialView("_EditRentedUnit", mRentedUnitVm);
        }

        public ActionResult _EditRentedUnit(int id)
        {
            var ru = TempAccountAsAddRentAccountVM().RentedUnits.Single(c => c.UnitId == id);
            var vm = Mapper.Map<RentedUnit, RentedUnitVM>(ru);
            vm.OldStockId = 0; //Prevent validation of same stock selected
            vm.CanChangeRentedDate = false;
            vm.CanChangeProductCycle = false;
            vm.HideStockSelector = true;
            vm.HideChangeDate = true;
            return PartialView("../Account/_EditRentedUnit", vm);
        }

        [HttpPost]
        public ActionResult _EditRentedUnit(RentedUnitVM mRentedUnitVm)
        {
            var ru = TempAccountAsAddRentAccountVM().RentedUnits.Single(c => c.UnitId == mRentedUnitVm.UnitId);
            if (TryUpdateModel(ru))
            {
                return ReturnJsonFormSuccess();
            }
            return PartialView("../Account/_EditRentedUnit",mRentedUnitVm);
        }

        public ActionResult _GetRentalAndDepositTotals()
        {
            var totalRental = TempAccountAsAddRentAccountVM().TotalRental;
            var totalDeposit = TempAccountAsAddRentAccountVM().TotalDeposit;
            return Json(new { TotalRental = totalRental, TotalDeposit = totalDeposit }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DeleteTempAccountStock(int id)
        {
            var ru = TempAccountAsAddRentAccountVM().RentedUnits.Single(c => c.UnitId == id);
            var prompt = new PromptVM(id, new DialogPrompt(string.Format("Remove model {0} from selection?", ru.Stock.ManufacturerModel)));
            return PartialView("_PromptDialog", prompt);
        }

        [HttpPost]
        public ActionResult _DeleteTempAccountStock(PromptVM mPromptVm)
        {
            var ru = TempAccountAsAddRentAccountVM().RentedUnits.Single(c => c.UnitId == mPromptVm.RecordIndex);
            TempAccountAsAddRentAccountVM().RentedUnits.Remove(ru);
            return ReturnJsonFormSuccess();
        }

        #endregion

        #endregion

        #region Add Purchase Account steps


        public ActionResult AddPurchaseAccountStep3()
        {
            //PurchaseUnitAndContractsGridClassFactory.UnitAndContractsClassToBuild =
            //    typeof(TempPurchaseUnitAndContracts);
            return View(TempAccountAsAddPurchaseAccountVM());
        }

        [HttpPost]
        public ActionResult AddPurchaseAccountStep3(AddPurchaseAccountVM mAddPurchaseAccountVm)
        {
            if (!ModelState.IsValid)
            {
                return View(mAddPurchaseAccountVm);
            }
            var purchaseAcc = TempAccountAsAddPurchaseAccountVM();
            if (purchaseAcc.PurchasedUnits == null || purchaseAcc.PurchasedUnits.Count == 0)
            {
                ModelState.AddModelError("", "No Unit has been selected!");
                return View(mAddPurchaseAccountVm);
            }

            return RedirectToAction(Stepper2.NextStep());
        }


        public ActionResult AddPurchaseAccountStep4() //Setup Item Charges
        {
            var purchaseAcc = TempAccountAsAddPurchaseAccountVM();
            if (purchaseAcc.OneOffItems == null || purchaseAcc.OneOffItems.Count == 0)
                purchaseAcc.OneOffItems = InitiateOneOffItems(c => c.PurchaseDefault);
            purchaseAcc.OneOffItems.ForEach(o => o.Quantity = purchaseAcc.PurchasedUnits.Count);
            return View(purchaseAcc);
        }

        [HttpPost]
        public ActionResult AddPurchaseAccountStep4(AddPurchaseAccountVM mAddPurchaseAccountVm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mAddPurchaseAccountVm);
        }

        public ActionResult AddPurchaseAccountStep5() //Alternate address and instructions
        {
            var purchaseAcc = TempAccountAsAddPurchaseAccountVM();
            return View(purchaseAcc);
        }

        [HttpPost]
        public ActionResult AddPurchaseAccountStep5(AddPurchaseAccountVM mAddPurchaseAccountVm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mAddPurchaseAccountVm);
        }

        public ActionResult AddPurchaseAccountStep6() // Payment 
        {
            var purchaseAcc = TempAccountAsAddPurchaseAccountVM();
            purchaseAcc.Payment = purchaseAcc.TotalToPay;
            return View(purchaseAcc);
        }

        [HttpPost]
        public ActionResult AddPurchaseAccountStep6(AddPurchaseAccountVM mAddPurchaseAccountVm)
        {
            if (ModelState.IsValid)
            {
                var purAcc = TempAccountAsAddPurchaseAccountVM();
                if (TryUpdateModel(purAcc))
                {
                    var newAccount = new PurchaseAccount
                    {
                        AlternateAddressInstructions = purAcc.AlternateAddressInstructions,
                        AlternateAddress = purAcc.AlternateAddress,
                        CustomerId = purAcc.CustomerId,
                        Customer = _customerService.GetCustomer(purAcc.CustomerId),
                        OneOffItems = new List<OneOffItem>(),
                        PurchasedUnits = new List<PurchaseUnit>(),
                    };

                    foreach (var unit in purAcc.PurchasedUnits)
                    {
                        var newUnit = new PurchaseUnit
                        {
                            PurchasedDate = unit.PurchasedDate,
                            RetailCost = unit.RetailCost,
                            DiscountedAmount = unit.DiscountedAmount,
                            //Stock = unit.Stock,
                            StockId = unit.StockId,
                            Contracts = unit.Contracts.Select(c => new ServiceContract
                            {
                                ContractTypeId = c.ContractTypeId,
                                //ContractType = c.ContractType,
                                ContractLengthInMonths = c.ContractLengthInMonths,
                                StartDate = c.StartDate,
                                Charge = c.Charge,
                                ExpiryDate = c.ExpiryDate,
                                PaymentPeriodId = c.PaymentPeriodId,
                            }).ToList(),
                        };
                        newAccount.PurchasedUnits.Add(newUnit);
                    }
                    foreach (var oneOffItem in purAcc.OneOffItems)
                    {
                        var newOneOffItem = new OneOffItem
                        {
                            Description = oneOffItem.Description,
                            Charge = oneOffItem.Charge,
                            Date = purAcc.StartDate,
                            Quantity = oneOffItem.Quantity,
                            OneOffTypeId = oneOffItem.OneOffTypeId
                        };
                        newAccount.OneOffItems.Add(newOneOffItem);
                    }
                    if (ExecuteRepositoryAction(() =>
                    {
                        _accountService.AddTempAccount(newAccount);
                        _accountService.CommitChanges();
                    }))
                    {
                        Session["_accountId"] = newAccount.CustomerAccountId;
                        return RedirectToAction(Stepper2.NextStep());
                    }
                }
            }
            return View(mAddPurchaseAccountVm);
        }

        public ActionResult AddPurchaseAccountStep7() // Accept Contract View/Print and finish
        {
            var id = (int)Session["_accountId"];
            return View(new AcceptAccountVM { CustomerAccountId = id });
        }

        [HttpPost]
        public ActionResult AddPurchaseAccountStep7(AcceptAccountVM mAcceptAccountVm) // Accept Contract View/Print and finish
        {
            if (ModelState.IsValid)
            {
                var newAccount = _accountService.GetAccount(mAcceptAccountVm.CustomerAccountId);
                if (ExecuteRepositoryAction(() =>
                {
                    newAccount.IsTemp = false;
                    _accountService.UpdateAccount(newAccount);
                    _accountService.CommitChanges();
                }))
                    return RedirectToAction("Edit", "Customer",
                                            new
                                            {
                                                id = newAccount.CustomerId,
                                                moveOnToEditAccount = newAccount.CustomerAccountId
                                            });
            }
            return View(mAcceptAccountVm);
        }

        #region Add Purchase Account Helpers

        [HttpPost]
        public ActionResult _UpdateAddPurchaseInstallationCharges(int id, bool status)
        {
            var purchaseAcc = TempAccountAsAddPurchaseAccountVM();
            if (status)
            {
                if (purchaseAcc.OneOffItems == null) purchaseAcc.OneOffItems = new List<OneOffItem>();
                //purchaseAcc.ItemCharges.Add(_itemChargeService.GetItemCharge(id));
            }
            else
            {
                purchaseAcc.OneOffItems.Remove(purchaseAcc.OneOffItems.Single(c => c.OneOffItemId == id));
            }
            return _GetAddPurchaseAccount();
        }

        public ActionResult _AddPurchaseUnit(int id)
        {
            var purchaseAccount = TempAccountAsAddPurchaseAccountVM();
            var ru = new PurchaseUnit();
            ru.Stock = _stockService.GetStock(id);
            ru.StockId = id;
            ru.PurchasedDate = purchaseAccount.StartDate;
            ru.UnitId = purchaseAccount.PurchasedUnits.NextIndex(c => c.UnitId);
            ru.RetailCost = ru.Stock.Model.RetailPrice;
            if (ru.Stock.CustomerAccountId > 0)
                return ReturnJsonError("Unit selected is attached to another account!");
            if (purchaseAccount.PurchasedUnits.Any(u => u.StockId == id))
                return ReturnJsonFormSuccess();
            purchaseAccount.PurchasedUnits.Add(ru);
            AddContractsToUnits(purchaseAccount);
            return ReturnJsonFormSuccess();
        }

        private void AddContractsToUnits(AddPurchaseAccountVM purchaseAcc)
        {
            foreach (var unit in purchaseAcc.PurchasedUnits)
            {
                if (unit.Contracts == null)
                {
                    unit.Contracts = new List<ServiceContract>();
                    foreach (var contract in _contractService.GetDefaultContractsForModel(unit.Stock.ModelId))
                    {
                        var startIndexFrom = purchaseAcc.PurchasedUnits.Count * 1000;
                        var serviceContract =
                            _accountService.CreateServiceContractFromContract(
                                unit.Contracts.NextIndex(c => c.ContractId, startIndexFrom), purchaseAcc.StartDate,
                                contract);
                        unit.Contracts.Add(serviceContract);
                    }
                }
            }
        }


        public ActionResult _GetTempPurchaseAccountTotals()
        {
            var pa = TempAccountAsAddPurchaseAccountVM();
            return Json(new { TotalUnits = pa.TotalUnits }, JsonRequestBehavior.AllowGet);
        }

        

        //public ActionResult _EditTempPurchaseServiceContract(int id)
        //{
        //    var contract =
        //        GetPurchaseAndContractGridClass().GetPurchaseUnitContract(id);
        //    var contVM = Mapper.Map<ServiceContract, EditServiceContractVM>(contract);
        //    return PartialView("../Account/_EditServiceContractPaymentPeriod", contVM);
        //}

        //[HttpPost]
        //public ActionResult _EditTempPurchaseServiceContract(EditServiceContractVM mEditServiceContractVm)
        //{
        //    var contract =
        //        GetPurchaseAndContractGridClass().GetPurchaseUnitContract(mEditServiceContractVm.ContractId);
        //    if (TryUpdateModel(contract))
        //    {
        //        contract.PaymentPeriodId = mEditServiceContractVm.PaymentPeriodId;
        //        contract.PaymentPeriod = _contractService.GetPaymentPeriodType(contract.PaymentPeriodId);
        //        if (GetPurchaseAndContractGridClass().UpdatePurchaseUnitContract(contract))
        //            return ReturnJsonFormSuccess();
        //    }
        //    return PartialView("../Account/_EditServiceContractPaymentPeriod", mEditServiceContractVm);
        //}

        private AddPurchaseAccountVM TempAccountAsAddPurchaseAccountVM()
        {
            return TempAccount as AddPurchaseAccountVM;
        }

        private AddRentAccountVM TempAccountAsAddRentAccountVM()
        {
            return TempAccount as AddRentAccountVM;
        }

        private CustomerAccount TempAccount
        {
            get { return TempCustomerAccount.TempAccount; }
            set { TempCustomerAccount.TempAccount = value; }
        }

        //public ActionResult _AddUnitContracts(int id)
        //{
        //    var unit = GetPurchaseAndContractGridClass().GetPurchaseUnit(id);
        //    var availableContracts = GetAvailableContracts(unit);
        //    var purchaseDate = unit.PurchasedDate;
        //    var minDate = unit.PurchasedDate;
        //    var vm = BuildSelectContractTypesVM(id, availableContracts, purchaseDate, minDate);
        //    return PartialView("../Account/_SelectServiceContractsForModel", vm);
        //}

        //private SelectContractTypesVM BuildSelectContractTypesVM(int unitId, IEnumerable<ContractType> avilableContracts,
        //    DateTime purchaseDate, DateTime minDate)
        //{
        //    var vm = new SelectContractTypesVM { UnitId = unitId, ContractTypes = new List<ContractTypeVM>(), PurchaseDate = purchaseDate, MinDate = minDate };
        //    vm.ContractTypes = GetContractTypesVM(avilableContracts);
        //    return vm;
        //}

        //private static List<ContractTypeVM> GetContractTypesVM(IEnumerable<ContractType> avilableContracts)
        //{
        //    return AutoMapperSetup.MapList<ContractType, ContractTypeVM>(avilableContracts).ToList();
        //}

        //private IEnumerable<ContractType> GetAvailableContracts(PurchaseUnit unit)
        //{
        //    return _contractService.GetContractsForModel(unit.Stock.ModelId).Where(
        //        b => unit.Contracts.All(c => c.ContractTypeId != b.ContractTypeId)).ToList();
        //}


        //[HttpPost]
        //public ActionResult _AddUnitContracts(SelectContractTypesVM mSelectContractTypeVm)
        //{
        //    var unit =
        //            GetPurchaseAndContractGridClass().GetPurchaseUnit(mSelectContractTypeVm.UnitId);

        //    if (ModelState.IsValid)
        //    {
        //        if (mSelectContractTypeVm.ContractTypes != null)
        //            foreach (var selectedContract in mSelectContractTypeVm.ContractTypes)
        //            {
        //                var contractType = _contractService.GetContractType(selectedContract.ContractTypeId);
        //                var svcContract =
        //                    CreateServiceContractFromContract(unit.Contracts.NextIndex(c => c.ContractId),
        //                        mSelectContractTypeVm.PurchaseDate, contractType);
        //                unit.Contracts.Add(svcContract);

        //            }
        //        if (GetPurchaseAndContractGridClass().UpdatePurchaseUnit(unit))
        //            return ReturnJsonFormSuccess();
        //    }
        //    mSelectContractTypeVm.ContractTypes = GetContractTypesVM(GetAvailableContracts(unit));
        //    return PartialView("../Account/_SelectServiceContractsForModel", mSelectContractTypeVm);
        //}

        //private static IPurchaseUnitAndContractsDataSource GetPurchaseAndContractGridClass()
        //{
        //    return PurchaseUnitAndContractsGridClassFactory.GetClass();
        //}


        [HttpPost]
        public ActionResult _AddAccountAddItemCharges(List<OneOffItem> ItemCharges)
        {
            foreach (var mItemCharge in ItemCharges)
            {
                //(TempAccount as IAddAccountVM).ItemCharges.Add(_itemChargeService.GetItemCharge(mItemCharge.OneOffItemId));
            }
            return ReturnJsonFormSuccess();
        }

        #region TempOneOffItems

        public ActionResult _AvailableOneOffCharges()
        {
            var oneOffItems =
                _itemChargeService.GetItemCharges()
                                  .Select(
                                      c =>
                                      new
                                      {
                                          c.ItemChargeId,
                                          Description = c.Description + " " + c.Amount.ToString("C")
                                      }).ToList();
            oneOffItems = AddAllSelector(new { ItemChargeId = -1, Description = "- Select To Add -" },
                                         oneOffItems);
            return new JsonResult { Data = new SelectList(oneOffItems, "ItemChargeId", "Description") };
        }

     
        [GridAction]
        public ActionResult _DisplayItemCharges()
        {
            var itemCharges = TempAccount.OneOffItems;
            return View(new GridModel(itemCharges));
        }

    

        public ActionResult _EditItemCharge(int id)
        {
            var oneOffItem = TempAccount.OneOffItems.Single(c => c.OneOffItemId == id);
            return PartialView(oneOffItem);
        }

        [HttpPost]
        public ActionResult _EditItemCharge(OneOffItem mOneOffItem)
        {
            var oneOffItem = TempAccount.OneOffItems.Single(c => c.OneOffItemId == mOneOffItem.OneOffItemId);
            if (TryUpdateModel(oneOffItem))
            {
                return ReturnJsonFormSuccess();
            }
            return PartialView(mOneOffItem);
        }

        public ActionResult _DeleteItemCharge(int id)
        {
            var itemCharge = TempAccount.OneOffItems.Single(c => c.OneOffItemId == id);
            var promptVm = new PromptVM(id, new DialogPrompt("Do you wish to remove Item Charge '" + itemCharge.Description, PromptMessageAlertLevel.Warning));
            return PartialView("_PromptDialog", promptVm);
        }

        [HttpPost]
        public ActionResult _DeleteItemCharge(PromptVM promptVm)
        {
            TempAccount.OneOffItems.RemoveAll(c => c.OneOffItemId == promptVm.RecordIndex);
            return ReturnJsonFormSuccess();
        }

        [HttpPost]
        public ActionResult _AddOneOffCharge(int id, int qty = 1)
        {
            var presetOneOfItem = _itemChargeService.GetItemCharge(id);
            var oneOfItem = CreateOneOfItemFromPresetOneOffCharges(presetOneOfItem);
            oneOfItem.OneOffItemId = TempAccount.OneOffItems.NextIndex(c => c.OneOffItemId);
            oneOfItem.Quantity = qty;
            TempAccount.OneOffItems.Add(oneOfItem);
            return ReturnJsonFormSuccess();
        }


        #endregion

        [OutputCache(Duration = 0)]
        public ActionResult _GetAddPurchaseAccount()
        {
            var tempAccountAsAddPurchaseAccountVm = TempAccountAsAddPurchaseAccountVM();
            return Json(tempAccountAsAddPurchaseAccountVm, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public ActionResult _GetAddRentAccount()
        {
            return Json(TempAccountAsAddRentAccountVM(), JsonRequestBehavior.AllowGet);
        }



        #region AlternateAddress

        [OutputCache(Duration = 0)]
        public ActionResult _EditStepperAlternateAddress()
        {
            var cu = TempAccount;
            return PartialView("../Account/_EditAlternateAddress", Mapper.Map<CustomerAccount, AlternateAddressVM>(cu));
        }

        [HttpPost]
        public ActionResult _EditStepperAlternateAddress(AlternateAddressVM mAlternateAddressVm)
        {
            var acc = TempAccount;
            if (TryUpdateModel(acc))
            {
                return ReturnJsonFormSuccess();
            }
            return PartialView("../Account/_EditAlternateAddress", mAlternateAddressVm);
        }

        public ActionResult _DeleteAlternateAddress()
        {
            var prompt = new PromptVM(0, new DialogPrompt("Delete Alternate Address & Instructions"));
            return PartialView("_PromptDialog", prompt);
        }

        [HttpPost]
        public ActionResult _DeleteAlternateAddress(PromptVM mPromptVM)
        {
            var acc = TempAccount;
            acc.AlternateAddress = new Address();
            acc.AlternateAddressInstructions = null;
            return ReturnJsonFormSuccess();
        }

        public ActionResult _DisplayAlternateAddress()
        {
            var ac = TempAccount;
            return PartialView("../Account/_AlternateAddress",ac);
        }
       
        #endregion

        #endregion

        #endregion


        public ActionResult CancelStepper()
        {
            return RedirectToAction("Edit", "Customer",
                                    new { id = TempRepository<NewAccountVM>.Entity.CustomerId });
        }

        #endregion

    }
}
