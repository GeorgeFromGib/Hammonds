using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Documents;
using HammondsIBMS_Domain.Model.Product;

namespace HammondsIBMS_Domain.Entities.Stock
{
    public partial class Model 
    {
        [ScaffoldColumn(false)]
        public int ModelId { get; set; }

        [Required]
        [DisplayName("Manufacturer")]
        [UIHint("Manufacturer")]
        public int ManufacturerId { get; set; }

        [ScaffoldColumn(false)]
        public virtual Manufacturer Manufacturer { get; set; }

        [Required]
        [DisplayName("Model Code")]
        public string ModelCode { get; set; }

        [StringLength(100,ErrorMessage = "The Spec. Summary cannot be more than 100 characters long")]
        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
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


        [DisplayName("Model Hyperlink")]
        [DataType(DataType.Url)]
        public string ModelHyperLink { get; set; }

        public int? ImageId { get; set; }
        public virtual ModelImage Image { get; set; }

        [DisplayName("Category")]
        [UIHint("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category{ get; set; }

        public int Size { get; set; }

        public bool IsObsolete { get; set; }

        public virtual List<ModelSpecificContract> ModelSpecificContracts { get; set; }

    }
}