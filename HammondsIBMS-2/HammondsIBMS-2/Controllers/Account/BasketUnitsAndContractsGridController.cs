using System.Web.Mvc;
using HammondsIBMS_2.Controllers.Account;
using HammondsIBMS_Application;

namespace HammondsIBMS_2.Controllers
{
    public class BasketUnitsAndContractsGridController : UnitsAndContractsGridBaseController
    {
        public BasketUnitsAndContractsGridController(ContractService contractService, CustomerAccountService accountService)
            : base(DependencyResolver.Current.GetService<BasketPurchaseUnitAndContracts>(), contractService, accountService)
        {
        }
    }
}