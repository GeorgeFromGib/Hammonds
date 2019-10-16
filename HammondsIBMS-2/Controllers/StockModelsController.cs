using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.ModelView;
using HammondsIBMS_2.ViewModels.StockView;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class StockModelsController : FilterController<ModelFilterVM>
    {
        private readonly StockService _svc;
        private readonly DocumentController _documentController;
        private readonly ContractService _contractService;

        public StockModelsController(StockService svc,DocumentController documentController,ContractService contractService)
        {
            _svc = svc;
            _documentController = documentController;
            _contractService = contractService;
            InitialiseManufacturesViewBag(svc);
            InitialiseCategoriesViewBag(svc);
            InitialiseContractTypeViewBag(contractService,true);
        }

        public ActionResult Index()
        {
            return View();
        }


        #region Image

        [OutputCache(Duration = 0)]
        public ActionResult _EditModelImage(int id)
        {
            var mdl = _svc.GetModel(id);
            var mdlImg = mdl.Image ?? new ModelImage{ModelId = id};
            var mdlImgVm = Mapper.Map<ModelImage,ModelImageEditVM>(mdlImg);
            mdlImgVm.ModelCode = mdl.ModelCode;
            return PartialView("_EditModelImage",mdlImgVm);
        }


        [HttpPost]
        public ActionResult _EditModelImage(ModelImageEditVM mModelImageVm)
        {
            var mdl = _svc.GetModel(mModelImageVm.ModelId);
            if (string.IsNullOrEmpty(mModelImageVm.FilePath))
            {
                if (mdl.Image != null)
                {
                    _svc.DeleteImage((int)mdl.ImageId);
                    mdl.ImageId = null;
                }
            }
            else
            {
                if (mdl.Image == null) mdl.Image = new ModelImage ();
                UpdateModel(mdl.Image);
                mdl.Image.LoadFile(mModelImageVm.FilePath); 
                DocumentController.RemoveFile(mModelImageVm.FilePath);
            }
                
            if (ExecuteRepositoryAction(() => {  _svc.UpdateModel(mdl);_svc.CommitChanges(); }))
                return ReturnJsonFormSuccess();
            return PartialView("_EditModelImage",mModelImageVm);
        }

        [OutputCache(Duration = 0)]
        public ActionResult _ReturnModelImage(int id)
        {
            var mdl = _svc.GetModel(id).Image;
            var retImge = mdl==null?null:_documentController.ReturnDocumentForDisplayById(mdl.DocumentId);
            return retImge ?? File(@"~/Content/site/img/noimage.gif", "image/gif");
        }

        [OutputCache(Duration = 0)]
        public ActionResult _ReturnModelImageJson(int id)
        {
            var mdl = _svc.GetModel(id).Image;

            if(mdl==null)
                return Json(new{base64image =Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/Content/site/img/noimage.gif"))),
                            type = "image/gif"
                }, JsonRequestBehavior.AllowGet);
            return _documentController.ReturnJsonDocument(mdl);
        }

        public ActionResult _DisplayExpandedImage(int id)
        {
            return PartialView(id);
        }

        #endregion

        #region List and Filter

        [GridAction]
        public ActionResult _StockModelsGridBind()
        {
            var mdl = FilterVM == null ? _svc.GetModels() : _svc.GetModels().Where(m => m.ModelCode.ToUpper().StartsWith(FilterVM.ModelCode==null?"":FilterVM.ModelCode.ToUpper()) && 
                                                                                                    (FilterVM.ManufacturerFilterId==-1 || m.ManufacturerId==FilterVM.ManufacturerFilterId) && 
                                                                                                    (FilterVM.CategoryFilterId==-1 || m.CategoryId==FilterVM.CategoryFilterId));
            var vm = AutoMapperSetup.MapList<Model, ModelVM>(mdl);
            var modelVms = vm as List<ModelVM> ?? vm.ToList();
            foreach (var modelVm in modelVms)
            {
                modelVm.NoInStock = GetStockForModel(modelVm.ModelId);
            }
            return View(new GridModel(modelVms));
        }


        protected override ModelFilterVM ResetFilterVM()
        {
            return new ModelFilterVM { ModelCode = "", ManufacturerFilterId = -1, CategoryFilterId = -1, IsFiltered = false };
        }


        protected override void AddSelectors()
        {
            ViewBag.Manufacturers = AddAllSelector(new Manufacturer {ManufacturerId = -1, Name = "---"},
                                                   ViewBag.Manufacturers);
            ViewBag.Categories = AddAllSelector(new Category {CategoryId = -1, Description = "---"}, ViewBag.Categories);
        }


        //[HttpPost]
        //public ActionResult _FilterModelList(ModelFilterVM mModelFilterVm)
        //{
        //    mModelFilterVm.IsFiltered = true;
        //    ModelFilter = mModelFilterVm;
        //    return ReturnJsonFormSuccess();
        //}

        //[OutputCache(Duration = 0)]
        //public ActionResult _FilterModelReset()
        //{
        //    AddSelectors();
        //    ModelFilter = ResetModelFilterVM();
        //    return PartialView("_FilterModelList",ModelFilter);
        //}

        #endregion

        #region Edit Model
        

        public ActionResult AddNewModel()
        {
            var model = new Model {CategoryId = 1, ManufacturerId = 1};
            return PartialView("_EditModelDetails",model);
        }

        [HttpPost]
        public ActionResult AddNewModel(Model mModel)
        {
            if (ModelState.IsValid && TryValidateModel(mModel))
            {
                if(ExecuteRepositoryAction(()=> { _svc.AddModel(mModel);_svc.CommitChanges(); }))
                    return ReturnJsonFormSuccess(mModel.ModelId);
            }
            return PartialView("_EditModelDetails", mModel);
        }


        public ActionResult Edit(int id)
        {
            var vm = GetModelVm(id);
            return View(vm);
        }

        private ModelVM GetModelVm(int id)
        {
            var vm = Mapper.Map<Model, ModelVM>(_svc.GetModel(id));
            return vm;
        }

        public ActionResult _EditModelDetails(int id)
        {
            return PartialView(_svc.GetModel(id));
        }

        [HttpPost]
        public ActionResult _EditModelDetails(Model mModel)
        {
            var mdl = _svc.GetModel(mModel.ModelId);
            if (ModelState.IsValid && TryUpdateModel(mdl))
            {
                if (ExecuteRepositoryAction(() => { _svc.UpdateModel(mdl); _svc.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mModel);
        }

        public ActionResult _DeleteModel(int id)
        {
            var mdl = _svc.GetModel(id);
            var vm = new DeleteModelVM{ModelId = id,MakeObsolete = false};
            var promptVm = new PromptVM{RecordIndex = id};
            if (_svc.GetStocks().Any(m => m.ModelId == id))
            {
                promptVm.AddPrompt(new DialogPrompt(string.Format(
                    "Model {0} has a stock history and cannot be deleted.",
                    mdl.ModelCode), PromptMessageAlertLevel.Danger));
                promptVm.AddPrompt(new DialogPrompt("Do you wish to mark as obsolete?", PromptMessageAlertLevel.Warning));
            }
            else
            {
                promptVm.AddPrompt(new DialogPrompt("Do you wish to delete model " + mdl.ModelCode+" ?", PromptMessageAlertLevel.Warning));
            }
            return PartialView("_PromptDialog",promptVm);
        }

        [HttpPost]
        public ActionResult _DeleteModel(PromptVM promptVm)
        {
            if (ModelState.IsValid)
            {
                var mdl = _svc.GetModel(promptVm.RecordIndex);
                if (_svc.GetStocks().Any(m => m.ModelId==mdl.ModelId))
                {

                    mdl.IsObsolete = true;
                    if (ExecuteRepositoryAction(() =>
                                                    {
                                                        _svc.UpdateModel(mdl);
                                                        _svc.CommitChanges();
                                                    }))
                        return ReturnJsonFormSuccess();
                }
                if (ExecuteRepositoryAction(() =>
                                                {
                                                    _svc.DeleteModel(mdl);
                                                    _svc.CommitChanges();
                                                }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_PromptDialog",promptVm);
        }

        #endregion

        public ActionResult _DisplayModelEditHeader(int id)
        {
            return PartialView(GetModelVm(id));
        }

        public ActionResult _DisplayStockForModel(int id)
        {
            return PartialView(GetModelVm(id));
        }

        private IEnumerable<StockVM> GetStockListVM(Func<Stock, bool> predicate)
        {
            return AutoMapperSetup.MapList<Stock, StockVM>(_svc.GetStocks().Where(predicate));
        }

        [GridAction]
        public ActionResult _InStockForModelGridBind(int id,bool? showInStockOnly)
        {
            var pred = (showInStockOnly != null && showInStockOnly.Value) ? ModelInStockPredicate(id) : (c=>c.ModelId==id);
            return View(new GridModel(GetStockListVM(pred)));
        }

        private Func<Stock, bool> ModelInStockPredicate(int id)
        {
            return
                m =>
                m.ModelId == id &&
                (m.ProductLifeCycleId == (int)ProductLifeCycleStatus.InStock ||
                 m.ProductLifeCycleId == (int)ProductLifeCycleStatus.ReRental);
        }

        public ActionResult _StockNoForModel(int id)
        {
            return PartialView(GetStockForModel(id));
        }

        private int GetStockForModel(int id)
        {
            return _svc.GetStocks().Count(ModelInStockPredicate(id));
        }

        public ActionResult _AddStockForModel(int id)
        {
            var mdl = _svc.GetModel(id);
            var vm = new StockEditVM
                         {
                             ModelId = id,
                             ManufacturerAndModel = mdl.Manufacturer.Name+" "+mdl.ModelCode,
                             ProductLifeCycleId = (int) ProductLifeCycleStatus.InStock,
                             InvoiceStatusId = (int) InvoiceStatusEnum.Pending
                         };
            return PartialView("_EditStockForModel", vm);
        }

        [HttpPost]
        public ActionResult _AddStockForModel(StockEditVM mStockEditVm)
        {
            if (ModelState.IsValid && TryValidateModel(mStockEditVm))
            {
                var stk = new Stock {History = new List<StockHistory>()};
                if (TryUpdateModel(stk))
                {
                    if (ExecuteRepositoryAction(() => { _svc.AddStock(stk); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView("_EditStockForModel", mStockEditVm);
        }

        public ActionResult _EditStockForModel(int id)
        {
            var vm = Mapper.Map<Stock, StockEditVM>(_svc.GetStock(id));
            return PartialView("_EditStockForModel", vm);
        }

        [HttpPost]
        public ActionResult _EditStockForModel(StockEditVM mStockEditVm)
        {
            if (ModelState.IsValid && TryValidateModel(mStockEditVm))
            {
                var stk = _svc.GetStock(mStockEditVm.StockId);
                if (TryUpdateModel(stk))
                {
                    if (ExecuteRepositoryAction(() => { _svc.UpdateStock(stk); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView("_EditStockForModel", mStockEditVm);
        }

        public ActionResult _DisplayModelDetails(int id)
        {
            var vm = GetModelVm(id);
            return PartialView(vm);
        }

        public ActionResult _DisplayModelDetailsBody(int id)
        {
            var vm = GetModelVm(id);
            return PartialView(vm);
        }


        #region Model Specific Contracts

        public ActionResult _DisplayModelSpecificContracts(int id)
        {
            return PartialView(GetModelVm(id));
        }

        [GridAction]
        public ActionResult _GetModelSpecificContracts(int id)
        {
            var modelContracts = _svc.GetModel(id).ModelSpecificContracts;
            var modelContractsVM =
                AutoMapperSetup.MapList<ModelSpecificContract, ModelSpecificContractVM>(modelContracts);
            return View(new GridModel(modelContractsVM));
        }

        public ActionResult _AddModelSpecificContract(int id)
        {
            var newContractVm = new ModelSpecificContractVM {ModelId = id,ContractTypeId = -1};
            return PartialView("_EditModelSpecificContract",newContractVm);
        }

        [HttpPost]
        public ActionResult _AddModelSpecificContract(ModelSpecificContractVM modelSpecificContractVm)
        {
            if(ModelState.IsValid)
            {
                var modelSpecificContract = new ModelSpecificContract();
                if(TryUpdateModel(modelSpecificContract))
                {
                    var model = _svc.GetModel(modelSpecificContractVm.ModelId);
                    model.ModelSpecificContracts.Add(modelSpecificContract);
                    if (ExecuteRepositoryAction(() => { _svc.UpdateModel(model); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView("_EditModelSpecificContract",modelSpecificContractVm);
        }

        public ActionResult _EditModelSpecificContract(int id)
        {
            var newContractVm = new ModelSpecificContractVM { ModelId = id };
            return PartialView(newContractVm);
        }

        [HttpPost]
        public ActionResult _EditModelSpecificContract(ModelSpecificContractVM modelSpecificContractVm)
        {
            if (ModelState.IsValid)
            {
                var modelSpecificContract =
                    _svc.GetModelSpecificContract(modelSpecificContractVm.ModelSpecificContractId);
                if (TryUpdateModel(modelSpecificContract))
                {
                    if (ExecuteRepositoryAction(() => { _svc.UpdateModelSpecificContract(modelSpecificContract); _svc.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(modelSpecificContractVm);
        }

        public ActionResult _DeleteModelSpecificContract(int id)
        {
            var contract = _svc.GetModelSpecificContract(id);
            var promptVM = new PromptVM(id,
                                        new DialogPrompt(string.Format("Remove Contract : {0}?",
                                                                       contract.ContractType.Description), PromptMessageAlertLevel.Warning));
          
            return PartialView("_PromptDialog", promptVM);
        }

        [HttpPost]
        public ActionResult _DeleteModelSpecificContract(PromptVM promptVm)
        {
            if(ModelState.IsValid)
            {
                if (ExecuteRepositoryAction(() => { _svc.DeleteModelSpecificContract((int)promptVm.RecordIndex); _svc.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_PromptDialog", promptVm);
        }

        public ActionResult _GetContractTypeDetails(int id)
        {
            return Json(_contractService.GetContractType(id), JsonRequestBehavior.AllowGet);
        }



        #endregion

       
    }

    
}
