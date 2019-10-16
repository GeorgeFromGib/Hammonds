using HammondsIBMS_2.Setup;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class PurchaseAccountVM:AccountTypeVM
    {
        private readonly PurchaseAccount _contractUnit;

        public PurchaseAccountVM(PurchaseAccount contractUnit) 
        {
            _contractUnit = contractUnit;
        }

        public override ReturnView DisplayAttachedContractsView()
        {

            return new ReturnView("_DisplayPurchaseAccountContracts", AutoMapperSetup.MapList<ServiceContract, ServiceContractVM>(_contractUnit.AttachedContracts.ConvertAll(c => (ServiceContract)c)));
        }


        public override ReturnView DisplayForCreateUnitOptions(CreateContractUnitVM createContractUnitVm)
        {
            return new ReturnView("_CreatePurchaseAccountOptions", createContractUnitVm);
        }


        public override void AddToRepository(ContractService contractservice)
        {
            contractservice.AddContractUnit(_contractUnit);
        }

        public override ReturnView DisplayContracUnitEditBody()
        {
            return new ReturnView("_PurchaseAccountEditBody", _contractUnit);
        }
    }
}