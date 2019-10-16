using System;
using System.Collections.Generic;
using System.Linq;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.Controllers
{
    public class TempPurchaseUnitAndContracts : IPurchaseUnitAndContractsDataSource
    {
        public List<PurchaseUnit> GetPurchaseUnits(int accountId)
        {
            List<PurchaseUnit> purchaseUnits = TempAccountAsAddPurchaseAccountVM().PurchasedUnits;
            return purchaseUnits;
        }

        public List<ServiceContract> GetPurchaseUnitContracts(int unitId)
        {
            return TempAccountAsAddPurchaseAccountVM().PurchasedUnits.Single(p => p.UnitId == unitId).Contracts;
        }

        public PurchaseUnit GetPurchaseUnit(int id)
        {
            return TempAccountAsAddPurchaseAccountVM().PurchasedUnits.Single(p => p.UnitId == id);
        }

        public bool UpdatePurchaseUnit(PurchaseUnit pu)
        {
            return true;
        }

        public bool DeletePurchaseUnit(int unitId)
        {
            PurchaseUnit pu = GetPurchaseUnit(unitId);
            TempAccountAsAddPurchaseAccountVM().PurchasedUnits.Remove(pu);
            return true;
        }


        public ServiceContract GetPurchaseUnitContract(int contractId)
        {
            return GetTempPurchaseAccountUnitContract(contractId);
        }

        public bool DeletePurchaseUnitContract(int contractId)
        {
            Tuple<ServiceContract, PurchaseUnit> tempContratTuple = GetTempPurchaseAccountUnitContractTuple(contractId);
            ServiceContract contract = tempContratTuple.Item1;
            PurchaseUnit unit = tempContratTuple.Item2;
            unit.Contracts.Remove(contract);
            return true;
        }

        public bool UpdatePurchaseUnitContract(ServiceContract serviceContract)
        {
            return true;
        }

        public void AddContractToPurchaseUnit(PurchaseUnit purhaseUnit, ServiceContract serviceContract)
        {
            purhaseUnit.Contracts.Add(serviceContract);
        }

        private AddPurchaseAccountVM TempAccountAsAddPurchaseAccountVM()
        {
            return TempCustomerAccount.TempAccount as AddPurchaseAccountVM;
        }

        private Tuple<ServiceContract, PurchaseUnit> GetTempPurchaseAccountUnitContractTuple(int id)
        {
            ServiceContract contract = null;
            PurchaseUnit holdingUnit = null;
            foreach (PurchaseUnit unit in TempAccountAsAddPurchaseAccountVM().PurchasedUnits)
            {
                if (unit.Contracts.Any(c => c.ContractId == id))
                {
                    contract = unit.Contracts.Single(c => c.ContractId == id);
                    holdingUnit = unit;
                    break;
                }
            }
            return new Tuple<ServiceContract, PurchaseUnit>(contract, holdingUnit);
        }

        private ServiceContract GetTempPurchaseAccountUnitContract(int id)
        {
            return GetTempPurchaseAccountUnitContractTuple(id).Item1;
        }
    }
}