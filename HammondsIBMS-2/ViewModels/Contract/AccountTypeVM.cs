using HammondsIBMS_Application;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public abstract class  AccountTypeVM
    {

        public struct ReturnView
        {
            public string View;
            public object Model;

            public ReturnView(string view,object model)
            {
                View = view;
                Model = model;
            }
        }

        public abstract ReturnView DisplayAttachedContractsView();
        public abstract ReturnView DisplayForCreateUnitOptions(CreateContractUnitVM createContractUnitVm);
        public abstract void AddToRepository(ContractService contractservice);
        public abstract ReturnView DisplayContracUnitEditBody();
    }
}