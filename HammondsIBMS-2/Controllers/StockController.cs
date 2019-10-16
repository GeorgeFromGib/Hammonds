using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.ViewModels.ModelView;
using HammondsIBMS_2.ViewModels.StockView;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class StockController : IbmsBaseController
    {
        private StockService _svc;


        public StockController(StockService srv)
        {
            _svc = srv;
            InitialiseManufacturesViewBag(srv);
            InitialiseProductLifeCycleViewBag(srv);
           
        }

        public ActionResult Index()
        {

            return View();
        }

        [GridAction]
        public ActionResult _StockGridBind()
        {
            var mdl = ModelFilter == null ? _svc.GetStocks() : _svc.GetStocks().Where(m => (ModelFilter.ModelCode==null || m.Model.ModelCode.ToUpper().StartsWith(ModelFilter.ModelCode.ToUpper())) &&
                                                                                                   (ModelFilter.HTVIdentifier==null || m.Identifier.ToUpper().StartsWith(ModelFilter.HTVIdentifier.ToUpper())) &&
                                                                                                   (ModelFilter.SerialNumber==null || m.SerialNumber.ToUpper().StartsWith(ModelFilter.SerialNumber.ToUpper())) &&
                                                                                                   (ModelFilter.IncludeinStockOnly == false || m.ProductLifeCycleId == (int)ProductLifeCycleStatus.InStock));
            return View(new GridModel(GetStockListVM(mdl)));
        }

        private IEnumerable<StockVM> GetStockListVM(IEnumerable<Stock> stocks)
        {
            return Mapper.Map<IEnumerable<Stock>,IEnumerable<StockVM>>(stocks);
        }

        #region stock filter

        public ActionResult _StockFilter()
        {
            return PartialView(ModelFilter);
        }

        [HttpPost]
        public ActionResult _StockFilter(StockFilterVM mStockFilterVm)
        {
            ModelFilter = mStockFilterVm;
            ModelFilter.IsFiltered = true;
            return ReturnJsonFormSuccess();
        }

        public ActionResult _ResetStockFilter()
        {
            ModelFilter = ResetStockFilterVM();
            return PartialView("_StockFilter", ModelFilter);
        }

        private StockFilterVM ResetStockFilterVM()
        {
            return new StockFilterVM { ModelCode = null,HTVIdentifier = null,IncludeinStockOnly = true,SerialNumber = null, IsFiltered = false };
        }

        private StockFilterVM ModelFilter
        {
            get
            {
                var d= Session["_stockFilter"] as StockFilterVM;
                return d ?? ResetStockFilterVM();
            }
            set { Session["_stockFilter"] = value; }
        }

        #endregion

        #region Add & Edit

        public ActionResult _Edit(int id)
        {
            var stock = _svc.GetStock(id);
            var mdl = Mapper.Map<Stock,StockEditVM>(stock);
            PopulateProductLifeCycles(mdl, stock);
            return PartialView(mdl);
        }

        private void PopulateProductLifeCycles(StockEditVM mdl, Stock stock)
        {
            if (!stock.ProductLifeCycleStatus.NotAttachedStatus)
                mdl.ProductLifeCycles = new List<ProductLifeCycle>() {stock.ProductLifeCycleStatus};
            else
                mdl.ProductLifeCycles = _svc.GetProductLifeCycles().Where(m => m.NotAttachedStatus).ToList();

        }

        [HttpPost]
        public ActionResult _Edit(StockEditVM mStock)
        {
            if (ModelState.IsValid)
            {
                var stk = _svc.GetStock(mStock.StockId);
                if (TryUpdateModel(stk))
                {
                    if (ExecuteRepositoryAction(() => { _svc.UpdateStock(stk);_svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(mStock);
        }

        public ActionResult _AddNewStock()
        {
            var mdl = new StockEditVM
                {
                    ManufacturerAndModel = "No Model Selected!!",
                    ProductLifeCycleId = (int) ProductLifeCycleStatus.InStock,
                    InvoiceStatusId = (int) InvoiceStatusEnum.Pending,
                    ProductLifeCycles = _svc.GetProductLifeCycles().Where(m => m.NotAttachedStatus).ToList()
                };
            return PartialView("_Edit", mdl);
        }

        [HttpPost]
        public ActionResult _AddNewStock(StockEditVM mStockEditVm)
        {
            if (ModelState.IsValid)
            {
                var stk = new Stock();
                if (TryUpdateModel(stk))
                {
                    if (ExecuteRepositoryAction(() => { _svc.AddStock(stk); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            mStockEditVm.ProductLifeCycles = _svc.GetProductLifeCycles().Where(m => m.NotAttachedStatus).ToList();
            if(mStockEditVm.ModelId!=0)
            {
                var mdl = _svc.GetModel(mStockEditVm.ModelId);
                mStockEditVm.ManufacturerAndModel = mdl.Manufacturer.Name + " " + mdl.ModelCode;
            }
            
            return PartialView("_Edit", mStockEditVm);
        }

        [HttpPost]
        public ActionResult _GetManufactures()
        {
            var man = _svc.GetManufacturers().OrderBy(m => m.Name);
            return Json(new SelectList(man, "ManufacturerId", "Name"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetManufacturerModels(int? ManufacturerId)
        {
            IEnumerable<Model> mods=new List<Model>();
            if (ManufacturerId.HasValue)
            {
                mods = _svc.GetModels().Where(m => m.ManufacturerId == ManufacturerId).OrderBy(m=>m.ModelCode).Select(m=>new Model{ModelId =m.ModelId, ModelCode=m.ModelCode +" " + m.Category.Description + " Size "+ m.Size});
            }
            return Json(new SelectList(mods, "ModelId", "ModelCode"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetManufacturerModelDescription(int? id)
        {
            var retVal = "";
            if (id.HasValue)
            {
                var mdl = _svc.GetModel((int)id);
                retVal = mdl.Manufacturer.Name + " " + mdl.ModelCode;
            }
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }

}
