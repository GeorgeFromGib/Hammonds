using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Repositories;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Application
{
    public class CustomerAccountService : ServiceBase
    {
        private readonly ICustomerAccountRepository _customerAccountRepository;
        private readonly StockService _stockService;
        private readonly AccountTransactionService _accountTransactionService;
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentPeriodRepository _paymentPeriodRepository;


        public CustomerAccountService(IUnitOfWork unitOfWork, IPaymentPeriodRepository paymentPeriodRepository,
            ICustomerAccountRepository customerAccountRepository,StockService stockService,AccountTransactionService accountTransactionService,IBasketRepository basketRepository) : base(unitOfWork)
        {
            _paymentPeriodRepository = paymentPeriodRepository;
            _customerAccountRepository = customerAccountRepository;
            _stockService = stockService;
            _accountTransactionService = accountTransactionService;
            _basketRepository = basketRepository;
        }


        public CustomerAccount GetAccount(int id)
        {
            return _customerAccountRepository.GetById(id);
        }

        public PaymentPeriod GetPaymentPeriod(int paymentPeriodId)
        {
            return _paymentPeriodRepository.GetById(paymentPeriodId);
        }

        public IEnumerable<PaymentPeriod> GetPaymentPeriodTypes()
        {
            return _paymentPeriodRepository.GetAll();
        }

        public void AddTempAccount(CustomerAccount customerAccount)
        {
            customerAccount.IsTemp = true;
            _customerAccountRepository.Add(customerAccount);
        }

        public void AddAccount(CustomerAccount newAccount)
        {
            SetAccountStockLifeCycle(newAccount);
            _customerAccountRepository.Add(newAccount);
        }

        public void UpdateAccount(CustomerAccount account)
        {
            SetAccountStockLifeCycle(account);
            _customerAccountRepository.Update(account);
        }

        private void SetAccountStockLifeCycle(CustomerAccount account)
        {
            if (account is PurchaseAccount)
            {
                var pacc = account as PurchaseAccount;
                SetPurchaseStockLifeCycle(pacc.PurchasedUnits, ProductLifeCycleStatus.Sold);
            }
            if (account is RentalAccount)
            {
                var racc= account as RentalAccount;
                SetRentalStockLifeCycle(racc.RentedUnits, ProductLifeCycleStatus.Rented);
            }
        }


        private void SetRentalStockLifeCycle(List<RentedUnit> units, ProductLifeCycleStatus productLifeCycleStatus)
        {
            if (units == null) return;
            foreach (var rentedUnit in units)
            {
                if (rentedUnit.Stock == null)
                    rentedUnit.Stock = _stockService.GetStock(rentedUnit.StockId);
                rentedUnit.Stock.ProductLifeCycleId = (int)productLifeCycleStatus;
            }
        }

        private void SetPurchaseStockLifeCycle(List<PurchaseUnit> units,ProductLifeCycleStatus productLifeCycleStatus)
        {
            foreach (var purchasedUnit in units)
            {
                if (purchasedUnit.Stock == null)
                    purchasedUnit.Stock = _stockService.GetStock(purchasedUnit.StockId);
                purchasedUnit.Stock.ProductLifeCycleId = (int)productLifeCycleStatus;
            }
        }

        public void AddRentAccount(RentalAccount newAccount)
        {
            _customerAccountRepository.Add(newAccount);
        }

        public Unit GetUnit(int unitId)
        {
            return GetAccountUnits().Single(u => u.UnitId == unitId);
        }

        public IEnumerable<Unit> GetAccountUnits()
        {
            return _customerAccountRepository.GetUnits();
        }

        public void AddRentedUnit(int accountId,RentedUnit rentedUnit)
        {
            var account = GetAccount(accountId) as RentalAccount;
            account.RentedUnits.Add(rentedUnit);
            rentedUnit.CustomerAccountId = accountId;
            //UpdateRentalAccountContract(account);
            UpdateAccount(account);
            //_customerAccountRepository.Update(account);
        }

        public void UpdateUnit(Unit unit)
        {
            
            if (unit is RentedUnit)
            {
                var rentedUnit = unit as RentedUnit;
                if (rentedUnit.OldAmount != rentedUnit.Amount)
                {
                    var proRata = CalcualteProRata((DateTime) rentedUnit.ChangedDate, (DateTime) rentedUnit.PaidUntil,
                        rentedUnit.Amount - rentedUnit.OldAmount);
                    _accountTransactionService.AddAccountTransaction(rentedUnit.CustomerAccountId,
                        AccountTransactionType.Rental,
                        proRata, AccountTransactionInputType.Adjustment, rentedUnit.StockId,
                        rentedUnit.Stock.ManufacturerModel + " Pro-rata");
                }
            }
           
            _customerAccountRepository.UpdateUnit(unit);
        }


        public void DeleteRentedUnit(RentedUnit rentedUnit, int toProductLifeCycle,bool returnDeposit)
        {
            var account = GetAccount(rentedUnit.CustomerAccountId) as RentalAccount;
            rentedUnit.Stock.ProductLifeCycleId = toProductLifeCycle;

            AddRentUnitRemoveProRata(rentedUnit, account.CustomerAccountId);
            if (returnDeposit)
            {
                _accountTransactionService.AddAccountTransaction(account.CustomerAccountId, AccountTransactionType.DepositReturn,
                    -rentedUnit.Deposit, AccountTransactionInputType.Charge, rentedUnit.StockId, rentedUnit.Stock.ManufacturerModel);
            }

            rentedUnit.PaidUntil = rentedUnit.RemovalDate;
            _customerAccountRepository.Update(account);
        }

        private void AddRentUnitRemoveProRata(RentedUnit rentedUnit, int customerAccountId)
        {
            if (rentedUnit.PaidUntil != null)
            {
                var proErrata = CalcualteProRata((DateTime)rentedUnit.PaidUntil, (DateTime)rentedUnit.RemovalDate, rentedUnit.Amount);
                _accountTransactionService.AddAccountTransaction(customerAccountId, AccountTransactionType.Rental,
                    proErrata, AccountTransactionInputType.Adjustment, rentedUnit.StockId,
                    rentedUnit.Stock.ManufacturerModel + " Pro-rata");
            }
        }

        //private decimal CalcualteProRata(DateTime startDate, DateTime endDate, decimal amount)
        //{
        //    var monthDiff = ((endDate - startDate).Days) /
        //                       ConstantValues.AvgDaysPerMonth;
        //    var proErrata = (decimal)monthDiff * amount;
        //    return proErrata;
        //}

        private decimal CalcualteProRata(DateTime startDate, DateTime endDate, decimal amount,
            int amountPeriodInMonths=1)
        {
            var monthDiff = ((endDate - startDate).Days) /
                              ConstantValues.AvgDaysPerMonth;
            var proErrata = (decimal)monthDiff * (amount/amountPeriodInMonths);
            return proErrata;
        }

        public void SwapRentedUnit(RentedUnit rentedUnit,int newStockId, int lifeCycleForPreviousStock)
        {
            var curStockid = rentedUnit.StockId;
            
            rentedUnit.StockId = newStockId;
            UpdateUnit(rentedUnit);

            var oldStock = _stockService.GetStock(curStockid);
            oldStock.ProductLifeCycleId = lifeCycleForPreviousStock;
            _stockService.UpdateStock(oldStock);
        }

        public void DeletePurchaseUnit(PurchaseUnit purchaseUnit,int toProductLifeCycle, bool refundAmount)
        {
            var account = GetAccount(purchaseUnit.CustomerAccountId) as PurchaseAccount;
            purchaseUnit.Stock.ProductLifeCycleId = toProductLifeCycle;
            if (refundAmount)
            {
                _accountTransactionService.AddAccountTransaction(account.CustomerAccountId, AccountTransactionType.Refund,
                   -purchaseUnit.Total, AccountTransactionInputType.Charge, purchaseUnit.StockId, purchaseUnit.Stock.ManufacturerModel);
            }
            _customerAccountRepository.Update(account);
        }

        public IEnumerable<AccountTypeChangeableLifeCycle> GetChangeableProductLifeCylesForAccount(int accountId)
        {
            CustomerAccount acc = GetAccount(accountId);
            IEnumerable<AccountTypeChangeableLifeCycle> lifeCyclesForAccount =
                _customerAccountRepository.GetChangeableLifeCycles()
                    .Where(c => c.AccountType == (int) acc.AccountType);
            return lifeCyclesForAccount;
        }

        public OneOffItem GetOneOffItem(int id)
        {
            return _customerAccountRepository.GeOneOffItem(id);
        }

        public void RemoveOneOffItem(OneOffItem oneOffItem)
        {
            var account =
               _customerAccountRepository.Get(c => c.OneOffItems.Any(o => o.OneOffItemId == oneOffItem.OneOffItemId));
            if (oneOffItem.Refunded)
            {
                _accountTransactionService.AddAccountTransaction(account.CustomerAccountId, AccountTransactionType.Refund,
                   -oneOffItem.TotalCharge, AccountTransactionInputType.Charge, 0, "One Off :" + oneOffItem.Description);
            }
            oneOffItem.RemovalDate = DateTime.Today;
           
            _customerAccountRepository.Update(account);
        }

        public void ExpireServiceContract(ServiceContract sc)
        {
            if (sc.ProRateRefunded)
            {
                var proRateRefund = sc.PeriodPaymentAmount-CalcualteProRata(sc.StartDate, (DateTime)sc.ExpiryDate, sc.PeriodPaymentAmount,
                    sc.PaymentPeriod.PeriodInMonths);
                var account =
                    _customerAccountRepository.GetAll()
                        .OfType<PurchaseAccount>()
                        .Single(p => p.PurchasedUnits.Any(u => u.Contracts.Any(c => c.ContractId == sc.ContractId)));
                _accountTransactionService.AddAccountTransaction(account.CustomerAccountId, AccountTransactionType.Refund,
                  -proRateRefund, AccountTransactionInputType.Charge, 0, "Service Contract :" + sc.ContractType.Description);
            }
            
        }

        public ServiceContract CreateServiceContractFromContract(int recordIndex, DateTime startDate, ContractType contract)
        {
            var serviceContract = new ServiceContract
            {
                ContractId = recordIndex,
                ContractTypeId = contract.ContractTypeId,
                ContractLengthInMonths = contract.PeriodInMonths,
                PaymentPeriod = GetPaymentPeriod(contract.PaymentPeriodId),
                PaymentPeriodId = contract.PaymentPeriodId,
                Charge = contract.NormalTermAmount,
                ContractType = contract,
                ExpiryDate = CalculateExpiryDate(startDate,contract.PeriodInMonths),
                StartDate = startDate,
            };
            return serviceContract;
        }

        private DateTime CalculateExpiryDate(DateTime startDate,int contractLengthInMonths)
        {
            return startDate.AddDays(contractLengthInMonths * ConstantValues.AvgDaysPerMonth);
        }

        public void AddContractToPurchaseUnit(PurchaseUnit purhaseUnit, ServiceContract serviceContract)
        {
            purhaseUnit.Contracts.Add(serviceContract);
            if (serviceContract.Charge > 0)
            {
                _accountTransactionService.AddAccountTransaction(purhaseUnit.CustomerAccountId, AccountTransactionType.Service,
                 serviceContract.Charge, AccountTransactionInputType.Charge, 0, "Service Contract :" + serviceContract.ContractType.Description);
            }

            var account = GetAccount(purhaseUnit.CustomerAccountId);
            UpdateAccount(account);
        }

        #region Basket

        public Basket GetBasket(int id)
        {
            return _basketRepository.GetById(id);
        }

        public void AddBasket(Basket basket)
        {
            ProcessBasket(basket);
            _basketRepository.Add(basket);
        }

        public void UpdateBasket(Basket basket)
        {
            ProcessBasket(basket);
            _basketRepository.Update(basket);
        }

        private void ProcessBasket(Basket basket)
        {
            if (basket is RentalBasket)
            {
                SetRentalBasketUnitsLifeCycle(basket as RentalBasket, ProductLifeCycleStatus.Reserved);
                
            }
            if (basket is PurchaseBasket)
            {
                SetPurchaseBasketUnitsLifeCycle(basket as PurchaseBasket, ProductLifeCycleStatus.Reserved);
            }
        }

       

        public void DeleteBasket(Basket basket,bool isCancel)
        {
            if (basket is RentalBasket)
            {
                var rentalBasket = basket as RentalBasket;
                if(isCancel) SetRentalBasketUnitsLifeCycle(rentalBasket,ProductLifeCycleStatus.InStock);
                rentalBasket.RentedUnits.Clear();
            }
            if (basket is PurchaseBasket)
            {
                var purchaseBasket = basket as PurchaseBasket;
                if (isCancel) SetPurchaseBasketUnitsLifeCycle(purchaseBasket, ProductLifeCycleStatus.InStock);
                purchaseBasket.PurchasedUnits.Clear();
            }
          
            basket.OneOffItems.Clear();
            _basketRepository.Delete(basket);
        }

        private  void SetRentalBasketUnitsLifeCycle(RentalBasket rentalBasket,ProductLifeCycleStatus productLifeCycleStatus)
        {
            if (rentalBasket.RentedUnits == null) return;

            SetRentalStockLifeCycle(rentalBasket.RentedUnits, productLifeCycleStatus);
           
        }

        private void SetPurchaseBasketUnitsLifeCycle(PurchaseBasket purchaseBasket, ProductLifeCycleStatus productLifeCycleStatus)
        {
            if(purchaseBasket.PurchasedUnits==null) return;

            SetPurchaseStockLifeCycle(purchaseBasket.PurchasedUnits, productLifeCycleStatus);

        }

        public IEnumerable<Basket> GetBaskets()
        {
            return _basketRepository.GetAll();
        }

        #endregion


    }
}