using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_2.ViewModels.BaseVM;

namespace HammondsIBMS_2.ViewModels.ModelView
{


    public class StockFilterVM : FilterBaseVM
    {
        [DisplayName("Model Code")]
        public string ModelCode { get; set; }

        [DisplayName("Serial Number")]
        public string SerialNumber { get; set;}

        [DisplayName("HTV Identifier")]
        public string HTVIdentifier { get; set; }

        [DisplayName("In Stock Only")]
        public bool IncludeinStockOnly { get; set; }
    }
}