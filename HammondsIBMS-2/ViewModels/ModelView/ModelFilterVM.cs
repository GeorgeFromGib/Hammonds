using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_2.ViewModels.BaseVM;

namespace HammondsIBMS_2.ViewModels.ModelView
{
    public class ModelFilterVM : FilterBaseVM
    {
        [DisplayName("Model Code")]
        public string ModelCode { get; set; }

        [DisplayName("Manufacturer")]
        [UIHint("Manufacturer")]
        public int ManufacturerFilterId { get; set; }

        [DisplayName("Category")]
        [UIHint("Category")]
        public int CategoryFilterId { get; set; }
    }
}