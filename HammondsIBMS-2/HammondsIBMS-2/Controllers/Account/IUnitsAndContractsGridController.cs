using System.Web.Mvc;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public interface IUnitsAndContractsGridController
    {
        [GridAction]
        ActionResult _DisplayPurchasedUnits(int accountId);

        ActionResult _EditPurchaseUnit(int id);

        [HttpPost]
        ActionResult _EditPurchaseUnit(EditPurchaseUnitVM mPurchaseUnit);

        ActionResult _DeletePurchaseUnit(int id);

        [HttpPost]
        ActionResult _DeletePurchaseUnit(DeletPurchaseUnitPromptVM mPromptVm);

      
        ActionResult _DeletePurchaseUnitContract(int id);

        [HttpPost]
        ActionResult _DeletePurchaseUnitContract(DeletePurchaseContractPromptVM mPromptVm);


        ActionResult _AddUnitContracts(int id);

        [HttpPost]
        ActionResult _AddUnitContracts(SelectContractTypesVM mSelectContractTypeVm);


        [GridAction]
        ActionResult _DisplayPurchaseUnitContracts(int unitId,bool showExpired=false);
    }
}