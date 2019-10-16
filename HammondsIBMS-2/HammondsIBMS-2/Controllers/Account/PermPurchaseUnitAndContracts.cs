using System;
using System.Collections.Generic;
using System.Linq;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.Controllers
{
    public class PermPurchaseUnitAndContracts : IPurchaseUnitAndContractsDataSource
    {
        private readonly CustomerAccountService _customerAccountService;

        public PermPurchaseUnitAndContracts(CustomerAccountService customerAccountService)
        {
            _customerAccountService = customerAccountService;
        }

        public List<PurchaseUnit> GetPurchaseUnits(int accountId)
        {
            return GetPurchaseAccount(accountId).PurchasedUnits;
        }

        public List<ServiceContract> GetPurchaseUnitContracts(int unitId)
        {
            return GetPurchaseUnit(unitId).Contracts;
        }

        public PurchaseUnit GetPurchaseUnit(int id)
        {
            return _customerAccountService.GetUnit(id) as PurchaseUnit;
        }

        public bool UpdatePurchaseUnit(PurchaseUnit purchaseUnit)
        {
            _customerAccountService.UpdateUnit(purchaseUnit);
            _customerAccountService.CommitChanges();
            return true;
        }

        public void AddContractToPurchaseUnit(PurchaseUnit purhaseUnit, ServiceContract serviceContract)
        {
            _customerAccountService.AddContractToPurchaseUnit(purhaseUnit, serviceContract);
            _customerAccountService.CommitChanges();
        }

        public bool DeletePurchaseUnit(int unitId)
        {
            throw new NotImplementedException();
        }

        public ServiceContract GetPurchaseUnitContract(int contractId)
        {
            return
                _customerAccountService.GetAccountUnits()
                    .OfType<PurchaseUnit>()
                    .Single(c => c.Contracts.Any(v=>v.ContractId==contractId)).Contracts.Single(c=>c.ContractId==contractId);
        }

        public bool DeletePurchaseUnitContract(int contractId)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePurchaseUnitContract(ServiceContract serviceContract)
        {
            throw new NotImplementedException();
        }

        private PurchaseAccount GetPurchaseAccount(int accountId)
        {
            return _customerAccountService.GetAccount(accountId) as PurchaseAccount;
        }
    }
}