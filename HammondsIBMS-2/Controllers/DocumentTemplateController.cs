using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_2.ViewModels.DocumentTemplates;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class DocumentTemplateController : IbmsBaseController
    {
        private readonly DocumentTemplateService _documentTemplateService;

        public DocumentTemplateController(DocumentTemplateService documentTemplateService)
        {
            _documentTemplateService = documentTemplateService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult _DocumentTemplateGridBind()
        {
            var documentTemplateVms = _documentTemplateService.GetAllDocumentTemplates().Select(MapDocumentTemplateToVM);
            return View(new GridModel(documentTemplateVms));
        }

        public ActionResult Edit(int id)
        {
            var docTplt = _documentTemplateService.GetById(id);
            docTplt.Body = HttpUtility.HtmlDecode(docTplt.Body);
            var docTpltVM = MapDocumentTemplateToVM(docTplt);
            LoadViewBagWithAvailableFields(docTplt);
            return PartialView(docTpltVM);
        }

        [HttpPost]
        public ActionResult Edit(DocumentTemplateVM mDocumentTemplate)
        {
            var docTemplte = _documentTemplateService.GetById(mDocumentTemplate.DocumentTemplateId);
            if(ModelState.IsValid)
            {
                if(TryUpdateModel(docTemplte))
                {
                    docTemplte.Body = HttpUtility.HtmlDecode(docTemplte.Body);
                    if (ExecuteRepositoryAction(() => { _documentTemplateService.Update(docTemplte); _documentTemplateService.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            //mDocumentTemplate.Body = HttpUtility.HtmlDecode(mDocumentTemplate.Body);
            LoadViewBagWithAvailableFields(docTemplte);
            return PartialView(mDocumentTemplate);
        }

        private void LoadViewBagWithAvailableFields(DocumentTemplate docTemplate)
        {
            ViewBag.AvailableFields = docTemplate.GetAllowedFieldsWithDelimeters().OrderBy(c=>c)
                                             .Select(c => new SelectListItem { Text = c, Value = c });
        }

        private static DocumentTemplateVM MapDocumentTemplateToVM(DocumentTemplate docTplt)
        {
            return AutoMapper.Mapper.Map<DocumentTemplate, DocumentTemplateVM>(docTplt);
        }
    }
}
