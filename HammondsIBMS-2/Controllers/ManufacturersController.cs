using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Manufacturer;
using HammondsIBMS_2.ViewModels.ModelView;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class ManufacturersController : IbmsBaseController
    {
        private readonly StockService _svc;

        public ManufacturersController(StockService svc)
        {
            _svc = svc;
        }


        public ActionResult Index()
        {

            return View();
        }



        [GridAction]
        public ActionResult _ManufacturerGridBind()
        {
            var vm = AutoMapperSetup.MapList<Manufacturer, ManufacturerVM>(_svc.GetAllManufacturers());
            return View(new GridModel(vm));
        }


        public ActionResult _AddManufacturer()
        {
            var man = new ManufacturerVM();
            return PartialView("Edit",man);
        }

        [HttpPost]
         public ActionResult _AddManufacturer(ManufacturerVM mManufacturerVm)
        {
            if (ModelState.IsValid)
            {
                var man = new Manufacturer();
                if (TryUpdateModel(man))
                {
                    if (ExecuteRepositoryAction(() => { _svc.AddManufacturer(man); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView("Edit", mManufacturerVm);
        }
        

        public ActionResult Edit(int id)
        {
            var vm = Mapper.Map<Manufacturer, ManufacturerVM>(_svc.GetManufacturer(id));
            return PartialView(vm);
        }

       

        [HttpPost]
        public ActionResult Edit(ManufacturerVM mManufacturerVm)
        {
            if (ModelState.IsValid)
            {
                var man = _svc.GetManufacturer(mManufacturerVm.ManufacturerId);
                if (TryUpdateModel(man))
                {
                    if (ExecuteRepositoryAction(() => { _svc.UpdateManufacturer(man); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(mManufacturerVm);
        }

 
        public ActionResult Delete(int id)
        {
            var mdl = _svc.GetManufacturer(id);
            var vm = new DeleteManufacturerVM { ManufacturerId = id, MakeObsolete = false };
            if (_svc.GetModels().Any(m => m.ManufacturerId == id))
            {
                vm.DeleteMessage = string.Format("Manufacturer {0} is attached to existing records and can not be deleted.", mdl.Name);
                vm.Prompt = "Do you wish to mark as retired?";
                vm.MakeObsolete = true;
            }
            else
            {
                vm.Prompt = "Do you wish to delete Manufacturer " + mdl.Name + " ?";
            }
            return PartialView("_DeleteManufacturer",vm);
        }

        [HttpPost]
        public ActionResult Delete(DeleteManufacturerVM mDeleteManufacturerVm)
        {
            if (ModelState.IsValid)
            {
                var mdl = _svc.GetManufacturer(mDeleteManufacturerVm.ManufacturerId);
                if (mDeleteManufacturerVm.MakeObsolete)
                {

                    mdl.Retired = true;
                    if (ExecuteRepositoryAction(() =>
                    {
                        _svc.UpdateManufacturer(mdl);
                        _svc.CommitChanges();
                    }))
                        return ReturnJsonFormSuccess();
                }
                if (ExecuteRepositoryAction(() =>
                {
                    _svc.DeleteManufacturer(mdl);
                    _svc.CommitChanges();
                }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_DeleteManufacturer",mDeleteManufacturerVm);
        }
    }
}
