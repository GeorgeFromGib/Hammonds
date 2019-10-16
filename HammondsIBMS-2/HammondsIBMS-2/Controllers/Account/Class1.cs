using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Contracts;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public abstract class UnitsAndContractsGridBaseController: IbmsBaseController, IUnitsAndContractsGridController
    {
        private readonly IPurchaseUnitAndContractsDataSource _purchaseUnitAndContracts;
        private readonly ContractService _contractService;
        private readonly CustomerAccountService _accountService;

        protected UnitsAndContractsGridBaseController(IPurchaseUnitAndContractsDataSource purchaseUnitAndContracts, ContractService contractService, CustomerAccountService accountService)
        {
            _purchaseUnitAndContracts = purchaseUnitAndContracts;
            _contractService = contractService;
            _accountService = accountService;
        }

        [GridAction]
        public ActionResult _DisplayPurchasedUnits(int accountId)
        {
            var purchaseUnits = _purchaseUnitAndContracts.GetPurchaseUnits(accountId);
            return View(new GridModel(AutoMapperSetup.MapList<PurchaseUnit, PurchasedUnitVM>(purchaseUnits)));
        }

        public ActionResult _EditPurchaseUnit(int id)
        {
            var pu = _purchaseUnitAndContracts.GetPurchaseUnit(id);
            return PartialView("../Account/_EditPurchaseUnit", Mapper.Map<PurchaseUnit, EditPurchaseUnitVM>(pu));
        }

        [HttpPost]
        public ActionResult _EditPurchaseUnit(EditPurchaseUnitVM mPurchaseUnit)
        {
            var pu = _purchaseUnitAndContracts.GetPurchaseUnit(mPurchaseUnit.UnitId);
            if (ModelState.IsValid)
            {
                pu.PurchasedDate = mPurchaseUnit.PurchasedDate;
                pu.DiscountedAmount = mPurchaseUnit.DiscountedAmount;
                if (_purchaseUnitAndContracts.UpdatePurchaseUnit(pu))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("../Account/_EditPurchaseUnit", mPurchaseUnit);
        }

        public ActionResult _DeletePurchaseUnit(int id)
        {
            var ru = _purchaseUnitAndContracts.GetPurchaseUnit(id);
            var prompt = new PromptVM(id, new DialogPrompt(string.Format("Remove model {0} from selection?", ru.Stock.ManufacturerModel)));
            return PartialView("_PromptDialog", prompt);
        }

        [HttpPost]
        public ActionResult _DeletePurchaseUnit(PromptVM mPromptVm)
        {
            _purchaseUnitAndContracts.DeletePurchaseUnit(mPromptVm.RecordIndex);
            return ReturnJsonFormSuccess();
        }

        public ActionResult _DeletePurchaseUnitContract(int id)
        {
            var contract = _purchaseUnitAndContracts.GetPurchaseUnitContract(id);
            var promptVm = new PromptVM { RecordIndex = id };
            promptVm.AddPrompt(new DialogPrompt("Do you wish to remove contract '" + contract.ContractType.Description + "' from this unit", PromptMessageAlertLevel.Warning));
            return PartialView("_PromptDialog", promptVm);
        }

        [GridAction]
        public ActionResult _DisplayPurchaseUnitContracts(int unitId)
        {
            var uc = _purchaseUnitAndContracts.GetPurchaseUnitContracts(unitId);
            return View(new GridModel(AutoMapperSetup.MapList<ServiceContract, ServiceContractVM>(uc)));
        }


        [HttpPost]
        public ActionResult _DeletePurchaseUnitContract(PromptVM mPromptVm)
        {
            _purchaseUnitAndContracts.DeletePurchaseUnitContract(mPromptVm.RecordIndex);
            return ReturnJsonFormSuccess();
        }

        public ActionResult _AddUnitContracts(int id)
        {
            var unit = _purchaseUnitAndContracts.GetPurchaseUnit(id);
            var availableContracts = GetAvailableContracts(unit);
            var purchaseDate = unit.PurchasedDate;
            var minDate = unit.PurchasedDate;
            var vm = BuildSelectContractTypesVM(id, availableContracts, purchaseDate, minDate);
            return PartialView("../Account/_SelectServiceContractsForModel", vm);
        }

        [HttpPost]
        public ActionResult _AddUnitContracts(SelectContractTypesVM mSelectContractTypeVm)
        {
            var unit =
                    _purchaseUnitAndContracts.GetPurchaseUnit(mSelectContractTypeVm.UnitId);

            if (ModelState.IsValid)
            {
                if (mSelectContractTypeVm.ContractTypes != null)
                    foreach (var selectedContract in mSelectContractTypeVm.ContractTypes)
                    {
                        var contractType = _contractService.GetContractType(selectedContract.ContractTypeId);
                        var svcContract =
                            CreateServiceContractFromContract(unit.Contracts.NextIndex(c => c.ContractId),
                                mSelectContractTypeVm.PurchaseDate, contractType);
                        unit.Contracts.Add(svcContract);

                    }
                if (_purchaseUnitAndContracts.UpdatePurchaseUnit(unit))
                    return ReturnJsonFormSuccess();
            }
            mSelectContractTypeVm.ContractTypes = GetContractTypesVM(GetAvailableContracts(unit));
            return PartialView("../Account/_SelectServiceContractsForModel", mSelectContractTypeVm);
        }

        private IEnumerable<ContractType> GetAvailableContracts(PurchaseUnit unit)
        {
            return _contractService.GetContractsForModel(unit.Stock.ModelId).Where(
                b => unit.Contracts.All(c => c.ContractTypeId != b.ContractTypeId)).ToList();
        }

        private ServiceContract CreateServiceContractFromContract(int recordIndex, DateTime startDate, ContractType contract)
        {
            var serviceContract = new ServiceContract
            {
                ContractId = recordIndex,
                ContractTypeId = contract.ContractTypeId,
                ContractLengthInMonths = contract.PeriodInMonths,
                PaymentPeriod = _accountService.GetPaymentPeriod(contract.PaymentPeriodId),
                PaymentPeriodId = contract.PaymentPeriodId,
                Charge = contract.NormalTermAmount,
                ContractType = contract,
            };
            serviceContract.SetStartDate(startDate);
            return serviceContract;
        }

        private SelectContractTypesVM BuildSelectContractTypesVM(int unitId, IEnumerable<ContractType> avilableContracts,
            DateTime purchaseDate, DateTime minDate)
        {
            var vm = new SelectContractTypesVM { UnitId = unitId, ContractTypes = new List<ContractTypeVM>(), PurchaseDate = purchaseDate, MinDate = minDate };
            vm.ContractTypes = GetContractTypesVM(avilableContracts);
            return vm;
        }

        private static List<ContractTypeVM> GetContractTypesVM(IEnumerable<ContractType> avilableContracts)
        {
            return AutoMapperSetup.MapList<ContractType, ContractTypeVM>(avilableContracts).ToList();
        }
    }
}