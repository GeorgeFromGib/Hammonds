using System.Collections.Generic;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.Controllers
{
    public interface IPurchaseUnitAndContractsDataSource
    {
        List<PurchaseUnit> GetPurchaseUnits(int accountId);
        List<ServiceContract> GetPurchaseUnitContracts(int unitId);
        PurchaseUnit GetPurchaseUnit(int id);
        bool UpdatePurchaseUnit(PurchaseUnit pu);
        bool DeletePurchaseUnit(int unitId);
        ServiceContract GetPurchaseUnitContract(int contractId);
        bool DeletePurchaseUnitContract(int contractId);
        bool UpdatePurchaseUnitContract(ServiceContract serviceContract);
        void AddContractToPurchaseUnit(PurchaseUnit purhaseUnit, ServiceContract serviceContract);
    }
}