using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.ModelView;
using HammondsIBMS_2.ViewModels.StockInvoice;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Values;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace HammondsIBMS_2.Controllers
{

    public class StockInvoiceController :IbmsBaseController
    {
        private IStockInvoiceService _stockInvSvc;
        private readonly ISupplierService _supSvc;
        private readonly MiscService _miscService;
        //private TempStockInvoiceRepository _tempRep;
        private StockInvoice _model;

        public StockInvoiceController(IStockInvoiceService svc,ISupplierService supSvc,MiscService miscService)
        {
            _stockInvSvc = svc;
            _supSvc = supSvc;
            _miscService = miscService;
            InitialiseSuppliersViewBag(supSvc);
            InitialiseExchangeRateViewBag(supSvc);

        }

        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    _tempRep = TempStockInvoiceRepository.GetRepository(this);
        //    _model = _tempRep.StockInvoice;
        //}
        //
        // GET: /StockInvoice/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult _StockInvoiceGridBind()
        {
            var vm = AutoMapperSetup.MapList<StockInvoice, StockInvoiceVM>(_stockInvSvc.GetAllInvoices());
            return View(new GridModel(vm));
        }


        public ActionResult CreateNewInvoice()
        {
            var newModel = new NewStockInvoiceVM{SupplierId = -1,InvoiceDate = DateTime.Today};
            var sup = ViewBag.Suppliers as List<Supplier>;
            sup.Insert(0, new Supplier{SupplierId = -1,Name = "None",PreferedExchangeRate =new ExchangeRate{RateToGBP = 0}});
            ViewBag.Suppliers = sup;

            return PartialView(newModel);

        }

        private static StockInvoiceVM MapStockInvoiceToEditViewModel(StockInvoice stockInvoice)
        {
            return Mapper.Map<StockInvoice, StockInvoiceVM>(stockInvoice);
        }

        [HttpPost]
        public ActionResult CreateNewInvoice(NewStockInvoiceVM mNewStockInvoiceVm)
        {
            if (ModelState.IsValid)
            {
                var sinv = new StockInvoice();
                if (TryUpdateModel(sinv))
                {
                    var pExRate=_supSvc.GetSupplier(sinv.SupplierId).PreferedExchangeRate;
                    sinv.ExchangeRate = pExRate;
                    sinv.ExchangeRateId = pExRate.ExchangeRateId;
                    sinv.InvoiceCurrency=new ForeignCurrency(pExRate);
                    if (ExecuteRepositoryAction(() => { _stockInvSvc.AddInvoice(sinv); _stockInvSvc.CommitChanges(); }))
                        return ReturnJsonFormSuccess(sinv.StockInvoiceid);
                }

            }
            return PartialView(mNewStockInvoiceVm);

        }

        public ActionResult Edit(int id)
        {
            return View(MapStockInvoiceToEditViewModel(_stockInvSvc.GetInvoice(id)));
        }

        public ActionResult _DisplayStockInvoiceDetails(int id)
        {
            var vm = MapStockInvoiceToEditViewModel(_stockInvSvc.GetInvoice(id));
            return PartialView(vm);
        }

        public ActionResult _DisplayStockInvoiceCharges(int id)
        {
            var vm=MapStockInvoiceToEditViewModel(_stockInvSvc.GetInvoice(id));
            return PartialView(vm);
        }

        public ActionResult _EditStockInvoiceCharges(int id)
        {
            var vm = Mapper.Map<StockInvoice, StockInvoiceChargesVM>(_stockInvSvc.GetInvoice(id));
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult _EditStockInvoiceCharges(StockInvoiceChargesVM mStockInvoiceChargesVm)
        {
            if (ModelState.IsValid)
            {
                var stk = _stockInvSvc.GetInvoice(mStockInvoiceChargesVm.StockInvoiceid);
                if (TryUpdateModel(stk))
                {
                    if (ExecuteRepositoryAction(() => { _stockInvSvc.UpdateInvoice(stk); _stockInvSvc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(mStockInvoiceChargesVm);
        }

        [GridAction]
        public ActionResult _StockItemGridBind(int id)
        {
            var si = _stockInvSvc.GetInvoice(id).Items;
            var vm = AutoMapperSetup.MapList<StockInvoiceItem, StockInvoiceItemVM>(si);
            return View(new GridModel(vm));
        }

        public ActionResult _EditStockInvoiceDetails(int id)
        {
            var vm = MapStockInvoiceToEditViewModel(_stockInvSvc.GetInvoice(id));
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult _EditStockInvoiceDetails(StockInvoiceVM mStockInvoiceVm)
        {
            if (ModelState.IsValid)
            {
                var stk = _stockInvSvc.GetInvoice(mStockInvoiceVm.StockInvoiceid);
                stk.ChangeExchangeRate(_supSvc.GetExchangeRate(mStockInvoiceVm.ExchangeRateId));
                if (TryUpdateModel(stk))
                {
                    
                    if (ExecuteRepositoryAction(() => { _stockInvSvc.UpdateInvoice(stk); _stockInvSvc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(mStockInvoiceVm);
        }

        public ActionResult _SelectAvailableStock(int id)
        {
            var selForInvVM = new SelectStockForInvoiceVM
            {
                StockInvoiceId = id,
                SelectedStock = AutoMapperSetup.MapList<Stock, SelectableStockVM>(_stockInvSvc.GetSelectableStock()).ToList()
            };
            Session["_availableForStock"] = selForInvVM.SelectedStock;
            return PartialView(selForInvVM);
        }

        [GridAction]
        public ActionResult _GetAvailableStock(int id)
        {
            var selectedStock = Session["_availableForStock"] as List<SelectableStockVM>;
            return View(new GridModel(selectedStock));
        }


        [HttpPost]
        public ActionResult _SelectAvailableStock(SelectStockForInvoiceVM mStockForInvoiceVm)
        {
            if (ModelState.IsValid)
            {
                var stkInv = _stockInvSvc.GetInvoice(mStockForInvoiceVm.StockInvoiceId);
                var defaultDuty = _miscService.GetDefaultDutyPercentage();
                foreach (var selectedStockVm in mStockForInvoiceVm.SelectedStock)
                {
                    var stk = _stockInvSvc.GetStock(selectedStockVm.StockId);
                    stkInv.AddStockToItems(stk, defaultDuty);
                }
                if (ExecuteRepositoryAction(() => { _stockInvSvc.UpdateInvoice(stkInv); _stockInvSvc.CommitChanges(); }))
                    return ReturnJsonFormSuccess();

            }
            return PartialView(mStockForInvoiceVm);
        }

        public ActionResult _EditInvoiceItem(int id)
        {
            var itm = _stockInvSvc.GetInvoiceItem(id);
            return PartialView(Mapper.Map<StockInvoiceItem,StockInvoiceItemVM>(itm));
        }

        [HttpPost]
        public ActionResult _EditInvoiceItem(StockInvoiceItemVM mStockInvoiceItemVm)
        {
            if (ModelState.IsValid)
            {
                var stkItm = _stockInvSvc.GetInvoiceItem(mStockInvoiceItemVm.StockInvoiceItemId);
                var inv = stkItm.Invoice;
                if (TryUpdateModel(stkItm))
                {
                    if (ExecuteRepositoryAction(() => { _stockInvSvc.UpdateInvoice(inv); _stockInvSvc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }

            }
            return PartialView(mStockInvoiceItemVm);
        }

        public ActionResult _EditAttachedStock(int id)
        {
            var stkInvItm = _stockInvSvc.GetInvoiceItem(id);
            var invAttStk = new StockInvoiceItemAttachedStockVM
            {
                StockInvoiceItemId = id,
                IsOpen = (stkInvItm.Invoice.DateProcessed==null),
                SelectableStock = AutoMapperSetup.MapList<Stock, SelectableStockVM>(stkInvItm.StockItems).ToList()
            };
            return PartialView(invAttStk);
        }

        [HttpPost]
        public ActionResult _EditAttachedStock(StockInvoiceItemAttachedStockVM mStockInvoiceItemAttachedStockVm)
        {
            if (ModelState.IsValid)
            {
                var stkInvItm = _stockInvSvc.GetInvoiceItem(mStockInvoiceItemAttachedStockVm.StockInvoiceItemId);
                var inv = stkInvItm.Invoice;
                foreach (var aSelStock in mStockInvoiceItemAttachedStockVm.SelectableStock.Where(m=>m.IsSelected))
                {
                    stkInvItm.RemoveStock(aSelStock.StockId);
                }
                if (ExecuteRepositoryAction(() => { _stockInvSvc.UpdateInvoice(inv); _stockInvSvc.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mStockInvoiceItemAttachedStockVm);
        }

        [OutputCache(Duration = 0)]
        public ActionResult _ChangeStockInvoiceItemCosts(StockInvoiceItemVM model)
        {
            var item = _stockInvSvc.GetInvoiceItem(model.StockInvoiceItemId);

            item.UnitCost = new ForeignCurrency(model.UnitCost.AmountNative, item.UnitCost);
            item.DutyPct = model.DutyPct;

            var totals = new
            {
                FreightApp = item.FreightApportionment.ToString("C2"),
                LandedCost = item.LandedCost.ToString("C2"),
                UnitCosts = string.Format("{0}{1:F} ({2:C2})", item.UnitCostTotal.Symbol, item.UnitCostTotal.AmountNative, item.UnitCostTotal.AmountGBP),
                RetailPrice = item.RetailPrice.ToString("C2")
            };

            return Json(totals, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _CloseStockInvoice(int id)
        {
            var vm = new CloseStockInvoiceVM 
            {
                StockInvoiceId = id,
                DeleteMessage = "Closing this invoice will update stock prices and landed Costs. You will not be able to edit this invoice after it has been closed.",
                Prompt = "Close Invoice?"
            };
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult _CloseStockInvoice(CloseStockInvoiceVM mCloseStockInvoiceVm)
        {
            if (ModelState.IsValid)
            {
                var inv = _stockInvSvc.GetInvoice(mCloseStockInvoiceVm.StockInvoiceId);
                inv.IsProcessed = true;
                if (ExecuteRepositoryAction(() => { _stockInvSvc.CloseInvoice(inv); _stockInvSvc.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mCloseStockInvoiceVm);
        }

        public ActionResult DeleteCancelInvoice(int id)
        {
            var promptVm = new PromptVM { RecordIndex = id };
            var stockInvoice = _stockInvSvc.GetInvoice(id);
            if(stockInvoice.DateProcessed!=null)
            {
                promptVm.AddPrompt(new DialogPrompt("This Invoice is Processed or Cancelled and no further action is allowed!", PromptMessageAlertLevel.Info));
                promptVm.Buttons=PromptVM.DialogButtons.Cancel;
            }
            else if (stockInvoice.Items.Count>0)
            {
                promptVm.AddPrompt(new DialogPrompt("Invoice has active stock entries and can only be cancelled!", PromptMessageAlertLevel.Danger));
                promptVm.AddPrompt(new DialogPrompt("Do you wish to Cancel?", PromptMessageAlertLevel.Warning));
            }
            else
            {
                promptVm.AddPrompt(new DialogPrompt("Do you wish to delete Invoice with reference: " + stockInvoice.InvoiceRef + " ?", PromptMessageAlertLevel.Warning));
            }
            return PartialView("_PromptDialog", promptVm);
        }

        [HttpPost]
        public ActionResult DeleteCancelInvoice(PromptVM promptVm)
        {
            var stkInv = _stockInvSvc.GetInvoice(promptVm.RecordIndex);
            if (ExecuteRepositoryAction(() =>
                {
                    if (stkInv.Items.Count > 0)
                    {
                        _stockInvSvc.CancelInvoice(stkInv);
                    }
                    else
                    {
                        _stockInvSvc.DeleteInvoice(stkInv);
                    }
                    _stockInvSvc.CommitChanges();
                }))
                return ReturnJsonFormSuccess();

            return PartialView("_PromptDialog",promptVm);
        }
        
    }
}
