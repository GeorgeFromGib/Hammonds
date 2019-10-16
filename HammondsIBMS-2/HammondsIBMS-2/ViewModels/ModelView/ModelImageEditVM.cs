using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HammondsIBMS_2.ViewModels.Documents;

namespace HammondsIBMS_2.ViewModels.ModelView
{
    public class ModelImageEditVM:DocumentVM
    {
        [ScaffoldColumn(false)]
        public int ModelId { get; set; }

        [DisplayName("Model Code")]
        [HiddenInput]
        public string ModelCode { get; set; }

    }
}