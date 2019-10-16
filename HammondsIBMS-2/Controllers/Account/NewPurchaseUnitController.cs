using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.ViewModels.Basket;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_2.Controllers.Account
{
    public class NewPurchaseUnitController : NewUnitBaseController
    {
        private readonly ContractService _contractService;
        private readonly AccountTransactionService _accountTransactionService;
        private readonly StockService _stockService;

        public NewPurchaseUnitController(CustomerAccountService accountService, ItemChargeService itemChargeService,
            CustomerService customerService, StockService stockService, ContractService contractService,AccountTransactionService accountTransactionService)
            : base(accountService, itemChargeService, customerService)
        {
            _stockService = stockService;
            _contractService = contractService;
            _accountTransactionService = accountTransactionService;
        }

        public ActionResult AddPurchaseUnitBasket(int customerAccountId)
        {
            var basket = new PurchaseBasket {CustomerAccountId = customerAccountId};
            _accountService.AddBasket(basket);
            _accountService.CommitChanges();
            BasketId = basket.BasketId;
            BuildPurchaseAccountStepper();
            return RedirectToAction(Stepper2.FirstStep());
        }

        public override ActionResult AddUnitBasket(int customerAccountId)
        {
            var basket = new PurchaseBasket { CustomerAccountId = customerAccountId };
            _accountService.AddBasket(basket);
            _accountService.CommitChanges();
            BasketId = basket.BasketId;
            BuildPurchaseAccountStepper();
            return RedirectToAction(Stepper2.FirstStep());
        }

        public override ActionResult EditBasketForAccount(int customerAccountId)
        {
            BasketId = _accountService.GetBaskets().Single(c => c.CustomerAccountId == customerAccountId).BasketId;
            BuildPurchaseAccountStepper();
            //Stepper2.DeleteStep(1);
            return RedirectToAction(Stepper2.FirstStep());
        }

        private void BuildPurchaseAccountStepper()
        {
            Stepper2.CreatePageStepper("CancelStepper", null);
            Stepper2.Header = string.Format("Customer {0} - Add Purchase Units (basket)",
                _customerService.GetCustomer(GetBasket().CustomerAccountId).SurnameFirstName);
            Stepper2.ClearSteps();
            Stepper2.AddStep("AddPurchaseUnitStep1");
            Stepper2.AddStep("AddPurchaseUnitStep3");
            Stepper2.AddStep("AddPurchaseUnitStep4");
            Stepper2.AddStep("AddPurchaseUnitStep6");
            Stepper2.AddStep("AddPurchaseUnitStep7");
        }

        public ActionResult CancelStepper()
        {
            return RemoveBasketAndReturnToAccountPage(true);
        }


        public ActionResult _AddPurchaseUnit(int id)
        {
            PurchaseBasket basket = GetPurchaseBasket();
            Stock stock = _stockService.GetStock(id);
            var ru = new PurchaseUnit
            {
                Stock = stock,
                StockId = id,
                PurchasedDate = basket.StartDate,
                //UnitId = basket.PurchasedUnits.NextIndex(c => c.UnitId),
                RetailCost = stock.Model.RetailPrice,
                CustomerAccountId = basket.CustomerAccountId
            };
            if ((ru.Stock.ProductLifeCycleStatus.ProductLifeCycleActions & ProductLifeCycleActions.CanBeSold)!=ProductLifeCycleActions.CanBeSold)
                return ReturnJsonError("Unit selected is not available. Status is : " + ru.Stock.ProductLifeCycleStatus.Description);
            if (basket.PurchasedUnits.Any(u => u.StockId == id))
                return ReturnJsonFormSuccess();
            basket.PurchasedUnits.Add(ru);
            AddContractsToUnits(basket);
            UpdateBasket(basket);
            return ReturnJsonFormSuccess();
        }

        private void AddContractsToUnits(PurchaseBasket purchaseBasket)
        {
            foreach (PurchaseUnit unit in purchaseBasket.PurchasedUnits)
            {
                if (unit.Contracts == null)
                {
                    unit.Contracts = new List<ServiceContract>();
                    foreach (ContractType contract in _contractService.GetDefaultContractsForModel(unit.Stock.ModelId))
                    {
                        int startIndexFrom = purchaseBasket.PurchasedUnits.Count*1000;
                        ServiceContract serviceContract =
                            _accountService.CreateServiceContractFromContract(
                                unit.Contracts.NextIndex(c => c.ContractId, startIndexFrom), purchaseBasket.StartDate,
                                contract);
                        unit.Contracts.Add(serviceContract);
                    }
                }
            }
        }


        [OutputCache(Duration = 0)]
        public ActionResult _GetPurchaseBasketTotals()
        {
            PurchaseBasketVM basket = GetPurchaseBasketVM();
            return Json(new
            {
                basket.TotalCharge,
                basket.TotalContracts,
                basket.TotalItemCharges,
                basket.TotalToPay,
                basket.TotalUnits,
            }
                , JsonRequestBehavior.AllowGet);
        }


        #region Add Purchase Unit Steps

        public ActionResult AddPurchaseUnitStep1()
        {
            return View(GetPurchaseBasketVM());
        }

        [HttpPost]
        public ActionResult AddPurchaseUnitStep1(PurchaseBasketVM mBasketVM)
        {
            PurchaseBasket basket = GetPurchaseBasket();
            basket.StartDate = mBasketVM.StartDate;
            UpdateBasket(basket);
            return RedirectToAction(Stepper2.NextStep());
        }

        public ActionResult AddPurchaseUnitStep3()
        {
            return View(GetPurchaseBasketVM());
        }

        [HttpPost]
        public ActionResult AddPurchaseUnitStep3(PurchaseBasketVM mPurchaseBasketVM)
        {
            if (!ModelState.IsValid)
            {
                return View(mPurchaseBasketVM);
            }
            PurchaseBasket basket = GetPurchaseBasket();
            if (basket.PurchasedUnits == null || basket.PurchasedUnits.Count == 0)
            {
                ModelState.AddModelError("", "No Unit has been selected!");
                return View(mPurchaseBasketVM);
            }

            return RedirectToAction(Stepper2.NextStep());
        }

        public ActionResult AddPurchaseUnitStep4() //Setup Item Charges
        {
            PurchaseBasket purchaseAcc = GetPurchaseBasket();
            if (purchaseAcc.OneOffItems == null || purchaseAcc.OneOffItems.Count == 0)
                purchaseAcc.OneOffItems = InitiateOneOffItems(c => c.PurchaseDefault);
            purchaseAcc.OneOffItems.ForEach(o => o.Quantity = purchaseAcc.PurchasedUnits.Count);
            return View(GetPurchaseBasketVM());
        }

        [HttpPost]
        public ActionResult AddPurchaseUnitStep4(PurchaseBasketVM mPurchaseBasketVM)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mPurchaseBasketVM);
        }

        public ActionResult AddPurchaseUnitStep6() //Setup Item Charges
        {
            PurchaseBasketVM basket = GetPurchaseBasketVM();
            basket.Payment = basket.TotalToPay;
            return View(basket);
        }

        [HttpPost]
        public ActionResult AddPurchaseUnitStep6(PurchaseBasketVM mPurchaseBasketVM)
        {
            var basket = GetPurchaseBasket();
            if (TryUpdateModel(basket))
            {
                UpdateBasket(basket);
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mPurchaseBasketVM);
        }

        public ActionResult AddPurchaseUnitStep7() // Accept Contract View/Print and finish
        {
            var basketAcceptance = new AcceptBasketVM
            {
                CustomerAccountId = GetPurchaseBasket().CustomerAccountId,
                AmountPayed = GetPurchaseBasket().TotalAmount,
                FullPayment = GetPurchaseBasket().FullPayment
            };
            return View(basketAcceptance);
        }

        [HttpPost]
        public ActionResult AddPurchaseUnitStep7(AcceptBasketVM mAcceptBasketVM)
            // Accept Contract View/Print and finish
        {
            if (ModelState.IsValid)
            {
                var basket = GetPurchaseBasket();
                if (ExecuteRepositoryAction(() =>
                {
                    UpdatePurchaseAccountFromBasket(basket);
                    UpdateTransactionsFromBasket(basket);
                    _accountService.CommitChanges();
                }))
                    return RemoveBasketAndReturnToAccountPage(false);
            }
            return View(mAcceptBasketVM);
        }

        private void UpdateTransactionsFromBasket(PurchaseBasket basket)
        {
            foreach (var purchase in basket.PurchasedUnits)
            {
                _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Purchase,
                    purchase.Total, AccountTransactionInputType.Charge, purchase.StockId, purchase.Stock.ManufacturerModel);
                _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Service,
                   purchase.TotalContracts, AccountTransactionInputType.Charge, purchase.StockId, purchase.Stock.ManufacturerModel);
                if (basket.FullPayment)
                {
                    _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Purchase,
                        -purchase.Total, AccountTransactionInputType.Payment, purchase.StockId, purchase.Stock.ManufacturerModel);
                    _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Service,
                  -purchase.TotalContracts, AccountTransactionInputType.Payment, purchase.StockId, purchase.Stock.ManufacturerModel);
                }
            }
            _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.OneOff,
                basket.TotalOneOffItems, AccountTransactionInputType.Charge);
            if (basket.FullPayment)
            {
                _accountTransactionService.AddAccountPaymentTransaction(basket.CustomerAccountId, AccountTransactionType.OneOff,
                    basket.TotalOneOffItems);
            }
        }

        private void UpdatePurchaseAccountFromBasket(PurchaseBasket basket)
        {

            var account = _accountService.GetAccount(basket.CustomerAccountId) as PurchaseAccount;
            foreach (var unit in basket.PurchasedUnits)
            {
                var newUnit = new PurchaseUnit
                {
                    PurchasedDate = unit.PurchasedDate,
                    RetailCost = unit.RetailCost,
                    DiscountedAmount = unit.DiscountedAmount,
                    StockId = unit.StockId,
                    CustomerAccountId = unit.CustomerAccountId,
                    Contracts = unit.Contracts.Select(c => new ServiceContract
                    {
                        ContractTypeId = c.ContractTypeId,
                        ContractLengthInMonths = c.ContractLengthInMonths,
                        StartDate = c.StartDate,
                        Charge = c.Charge,
                        ExpiryDate = c.ExpiryDate,
                        PaymentPeriodId = c.PaymentPeriodId,
                        
                    }).ToList(),
                };
                account.PurchasedUnits.Add(newUnit);
            }
            foreach (var oneOffItem in basket.OneOffItems)
            {
                var newOneOffItem = new OneOffItem
                {
                    Description = oneOffItem.Description,
                    Charge = oneOffItem.Charge,
                    Date = basket.StartDate,
                    Quantity = oneOffItem.Quantity,
                    OneOffTypeId = oneOffItem.OneOffTypeId
                };
                account.OneOffItems.Add(newOneOffItem);
            }

            _accountService.UpdateAccount(account);
        }


        private PurchaseBasketVM GetPurchaseBasketVM()
        {
            return Mapper.Map<PurchaseBasket, PurchaseBasketVM>(GetPurchaseBasket());
        }

        private PurchaseBasket GetPurchaseBasket()
        {
            return GetBasket() as PurchaseBasket;
        }

        #endregion


      
    }
}