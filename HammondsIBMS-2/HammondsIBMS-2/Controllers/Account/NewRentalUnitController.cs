using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_2.ViewModels.Basket;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Stock;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers.Account
{
    public class NewRentalUnitController : NewUnitBaseController
    {
        private readonly StockService _stockService;
        private readonly MiscService _miscService;
        private readonly AccountTransactionService _accountTransactionService;


        public NewRentalUnitController(CustomerService customerService,CustomerAccountService accountService, StockService stockService, MiscService miscService, ItemChargeService itemChargeService, AccountTransactionService accountTransactionService) : base(accountService,itemChargeService,customerService)
        {
            _stockService = stockService;
            _miscService = miscService;
            _accountTransactionService = accountTransactionService;
        }

       

        public override ActionResult AddUnitBasket(int customerAccountId)
        {
            var basket = new RentalBasket { CustomerAccountId = customerAccountId };
            _accountService.AddBasket(basket);
            _accountService.CommitChanges();
            BasketId = basket.BasketId;
            BuildRentAccountStepper();
            return RedirectToAction(Stepper2.FirstStep());
        }

        public override ActionResult EditBasketForAccount(int customerAccountId)
        {
            BasketId = _accountService.GetBaskets().Single(c => c.CustomerAccountId == customerAccountId).BasketId;
            BuildRentAccountStepper();
            //Stepper2.DeleteStep(1);
            return RedirectToAction(Stepper2.FirstStep());
        }

        private void BuildRentAccountStepper()
        {
            Stepper2.CreatePageStepper("CancelStepper",null);
            Stepper2.Header = string.Format("Customer {0} - Add Rental Units (basket)",
                _customerService.GetCustomer(GetRentalBasket().CustomerAccountId).SurnameFirstName);
            Stepper2.ClearSteps();
            Stepper2.AddStep("AddRentUnitStep1");
            Stepper2.AddStep("AddRentUnitStep2");
            Stepper2.AddStep("AddRentUnitStep3");
            Stepper2.AddStep("AddRentUnitStep5");
            Stepper2.AddStep("AddRentUnitStep6");
        }

        public ActionResult CancelStepper()
        {
            return RemoveBasketAndReturnToAccountPage(true);
        }


        #region Add Rent Account Steps

        public ActionResult AddRentUnitStep1()
        {
            return View(GetRentalBasketVM());
        }

        [HttpPost]
        public ActionResult AddRentUnitStep1(RentalBasketVM mRentalBasketVM)
        {
            var basket = GetRentalBasket();
            basket.StartDate = mRentalBasketVM.StartDate;
            UpdateBasket(basket);
            return RedirectToAction(Stepper2.NextStep());
        }

      

        public ActionResult AddRentUnitStep2()
        {
            return View(GetRentalBasketVM());
        }

        public ActionResult AddRentUnitStep3() // Add One Off Items
        {

            return View(GetRentalBasketVM());
        }

        [HttpPost]
        public ActionResult AddRentUnitStep3(RentalBasketVM mRentalBasketVM)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(Stepper2.NextStep());
            }
            return View(mRentalBasketVM);
        }


        public ActionResult AddRentUnitStep5() // Payment and finish
        {
            var basket = GetRentalBasketVM();
            basket.Payment = basket.TotalToPay;
            basket.FullPayment = true;
            return View(basket);
        }

        [HttpPost]
        public ActionResult AddRentUnitStep5(RentalBasketVM mRentalBasketVM)
        {
            if (ModelState.IsValid)
            {
                var basket = GetRentalBasket();
                if (TryUpdateModel(basket))
                {
                    UpdateBasket(basket);
                    return RedirectToAction(Stepper2.NextStep());
                }
            }
            return View(mRentalBasketVM);
        }

        public ActionResult AddRentUnitStep6() // Accept Contract View/Print and finish
        {
            var basketAcceptance = new AcceptBasketVM { CustomerAccountId = GetRentalBasket().CustomerAccountId,AmountPayed = GetRentalBasket().TotalAmount,FullPayment = GetRentalBasket().FullPayment};
            return View(basketAcceptance);
        }

        [HttpPost]
        public ActionResult AddRentUnitStep6(AcceptBasketVM mAcceptBasketVM) // Accept Contract View/Print and finish
        {
            if (ModelState.IsValid)
            {
                var basket = GetRentalBasket();
                
                if (ExecuteRepositoryAction(() =>
                {
                    UpdateRentalAccountFromBasket(basket);
                    UpdateTransactionsFromBasket(basket);
                    _accountService.CommitChanges();
                }))
                    return RemoveBasketAndReturnToAccountPage(false);
            }
            return View(mAcceptBasketVM);
        }

        private void UpdateTransactionsFromBasket(RentalBasket basket)
        {
            foreach (var rental in basket.RentedUnits)
            {
                _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Rental,
                    rental.Amount, AccountTransactionInputType.Charge, rental.StockId, rental.Stock.ManufacturerModel);
                _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Deposit,
                    rental.Deposit, AccountTransactionInputType.Charge, rental.StockId, rental.Stock.ManufacturerModel);
                if (basket.FullPayment)
                {
                    _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Rental,
                        -rental.Amount, AccountTransactionInputType.Payment, rental.StockId, rental.Stock.ManufacturerModel);
                    _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.Deposit,
                        -rental.Deposit, AccountTransactionInputType.Payment, rental.StockId, rental.Stock.ManufacturerModel);
                }
            }
            _accountTransactionService.AddAccountTransaction(basket.CustomerAccountId, AccountTransactionType.OneOff,
                basket.TotalOneOffItems, AccountTransactionInputType.Charge);
            if (basket.FullPayment)
            {
                _accountTransactionService.AddAccountPaymentTransaction(basket.CustomerAccountId, AccountTransactionType.OneOff,
                    -basket.TotalOneOffItems);
            }
        }

        private void UpdateRentalAccountFromBasket(RentalBasket basket)
        {
            var account = _accountService.GetAccount(basket.CustomerAccountId) as RentalAccount;
            foreach (RentedUnit mRentedUnit in basket.RentedUnits)
            {
                var rentedUnit = new RentedUnit
                {
                    Amount = mRentedUnit.Amount,
                    RentedDate = mRentedUnit.RentedDate,
                    StockId = mRentedUnit.StockId,
                    Stock = _stockService.GetStock(mRentedUnit.StockId),
                    Deposit = basket.FullPayment ? mRentedUnit.Deposit : 0,
                    PaidUntil = mRentedUnit.RentedDate,
                };
                _accountService.AddRentedUnit(account.CustomerAccountId,rentedUnit);
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

        private RentalBasketVM GetRentalBasketVM()
        {
            return Mapper.Map<RentalBasket, RentalBasketVM>(GetRentalBasket());
        }

        private RentalBasket GetRentalBasket()
        {
            return GetBasket() as RentalBasket;
        }

        [HttpPost]
        public ActionResult AddRentUnitStep2(RentalBasketVM mRentalBasketVM)
        {

            var rentAcc = GetRentalBasket();
            if (rentAcc.RentedUnits == null || rentAcc.RentedUnits.Count == 0)
            {
                ModelState.AddModelError("", "No Unit has been selected!");
                return View(mRentalBasketVM);
            }

            //rentAcc.AttachedContracts = new List<Contract>();
            rentAcc.OneOffItems.ForEach(c => c.Quantity = rentAcc.RentedUnits.Count);
            return RedirectToAction(Stepper2.NextStep());
        }

        #endregion

        #region Add Rent Account Helpers

        [GridAction]
        public ActionResult _DisplayRentedUnits()
        {
            var rentedUnits = GetRentalBasket().RentedUnits;
            return View(new GridModel(AutoMapperSetup.MapList<RentedUnit, RentedUnitVM>(rentedUnits)));
        }


        [HttpPost]
        public ActionResult _AddRentedUnit(int id)
        {
            var basket = GetRentalBasket();
            var stk = _stockService.GetStock(id);
            var ru = new RentedUnit
            {
                Stock = stk,
                StockId = id,
                RentedDate = GetRentalBasket().StartDate,
                UnitId = GetRentalBasket().RentedUnits.NextIndex(c => c.UnitId),
                Amount = stk.Model.RentalPrice,
                Deposit = _miscService.GetDefaultDepositForRentalUnits(),
                CustomerAccountId = GetRentalBasket().CustomerAccountId,
            };
            if ((ru.Stock.ProductLifeCycleStatus.ProductLifeCycleActions & ProductLifeCycleActions.CanBeRented)!=ProductLifeCycleActions.CanBeRented)
                return ReturnJsonError("Unit selected is not available. Status is : " + ru.Stock.ProductLifeCycleStatus.Description);
            if (basket.RentedUnits.Any(u => u.StockId == id))
                return ReturnJsonFormSuccess();
            basket.RentedUnits.Add(ru);
            UpdateBasket(basket);
            return ReturnJsonFormSuccess();
        }

      

        public ActionResult _EditRentedUnit(int id)
        {
            var ru = GetRentalBasket().RentedUnits.Single(c => c.UnitId == id);
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
            var basket = GetRentalBasket();
            var ru = basket.RentedUnits.Single(c => c.UnitId == mRentedUnitVm.UnitId);
            if (TryUpdateModel(ru))
            {
                UpdateBasket(basket);
                return ReturnJsonFormSuccess();
            }
            return PartialView("../Account/_EditRentedUnit", mRentedUnitVm);
        }

        public ActionResult _GetRentalAndDepositTotals()
        {
            var totalRental = GetRentalBasketVM().TotalRental;
            var totalDeposit = GetRentalBasketVM().TotalDeposit;
            return Json(new { TotalRental = totalRental, TotalDeposit = totalDeposit }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DeleteBasketAccountStock(int id)
        {
            var ru = GetRentalBasket().RentedUnits.Single(c => c.UnitId == id);
            var prompt = new PromptVM(id, new DialogPrompt(string.Format("Remove model {0} from selection?", ru.Stock.ManufacturerModel)));
            return PartialView("_PromptDialog", prompt);
        }

        [HttpPost]
        public ActionResult _DeleteBasketAccountStock(PromptVM mPromptVm)
        {
            var rentalBasket = GetRentalBasket();
            var ru = rentalBasket.RentedUnits.Single(c => c.UnitId == mPromptVm.RecordIndex);
            ru.Stock.ProductLifeCycleId = (int) ProductLifeCycleStatus.InStock;
            rentalBasket.RentedUnits.Remove(ru);
            UpdateBasket(rentalBasket);
            return ReturnJsonFormSuccess();
        }

        [OutputCache(Duration = 0)]
        public ActionResult _GetRentBasketTotals()
        {
            var basket = GetRentalBasketVM();
            return Json(new
            {
                basket.TotalCharge,
                basket.TotalDeposit,
                basket.TotalItemCharges,
                basket.TotalToPay
            }
                , JsonRequestBehavior.AllowGet);
        }

        #endregion


      
    }
}
