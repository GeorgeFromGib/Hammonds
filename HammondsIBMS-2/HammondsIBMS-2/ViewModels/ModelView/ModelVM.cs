using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.ModelView
{
    public class ModelVM
    {
        public int ModelId { get; set; }
        public string Manufacturer { get; set; }

        [DisplayName("Model Code")]
        public string ModelCode { get; set; }
        public string Category { get; set; }
        public int Size { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        
        [DisplayName("Rental Price")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal RentalPrice { get; set; }

        [DisplayName("Retail Price")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal RetailPrice { get; set; }

        [DisplayName("Spain RP")]
        [DataType(DataType.Currency)]
        [UIHint("Euro")]
        public decimal SpainRetailPrice { get; set; }

        [DisplayName("In Stock")]
        public int NoInStock { get; set; }

        [DisplayName("Obsolete?")]
        public bool IsObsolete { get; set; }

    }
}