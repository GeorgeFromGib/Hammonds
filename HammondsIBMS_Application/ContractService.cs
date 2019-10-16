using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Repositories;
using HammondsIBMS_Domain.Values;


namespace HammondsIBMS_Application
{


    public class ContractService : ServiceBase
    {
        private readonly StockService _stockService;
        private readonly ICustomerAccountRepository _contractUnitRepsoitory;
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IPaymentPeriodRepository _paymentPeriodRepository;
        private readonly IRentedUnitRepository _rentedUnitRepository;
        private readonly IPostedRepository _postedRepository;
        private readonly MiscService _miscService;

        public ContractService(IUnitOfWork unitOfWork, StockService stockService,
                               ICustomerAccountRepository contractUnitRepository,
                               IContractTypeRepository contractTypeRepository,
                               IContractRepository contractRepository,
                               IPaymentPeriodRepository paymentPeriodRepository,
                               IRentedUnitRepository rentedUnitRepository,
                               IPostedRepository postedRepository,MiscService miscService)
            : base(unitOfWork)
        {
            _stockService = stockService;
            _contractUnitRepsoitory = contractUnitRepository;
            _contractTypeRepository = contractTypeRepository;
            _contractRepository = contractRepository;
            _paymentPeriodRepository = paymentPeriodRepository;
            _rentedUnitRepository = rentedUnitRepository;
            _postedRepository = postedRepository;
            _miscService = miscService;
        }

        //public Stock GetStock(int id)
        //{
        //    return _stockService.GetStock(id);
        //}

        //public IEnumerable<Stock> FindStock(Expression<Func<Stock, bool>> where)
        //{
        //    return _stockService.FindStock(where);
        //}

        public CustomerAccount GetCustomerAccount(int id)
        {
            return _contractUnitRepsoitory.GetById(id);
        }

        public IEnumerable<ContractType> GetContractTypes()
        {
            return _contractTypeRepository.GetAll();
        }

        public void UpdatePurchaseAccountStock(PurchaseAccount purchaseAccount, int oldStockId,
                                                    int oldStockProductCycleId)
        {

            //purchaseAccount.Stock = _stockService.GetStock(purchaseAccount.StockId);
            throw new NotImplementedException("SetStockAppropriateLifeCycle not called");
            //purchaseAccount.SetStockAppropriateLifeCycle();
            //_stockService.UpdateStock(purchaseAccount.Stock);
            UpdateCustomerAccount(purchaseAccount);

            _stockService.ChangeStockLifeCycle(oldStockId, oldStockProductCycleId);
            _stockService.SetCustomerAccountId(null,oldStockId);
        }

        public void UpdateCustomerAccount(CustomerAccount entity)
        {
            _contractUnitRepsoitory.Update(entity);
        }

        public void UpdateRentalContractUnit(RentalAccount entity, int oldPaymentPeriodId)
        {
            _contractUnitRepsoitory.Update(entity);
        }

        public void UpdateRentalContractUnitStock(RentedUnit rentedUnit,int oldStockId,int oldStockProductCycleId)
        {
            var cu = _contractUnitRepsoitory.GetById(rentedUnit.CustomerAccountId) as RentalAccount;
            rentedUnit.Stock = _stockService.GetStock(rentedUnit.StockId);
            cu.UpdateRentedUnitAndUpdateContract(rentedUnit);
            _contractUnitRepsoitory.Update(cu);

            _stockService.ChangeStockLifeCycle(oldStockId, oldStockProductCycleId);
            _stockService.SetCustomerAccountId(null, oldStockId);
        }

        public Contract GetContract(int id)
        {
            return _contractRepository.GetById(id);
        }

        public IEnumerable<ContractType> GetDefaultContractsForModel(int modelId)
        {
            return GetContractsForModel(modelId, c => c.PurchaseDefault);
        }

        public IEnumerable<ContractType> GetContractsForModel(int modelId)
        {
            return GetContractsForModel(modelId, c=>true);
        }

        private IEnumerable<ContractType> GetContractsForModel(int modelId,Func<ContractType,bool>  contractTypeWhereFunc  )
        {
            var mdlContracts = _stockService.GetModel(modelId).ModelSpecificContracts;
            var contracts = _contractTypeRepository.GetAll().ToList();
            
            foreach (var contractType in contracts)
            {
                ModelSpecificContract serviceContract;
                if ((serviceContract = mdlContracts.SingleOrDefault(c => c.ContractTypeId == contractType.ContractTypeId)) != null)
                {
                    contractType.NormalTermAmount = serviceContract.NormalTermAmount;
                    contractType.PeriodInMonths = serviceContract.PeriodInMonths;
                    contractType.PurchaseDefault = serviceContract.Default;
                }
            }

            var contractsToReturn = contracts.Where(contractTypeWhereFunc).ToList();
            return contractsToReturn;
        }


        public void UpdateServiceContract(ServiceContract entity)
        {
            _contractRepository.Update(entity);
        }

        //public void AddServiceContract(ServiceContract entity)
        //{
        //    var cu = _contractUnitRepsoitory.GetById(entity.CustomerAccountId);
        //    cu.AddContract(entity);
        //    _contractUnitRepsoitory.Update(cu);
        //}

        public void AddContractUnit(PurchaseAccount entity)
        {
            _contractUnitRepsoitory.Add(entity);
            //entity.Stock = _stockService.GetStock(entity.StockId);
            throw new NotImplementedException("SetStockAppropriateLifeCycle not called");
            //entity.SetStockAppropriateLifeCycle();
           // _stockService.SetCustomerAccountId(entity.CustomerAccountId,entity.StockId);
           // _stockService.UpdateStock(entity.Stock);
        }

        //public void AddContractUnit(RentalAccount entity)
        //{
        //    _contractUnitRepsoitory.Add(entity);
        //}

        //public void RefreshContractUnit(CustomerAccount entity)
        //{
        //    _contractUnitRepsoitory.Refresh(entity);
        //}

        //public IEnumerable<CustomerAccount> GetContractUnits()
        //{
        //    return _contractUnitRepsoitory.GetAll();
        //}

        public IEnumerable<PaymentPeriod> GetPaymentPeriodTypes()
        {
            return _paymentPeriodRepository.GetAll();
        }

        public ContractType GetContractType(int id)
        {
            return _contractTypeRepository.GetById(id);
        }

        public PaymentPeriod GetPaymentPeriodType(int id)
        {
            return _paymentPeriodRepository.GetById(id);
        }

        public void UpdateContractType(ContractType contractType)
        {
            _contractTypeRepository.Update(contractType);
        }

        public void AddContractType(ContractType contractType)
        {
            _contractTypeRepository.Add(contractType);
        }

        public void UpdatePaymentPeriod(PaymentPeriod mPaymentPeriod)
        {
            _paymentPeriodRepository.Update(mPaymentPeriod);
        }

        public void AddPaymentPeriod(PaymentPeriod mPaymentPeriod)
        {
            _paymentPeriodRepository.Add(mPaymentPeriod);
        }

        public IEnumerable<ContractType> GetActiveContractTypes()
        {
            return _contractTypeRepository.GetMany(t => t.Expired == false);
        }


        //public void AddRentedUnit(RentedUnit entity)
        //{
        //    var cu = (RentalAccount)_contractUnitRepsoitory.GetById(entity.CustomerAccountId);
        //    entity.Stock = _stockService.GetStock(entity.StockId);
        //    cu.AddRentedUnitAndUpdateContract(entity);
        //   _contractUnitRepsoitory.Update(cu);
        //}

        //public RentedUnit GetRentedUnit(int id)
        //{
        //    return _rentedUnitRepository.GetById(id);
        //}

        //public void UpdateRentedUnit(RentedUnit ru)
        //{
        //    var cu = (RentalAccount)_contractUnitRepsoitory.GetById(ru.CustomerAccountId);
        //    cu.UpdateRentedUnitAndUpdateContract(ru);
        //    _contractUnitRepsoitory.Update(cu);
        //}

        //public void DeleteRentedUnitStock(RentedUnit rentedUnit)
        //{
        //    var cu = (RentalAccount)_contractUnitRepsoitory.GetById(rentedUnit.CustomerAccountId);
        //    cu.RemoveRentedUnit(rentedUnit);
        //    _contractUnitRepsoitory.Update(cu);
        //}

        public void UpdateRentalContract(RentalContract rc)
        {
            //var cur = _contractUnitRepsoitory.GetById(rc.CustomerAccountId) as ContractUnitRental;
            //TODO Review. rc.ChangePaymentPeriod(_paymentPeriodRepository.GetById(rc.PaymentPeriodId));
            //cur.RentedContract = rc;
            //_contractUnitRepsoitory.Update(cur);
        }

        
        public void ChangeContractPaymentPeriod(int contractId,int newPaymentPeriodId)
        {
            var contract = _contractRepository.GetById(contractId);
            //TODO Review. contract.ChangePaymentPeriod(_paymentPeriodRepository.GetById(newPaymentPeriodId));

        }

        public void ExpireContract(Contract sc)
        {
            //TODO Review. sc.ExpireContract();
            _contractRepository.Update(sc);

        }

        //public List<ProductLifeCycle> GetChangeableProductLifeCylesForAccountType(CustomerAccount customerAccount)
        //{
        //    var lca = _contractUnitRepsoitory.GetChangeableLifeCycleForAccount(customerAccount).ToList();
        //    var plc=lca.Select(m => m.ProductLifeCycle).ToList();
        //    return plc;
        //}

        public int GetContractExpiryWarningDays()
        {
            return _miscService.GetContractExpiryPeriod();
        }
    }
}