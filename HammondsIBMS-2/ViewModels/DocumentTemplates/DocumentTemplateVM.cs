using System.Collections.Generic;
using System.Web.Mvc;

namespace HammondsIBMS_2.ViewModels.DocumentTemplates
{
    public class DocumentTemplateVM 
    {
        public int DocumentTemplateId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<SelectListItem> AvailableFields { get; set; }

    }
}