using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_2.ViewModels.StockView
{
    public class StockEditVM
    {
        public int StockId { get; set; }

        [DisplayName("Model")]
        public string ManufacturerAndModel { get; set; }

        [Required]
        public int ModelId { get; set; }

        [DisplayName("Serial No.")]
        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string Identifier { get; set; }

        [DisplayName("Landed Cost")]
        [UIHint("Money")]
        public decimal LandedCost { get; set; }

        [UIHint("_ProductLifeCycles")]
        public int ProductLifeCycleId { get; set; }

        public List<ProductLifeCycle> ProductLifeCycles { get; set; }

        public int InvoiceStatusId { get; set; }
    }
}