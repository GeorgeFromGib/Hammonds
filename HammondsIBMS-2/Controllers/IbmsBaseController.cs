using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Helpers.Html;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.StockInvoice;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Supplier;

namespace HammondsIBMS_2.Controllers
{
    public delegate ActionResult ReturnAction();

    public class IbmsBaseController : Controller
    {

        protected bool ExecuteRepositoryAction(Action dataFunction)
        {
            try
            {
                dataFunction();
                return true;
            }
            catch (DataException de)
            {
                throw;
                //ModelState.AddModelError("", "Data error: " + de.Message);
            }
            catch (BusinessRuleException se)
            {
                ModelState.AddModelError(se.FieldName, se.Message);
            }
            return false;
        }



        protected ActionResult ExecuteRepositoryAction(Action dataFunction, ReturnAction onSuccess, ReturnAction onError = null)
        {
            try
            {
                dataFunction();
                if (onSuccess != null)
                    return onSuccess();
                throw new ApplicationException("Method ExecuteRepositoryAction : onSuccess method has not been defined");
            }
            catch (DataException de)
            {
                ModelState.AddModelError("", "Data error: " + de.Message + (de.InnerException == null ? " " : "Inner Exception : " + de.InnerException.Message));
            }


            if (Request.IsAjaxRequest())
                return AjaxErrorReturn();
            else
            {
                if (onError != null)
                    return onError();
                throw new ApplicationException("Method ExecuteRepositoryAction : onError Method has not been defined");
            }
        }

        protected ActionResult AjaxErrorReturn()
        {
            Response.StatusCode = 500;
            var cont = Content(String.Join(" ",
                (ModelState.Select(state => state))
                .SelectMany(s => s.Value.Errors)
                .Select(e => e.ErrorMessage).ToArray()));
            return cont;
        }

        public ActionResult ReturnJsonError(string error)
        {
            Response.StatusCode = 500;
            return Json(new { success = false, error });
        }

        protected ActionResult ReturnJsonFormSuccess()
        {
            return Json(new { success = true });
        }

        protected ActionResult ReturnJsonFormSuccess(int idxValue)
        {
            return Json(new { success = true,idx=idxValue });
        }

        public ActionResult StepperControl()
        {
            return PartialView("_StepperControl", Stepper2.StepperModel);
        }

        public ActionResult StepperHeader()
        {
            return PartialView("_StepperHeader", Stepper2.StepperModel);
        }

        public ActionResult StepperMoveToPreviousStep()
        {
            return RedirectToAction(Stepper2.PreviousStep());
        }


        protected void InitialiseSuppliersViewBag(ISupplierService svc)
        {
            ViewBag.Suppliers = svc.GetAllSuppliers().ToList();
        }

        protected void InitialiseExchangeRateViewBag(ISupplierService svc)
        {
            ViewBag.ExchangeRates =
                Mapper.Map<IEnumerable<ExchangeRate>, IEnumerable<ExchangeRateDropDownVM>>(svc.GetAllExchangeRates());
        }

        protected void InitialiseManufacturesViewBag(StockService svc)
        {
            ViewBag.Manufacturers = svc.GetManufacturers().ToList();
        }

        protected void InitialiseCategoriesViewBag(StockService svc)
        {
            ViewBag.Categories = svc.GetCategories().ToList();
        }

        protected void InitialiseContractTypeViewBag(ContractService svc,bool addNoneSelection=false)
        {
            var contractTypes = new List<ContractType>();
            if (addNoneSelection) contractTypes.Add(new ContractType { ContractTypeId = -1, Description = "---" });
            contractTypes.AddRange(svc.GetActiveContractTypes().ToList());
            
            ViewBag.ContractTypes = contractTypes;
        }

        protected void InitialisePaymentPeriodViewBag(ContractService svc)
        {
            ViewBag.PaymentPeriods = svc.GetPaymentPeriodTypes().ToList();
        }

        protected void InitialiseProductLifeCycleViewBag(StockService svc)
        {
            ViewBag.ProductLifeCycles=svc.GetProductLifeCycles().ToList();
        }

        protected void InitialisePaymentSourcesViewBag(TransactionsService svc)
        {
            ViewBag.PaymentSources = svc.GetPaymentSources().ToList();
        }

        protected void InitialisePaymentSourcesViewBag(AccountTransactionService svc)
        {
            ViewBag.PaymentSources = svc.GetPaymentSources().ToList();
        }

        protected void InitialiseContractChangeableProductLifeCycleViewBag(StockService svc)
        {
            ViewBag.ContractChangeableProductLifeCycle =
                svc.GetProductLifeCycles().Where(c => c.ContractChangeable).ToList();
        }

        protected void InitialiseEmployersViewBag(EmployerService svc)
        {
            ViewBag.Employers = svc.GetAllEmployers().ToList();
        }

        

        protected List<T> AddAllSelector<T>(T model,dynamic source)
        {
            var lst = new List<T>{model};
            lst.AddRange(source);
            return lst;
        }
    }
}


