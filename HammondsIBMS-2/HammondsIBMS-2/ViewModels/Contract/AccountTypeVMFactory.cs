using System;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using PurchaseAccount = HammondsIBMS_Domain.Entities.Accounts.PurchaseAccount;


namespace HammondsIBMS_2.ViewModels.Contract
{
    public static class AccountTypeVMFactory
    {
        public static AccountTypeVM CreateAccountTypeVM(CustomerAccount customerAccount)
        {
            if(customerAccount is PurchaseAccount)
                return new PurchaseAccountVM((PurchaseAccount)customerAccount);

            if (customerAccount is RentalAccount)
                return new RentalAccountVM((RentalAccount)customerAccount);

            throw new ApplicationException("Unknown CustomerAccount Type passed");
        }

        public static AccountTypeVM.ReturnView GetCreateContractUnitOptionsVM(CreateContractUnitVM createContractUnitVm)
        {
            switch (createContractUnitVm.ContractUnitType)
            {
                case AccountType.Purchase:
                    return new PurchaseAccountVM(null).DisplayForCreateUnitOptions(createContractUnitVm);
                case AccountType.Rent:
                    return new RentalAccountVM(null).DisplayForCreateUnitOptions(createContractUnitVm);
                default:
                    throw new ApplicationException("Unknown CustomerAccount Type passed");
            }
        }
    }
}