using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.ViewModels.Invoice;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Invoicing;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class InvoiceController : IbmsBaseController
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        //
        // GET: /Invoice/

        public ActionResult Index()
        {
            var invoices =_invoiceService.GetAll();
            return View(invoices);
        }

        public ActionResult InvoiceRun()
        {
            var dte = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var invVM = new InvoiceVM { InvoiceDate = dte, BillCycle = new BillCycle(dte) };
            return View(invVM);
        }

        [HttpPost]
        public ActionResult InvoiceRun(InvoiceVM invoiceVm)
        {
           // _invoiceService.ExecuteInvoicingRun(invoiceVm.BillCycle);
            _invoiceService.CommitChanges();
            return View(invoiceVm);
        }

        public ActionResult _DisplayCustomerInvoices(int id)
        {
            ViewBag.CustomerId = id;
            return PartialView();
        }

        private IEnumerable<CustomerInvoiceVM> GetCustomerInvoiceVM(int id)
        {
            var customerInvoices = _invoiceService.GetInvoicesForCustomer(id);
            var customerInvoiceVM = Mapper.Map<IEnumerable<Invoice>, IEnumerable<CustomerInvoiceVM>>(customerInvoices);
            return customerInvoiceVM;
        }

        [GridAction]
        public ActionResult _CustomerInvoicesGridBind(int id)
        {
            ViewBag.CustomerId = id;
            return View(new GridModel(GetCustomerInvoiceVM(id)));
        }

        public ActionResult _DisplayInvoiceDetails(int id)
        {
            var inv = _invoiceService.GetInvoice(id);
            return PartialView(inv);

        }

        public ActionResult _DisplayContractInvoiceItem(int id)
        {
            var invitms = _invoiceService.GetInvoiceItem(id);
            return PartialView(invitms);

        }

        #region Posts
        
        public ActionResult _DisplayPostedForCustomer(int id)
        {
            ViewBag.CustomerId = id;
            return PartialView(PostedForCustomerToPostedVMList(id));
        }

        private IEnumerable<PostedVM> PostedForCustomerToPostedVMList(int id, bool showInvoiced = false)
        {
            var pred = showInvoiced ? new Func<AdjustmentPost, bool>(c => true) : (c => c.InvoiceId == null);
            var pl = _invoiceService.GetPostedByCustomer(id).Where(pred);
            var vm = pl.Select(c => Mapper.Map(c, c.GetType(), typeof (PostedVM)) as PostedVM);
            return vm;
        }

        [GridAction]
        public ActionResult _CustomerPostedGridBind(int cusId,bool? showInvoiced)
        {
            ViewBag.CustomerId = cusId;
            var vml = PostedForCustomerToPostedVMList(cusId, showInvoiced ?? false);
            return View(new GridModel(vml));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeletePosted(int id, bool? showInvoiced,int cusId)
        {
            var post = _invoiceService.GetPostedById(id);
            if (post != null)
            {
                ExecuteRepositoryAction(() =>
                                            {
                                                _invoiceService.DeletePosted(post);
                                                _invoiceService.CommitChanges();
                                            });
            }

            return View(new GridModel(PostedForCustomerToPostedVMList(cusId, showInvoiced ?? false)));
        }

        #endregion

    }
}
