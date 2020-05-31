﻿using System.Web;
using System.Web.Mvc;
using HammondsIBMS_Application;
using HammondsIBMS_Data.Repositories;


namespace HammondsIBMS_2.Controllers
{
    public class TempUnitsAndContractsGridController : UnitsAndContractsGridBaseController
    {
        public TempUnitsAndContractsGridController( ContractService contractService, CustomerAccountService accountService) : base(DependencyResolver.Current.GetService<TempPurchaseUnitAndContracts>(), contractService, accountService)
        {
        }
    }
}
