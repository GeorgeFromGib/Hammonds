using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Suppliers;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Supplier;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace HammondsIBMS_2.Controllers
{

    public class SuppliersController : IbmsBaseController
    {
        private ISupplierService _svc;

        public SuppliersController(ISupplierService svc)
        {
            _svc = svc;
            InitialiseExchangeRateViewBag(svc);
        }
        //
        // GET: /Suppliers/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult _SuppliersGridBind()
        {
            return View(new GridModel(AutoMapperSetup.MapList<Supplier,SupplierVM>(_svc.GetAllSuppliers())));
        }

        public ActionResult Edit(int id)
        {
            return PartialView(MapToVM(_svc.GetSupplier(id)));
        }

        private SupplierVM MapToVM(Supplier supplier)
        {
            return Mapper.Map<Supplier, SupplierVM>(supplier);
        }

        [HttpPost]
        public ActionResult Edit(SupplierVM mSupplierVm)
        {
            if (ModelState.IsValid)
            {
                var sup = _svc.GetSupplier(mSupplierVm.SupplierId);
                if (TryUpdateModel(sup))
                {
                    if (ExecuteRepositoryAction(() => { _svc.UpdateSupplier(sup); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
                
            }
            return PartialView(mSupplierVm);
        }

        public ActionResult Add()
        {
            var sup = new Supplier {ExchangeRateId = 1};
            return PartialView("Edit", MapToVM(sup));
        }

        [HttpPost]
        public ActionResult Add(SupplierVM mSupplierVm)
        {
            if (ModelState.IsValid)
            {
                var sup = new Supplier();
                if (TryUpdateModel(sup))
                {
                    if (ExecuteRepositoryAction(() => { _svc.AddSupplier(sup); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView("Edit", mSupplierVm);
        }
    }
}
