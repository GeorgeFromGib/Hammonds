using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Manufacturer;
using HammondsIBMS_2.ViewModels.ModelView;
using HammondsIBMS_Application;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class EmployerController : IbmsBaseController
    {
        private readonly EmployerService _employerService;

        public EmployerController(EmployerService employerService)
        {
            _employerService = employerService;
        }


        public ActionResult Index()
        {
            return View();
        }



        [GridAction]
        public ActionResult _EmployersGridBind()
        {
            var vm = AutoMapperSetup.MapList<Employer, EmployerVM>(_employerService.GetAllEmployers());
            return View(new GridModel(vm));
        }


        public ActionResult _AddEmployer()
        {
            var man = new Employer();
            return PartialView("Edit", man);
        }

        [HttpPost]
        public ActionResult _AddEmployer(Employer mEmployer)
        {
            if (ModelState.IsValid)
            {
                var man = new Employer();
                if (TryUpdateModel(man))
                {
                    if (ExecuteRepositoryAction(() => { _employerService.AddEmployer(man); _employerService.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView("Edit", mEmployer);
        }


        public ActionResult Edit(int id)
        {
            var vm = _employerService.GetEmployerById(id);
            return PartialView(vm);
        }



        [HttpPost]
        public ActionResult Edit(Employer mEmployer)
        {
            if (ModelState.IsValid)
            {
                var emp = _employerService.GetEmployerById(mEmployer.EmployerId);
                if (TryUpdateModel(emp))
                {
                    if (ExecuteRepositoryAction(() => { _employerService.UpdateEmployer(emp); _employerService.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(mEmployer);
        }

 
        //public ActionResult Delete(int id)
        //{
        //    var mdl = _employerService.GetManufacturer(id);
        //    var vm = new DeleteManufacturerVM { ManufacturerId = id, MakeObsolete = false };
        //    if (_employerService.GetModels().Any(m => m.ManufacturerId == id))
        //    {
        //        vm.DeleteMessage = string.Format("Manufacturer {0} is attached to existing records and can not be deleted.", mdl.Name);
        //        vm.Prompt = "Do you wish to mark as retired?";
        //        vm.MakeObsolete = true;
        //    }
        //    else
        //    {
        //        vm.Prompt = "Do you wish to delete Manufacturer " + mdl.Name + " ?";
        //    }
        //    return PartialView("_DeleteManufacturer",vm);
        //}

        //[HttpPost]
        //public ActionResult Delete(DeleteManufacturerVM mDeleteManufacturerVm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var mdl = _employerService.GetManufacturer(mDeleteManufacturerVm.ManufacturerId);
        //        if (mDeleteManufacturerVm.MakeObsolete)
        //        {

        //            mdl.Retired = true;
        //            if (ExecuteRepositoryAction(() =>
        //            {
        //                _employerService.UpdateManufacturer(mdl);
        //                _employerService.CommitChanges();
        //            }))
        //                return ReturnJsonFormSuccess();
        //        }
        //        if (ExecuteRepositoryAction(() =>
        //        {
        //            _employerService.DeleteManufacturer(mdl);
        //            _employerService.CommitChanges();
        //        }))
        //            return ReturnJsonFormSuccess();
        //    }
        //    return PartialView("_DeleteManufacturer",mDeleteManufacturerVm);
        //}
    }
}
