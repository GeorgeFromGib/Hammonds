using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Documents;

namespace HammondsIBMS_2.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ContentType _contentType;
        private readonly IDocumentService _documentService;
        private string _tempDocumentPath;

        public static string SessionName="_DocumentTempArea";

        public DocumentController(ContentType contentType,IDocumentService documentService)
        {
            _contentType = contentType;
            _documentService = documentService;
            
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CreateDocumentTempArea();
        }

        private void CreateDocumentTempArea()
        {
            var sessionDocTempArea = Server.MapPath(Session[SessionName] as string);
            var sessionFileWorkSpacePath = sessionDocTempArea;

            if (!Directory.Exists(sessionFileWorkSpacePath))
            {
                Directory.CreateDirectory(sessionFileWorkSpacePath);
            }

            _tempDocumentPath = sessionDocTempArea;

        }


        public ActionResult UploadDocumentFile(IEnumerable<HttpPostedFileBase> documentUpload)
        {
            // The Name of the Upload component is "attachments"            
            foreach (var file in documentUpload)
            {
                // Some browsers send file names with full path. This needs to be stripped.                
                var fileName = Path.GetFileName(file.FileName);
                var physicalPath = Path.Combine(_tempDocumentPath, fileName);
                file.SaveAs(physicalPath);
                return Json(new { Path = Server.HtmlEncode(physicalPath) }, "text/plain");
            }
            // Return an empty string to signify success  - reached if no files          
            return Content("");
        }

        public ActionResult ReturnDocumentForDisplay(Document document)
        {
            return File(document.ImageBytes, _contentType.GetContentTypeFromExtension(document.ContentType),document.FileName);

        }

        public ActionResult ReturnJsonDocument(Document document)
        {
            return Json(new { base64image = Convert.ToBase64String(document.ImageBytes), type = _contentType.GetContentTypeFromExtension(document.ContentType) }
                , JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public ActionResult ReturnDocumentForDisplayById(int id)
        {
            var cd = _documentService.GetDocument(id);
            return ReturnDocumentForDisplay(cd);
        }

        public static void RemoveFile(string filePath)
        {
            if(System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

    }
}
