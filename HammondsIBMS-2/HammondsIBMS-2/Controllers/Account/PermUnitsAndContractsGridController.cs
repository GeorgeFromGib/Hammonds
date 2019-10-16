using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.Controllers
{
    public class PermUnitsAndContractsGridController : UnitsAndContractsGridBaseController
    {
        public PermUnitsAndContractsGridController(ContractService contractService, CustomerAccountService accountService)
            : base(DependencyResolver.Current.GetService<PermPurchaseUnitAndContracts>(), contractService, accountService)
        {
        }

        public override ActionResult _DeletePurchaseUnit(int id)
        {
            var ru = _purchaseUnitAndContracts.GetPurchaseUnit(id);

            var prompt = new DeletPurchaseUnitPromptVM()
            {
                RecordIndex = id,
                Model = ru.Stock.ManufacturerModel,
                Amount = ru.Total,
                PurchaseDate = ru.PurchasedDate,
                ProductLifeCycleId = ru.Stock.ProductLifeCycleId,
                Buttons = PromptVM.DialogButtons.All
            };
            LoadViewBagWithChangeableProductLifeCyclesForAccount(ru);
            return PartialView("_DeletePurchaseUnit", prompt);
        }

        private void LoadViewBagWithChangeableProductLifeCyclesForAccount(PurchaseUnit ru)
        {
            ViewBag.ChanageableProductLifeCycles =
                _accountService.GetChangeableProductLifeCylesForAccount(ru.CustomerAccountId)
                    .Select(c => new {c.ProductLifeCycle.ProductLifeCycleId, c.ProductLifeCycle.Description});
        }

        [HttpPost]
        public override ActionResult _DeletePurchaseUnit(DeletPurchaseUnitPromptVM mPromptVm)
        {
            var pu = _purchaseUnitAndContracts.GetPurchaseUnit(mPromptVm.RecordIndex);
            var hasUnexpiredPaidContracts = pu.Contracts.Any(c => c.ExpiryDate >= mPromptVm.RemovalDate && c.Charge > 0);
            if (!hasUnexpiredPaidContracts)
            {
                if (TryUpdateModel(pu))
                {
                    if (ExecuteRepositoryAction(() =>
                    {
                        _accountService.DeletePurchaseUnit(pu, mPromptVm.ProductLifeCycleId, mPromptVm.RefundAmount);
                        _accountService.CommitChanges();
                    }))
                        return ReturnJsonFormSuccess();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unit cannot be removed until un-expired paid service contracts are removed.");
            }
            LoadViewBagWithChangeableProductLifeCyclesForAccount(pu);
            return PartialView("_DeletePurchaseUnit",mPromptVm);
        }


        public override ActionResult _DeletePurchaseUnitContract(int id)
        {
            var contract = _purchaseUnitAndContracts.GetPurchaseUnitContract(id);
            var prompt = new DeletePurchaseContractPromptVM()
            {
                RecordIndex = contract.ContractId,
                StartDate = contract.StartDate,
                ExpiryDate = contract.StartDate>DateTime.Today? contract.StartDate: DateTime.Today,
                Amount = contract.PeriodPaymentAmount,
                ProRateRefunded = true,
                Buttons = PromptVM.DialogButtons.All,
                Message = contract.ContractType.Description,
                SheduledExpiryDate = contract.ExpiryDate??contract.StartDate,
            };

            return PartialView(prompt);

        }

        [HttpPost]
        public override ActionResult _DeletePurchaseUnitContract(DeletePurchaseContractPromptVM mPromptVm)
        {
            var sc = _purchaseUnitAndContracts.GetPurchaseUnitContract(mPromptVm.RecordIndex);
            if (TryUpdateModel(sc))
            {
                if (ExecuteRepositoryAction(() =>
                {
                    _accountService.ExpireServiceContract(sc);
                    _accountService.CommitChanges();
                }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mPromptVm);
        }

        protected override IEnumerable<ContractType> GetAvailableContracts(PurchaseUnit unit)
        {
            return _contractService.GetContractsForModel(unit.Stock.ModelId).ToList();
        }
    }

   
}