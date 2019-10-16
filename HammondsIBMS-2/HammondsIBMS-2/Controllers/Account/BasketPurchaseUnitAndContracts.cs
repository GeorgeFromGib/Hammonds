using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_2.Controllers.Account
{
    public class BasketPurchaseUnitAndContracts:IPurchaseUnitAndContractsDataSource
    {
        private readonly CustomerAccountService _customerAccountService;

        public BasketPurchaseUnitAndContracts(CustomerAccountService customerAccountService)
        {
            _customerAccountService = customerAccountService;
        }

        public List<PurchaseUnit> GetPurchaseUnits(int basketId)
        {
            return GetPurchaseBasket(basketId).PurchasedUnits;
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
            //_customerAccountService.UpdateUnit(purchaseUnit);
            _customerAccountService.CommitChanges();
            return true;
        }

        public bool DeletePurchaseUnit(int unitId)
        {
            var theBasket = GetBaskets().FirstOrDefault(basket => basket.PurchasedUnits.Any(c => c.UnitId == unitId));
            var unit = GetPurchaseUnit(unitId);
            unit.Stock.ProductLifeCycleId = (int) ProductLifeCycleStatus.InStock;
             theBasket.PurchasedUnits.Remove(unit);
            UpdateBasket(theBasket);
            return true;
        }

        private IEnumerable<PurchaseBasket> GetBaskets()
        {
            return _customerAccountService.GetBaskets().Cast<PurchaseBasket>();
        }

        private void UpdateBasket(PurchaseBasket theBasket)
        {
            _customerAccountService.UpdateBasket(theBasket);
            _customerAccountService.CommitChanges();
        }

        public ServiceContract GetPurchaseUnitContract(int contractId)
        {
            return (from basket in GetBaskets() from unit in basket.PurchasedUnits 
                    where unit.Contracts.Any(c => c.ContractId == contractId) select unit.Contracts.Single(c => c.ContractId == contractId)).FirstOrDefault();
        }

        public bool DeletePurchaseUnitContract(int contractId)
        {
            var contract = GetPurchaseUnitContract(contractId);
            var theUnit=(from basket in GetBaskets()
                from unit in basket.PurchasedUnits
                where unit.Contracts.Any(c => c.ContractId == contractId)
                select unit).FirstOrDefault();
            theUnit.Contracts.Remove(contract);
            _customerAccountService.CommitChanges();
            return true;
        }

        public bool UpdatePurchaseUnitContract(ServiceContract serviceContract)
        {
            _customerAccountService.CommitChanges();
            return true;
        }

        public void AddContractToPurchaseUnit(PurchaseUnit purhaseUnit, ServiceContract serviceContract)
        {
            purhaseUnit.Contracts.Add(serviceContract);
            _customerAccountService.CommitChanges();
        }

        private PurchaseBasket GetPurchaseBasket(int basketId)
        {
            return _customerAccountService.GetBasket(basketId) as PurchaseBasket;
        }
    }
 
}