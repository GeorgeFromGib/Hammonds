using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Payment;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Payments;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class PaymentController : IbmsBaseController
    {
        private readonly PaymentService _paymentService;
        private readonly CustomerService _customerService;
        private readonly TransactionsService _transactionsService;

        public PaymentController(PaymentService paymentService,CustomerService customerService,TransactionsService transactionsService)
        {
            _paymentService = paymentService;
            _customerService = customerService;
            _transactionsService = transactionsService;
            InitialisePaymentSourcesViewBag(transactionsService);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _MakePayment(int id)
        {
            var vm = new MakePaymentVM();

            var cust = _customerService.GetCustomer(id);
            if (!DoesCustomerHaveAccounts(id))
            {
                vm.IsError = true;
                vm.ErrorMessage = "Payments can only be collected on Customers with existing Accounts";
            }
            else
            {
                var rolledTransactions =
                    _paymentService.GetRolledTransactions(id).Where(s => s.Amount != 0).OrderBy(s => s.BillCycle.Date);
                vm = new MakePaymentVM
                             {
                                 CustomerId = id,
                                 PreferredPaymentSourceId = cust.PreferredPaymentSourceId,
                                 Balance = _transactionsService.GetBalanceForCustomer(id),
                                 RolledTransactions =
                                     AutoMapperSetup.MapList<PaymentService.RolledTransaction, RolledTransactionVM>(
                                         rolledTransactions).ToList(),
                                 CreditedAccountId = cust.CustomerAccounts.Min(c => c.CustomerAccountId),
                                 CustomerAccounts = cust.CustomerAccounts
                             };
            }
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult _MakePayment(MakePaymentVM mVm)
        {
            if (TryValidateModel(mVm))
            {
                var pymnt = new Payment
                                {
                                    CustomerId = mVm.CustomerId,
                                    PaymentDate = DateTime.Today,
                                    PaymentSourceId = mVm.PreferredPaymentSourceId,
                                    Amount = mVm.TenderedAmount,
                                    Reference = mVm.Reference,
                                    PaymentItems = new List<PaymentItem>() 
                                };

                if (mVm.RolledTransactions!=null)
                {
                    foreach (var rolldTransVm in mVm.RolledTransactions.Where(p => p.Payment != 0))
                    {
                        var pmtItm = new PaymentItem
                                         {
                                             Amount = rolldTransVm.Payment,
                                             BillCycle = new BillCycle(rolldTransVm.BillCycleNo),
                                             PaymentItemTypeId = (int)PaymentItemTypes.InvoiceSettlement,
                                             CustomerAccountId = rolldTransVm.CustomerAccountId
                                         };
                        pymnt.PaymentItems.Add(pmtItm);
                    } 
                }

                if (mVm.CreditedAmount != 0)
                {
                    var crdPmt = new PaymentItem
                                     {
                                         CustomerAccountId = mVm.CreditedAccountId,
                                         BillCycle = new BillCycle(DateTime.Today).AddCycles(1),
                                         PaymentItemTypeId = (int) PaymentItemTypes.Credit,
                                         Amount = mVm.CreditedAmount
                                     };
                    pymnt.PaymentItems.Add(crdPmt);
                }
                
                if(ExecuteRepositoryAction(()=>
                                               {
                                                   _paymentService.AddPayment(pymnt);
                                                   _paymentService.CommitChanges();
                                               }))
                    return ReturnJsonFormSuccess();
            }
            mVm.CustomerAccounts = _customerService.GetCustomer(mVm.CustomerId).CustomerAccounts;
            return PartialView(mVm);
        }

        public ActionResult _DisplayCustomerPayments(int id)
        {
            ViewBag.CustomerId = id;
            return PartialView();
        }

        [GridAction]
        public ActionResult _CustomerAccountPaymentsGridBind(int id)
        {
            ViewBag.CustomerId = id;
            var pymnts = _paymentService.GetPaymentsForCustomer(id).OrderByDescending(o=>o.PaymentDate);
            return View(new GridModel(AutoMapperSetup.MapList<Payment, PaymentVM>(pymnts).ToList()));
        }

        [GridAction]
        public ActionResult _PaymentsItemsGridBind(int id)
        {
            var pItms = _paymentService.GetPaymentItemsForPayment(id);
            var vm = pItms.Select(c => Mapper.Map(c, c.GetType(), typeof (PaymentItemVM)));
            return View(new GridModel(vm));
        }

        public ActionResult _GetCustomerAccounts(int id)
        {
            var ca = _customerService.GetCustomer(id).CustomerAccounts;
            return new JsonResult { Data = new SelectList(ca.ToList(), "CustomerAccountId", "ContractUnitDescription") };
        }


        private bool DoesCustomerHaveAccounts(int id)
        {
            return (_customerService.GetCustomer(id).CustomerAccounts.Count > 0);
        }
    }
}
