using System;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class RentalAccountVM:AccountTypeVM
    {
        private readonly RentalAccount _rentalAccount;

        public RentalAccountVM(RentalAccount account)
        {
            _rentalAccount = account;
        }

        public override ReturnView DisplayAttachedContractsView()
        {
            return new ReturnView("_DisplayRentalAccountContracts", _rentalAccount);
        }

        public override ReturnView DisplayForCreateUnitOptions(CreateContractUnitVM createContractUnitVm)
        {
            return new ReturnView("_CreateRentalAccountOptions", createContractUnitVm);
        }

        public override void AddToRepository(ContractService contractservice)
        {
            throw new NotImplementedException();
            //contractservice.AddContractUnit(_rentalAccount);
        }

        public override ReturnView DisplayContracUnitEditBody()
        {
            return new ReturnView("_RentalAccountEditBody",_rentalAccount);
        }
    }
}