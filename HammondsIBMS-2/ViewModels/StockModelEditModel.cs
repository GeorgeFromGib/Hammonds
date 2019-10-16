using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_2.ViewModels.Manufacturer;

namespace HammondsIBMS_2.ViewModels
{

    public class StockModelEditModel
    {
        [ScaffoldColumn(false)]
        public int ModelId { get; set; }

        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Model Code")]
        public string ModelCode { get; set; }

        [UIHint("Manufacturer"), Required]
        public ManufacturerVM Manufacturer { get; set; }

 

        public bool Discontinued { get; set; }

        

        public StockModelEditModel()
        {}

    }
}