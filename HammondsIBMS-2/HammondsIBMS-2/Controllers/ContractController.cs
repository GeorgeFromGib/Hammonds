using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace HammondsIBMS_2.Controllers
{

    public class ContractController : IbmsBaseController
    {
        private readonly ContractService _contractService;
        private readonly CustomerService _customerService;


        public ContractController(ContractService contractService,CustomerService customerService,StockService stockService)
        {
            _contractService = contractService;
            _customerService = customerService;
            InitialiseContractTypeViewBag(_contractService);
            InitialisePaymentPeriodViewBag(_contractService);
            //InitialiseContractChangeableProductLifeCycleViewBag(stockService);
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Contract Types

        public ActionResult ContractTypes()
        {
            return View();
        }

        [GridAction]
        public ActionResult ContractTypesGridBind()
        {
            var conTypes=_contractService.GetContractTypes();
            return View(new GridModel(conTypes));
        }

        public ActionResult _EditContractType(int id)
        {
            return PartialView(_contractService.GetContractType(id));
        }

        [HttpPost]
        public ActionResult _EditContractType(ContractType mContractType)
        {
            var contractType = _contractService.GetContractType(mContractType.ContractTypeId);
            if (TryUpdateModel(contractType))
            {
                if(ExecuteRepositoryAction(()=>{
                    _contractService.UpdateContractType(contractType);
                    _contractService.CommitChanges();}))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mContractType);
        }

        public ActionResult _AddContractType()
        {
            var ct = new ContractType{PaymentPeriodId = 4};
            return PartialView("_EditContractType", ct);
        }

        [HttpPost]
        public ActionResult _AddContractType(ContractType mContractType)
        {
            if (TryValidateModel(mContractType))
            {
                if (ExecuteRepositoryAction(() => { _contractService.AddContractType(mContractType); _contractService.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_EditContractType",mContractType);
        }

        #endregion

       
    }
}
