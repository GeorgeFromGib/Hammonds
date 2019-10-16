using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_Domain.Entities.Stock
{

    public partial class Stock
    {
        [ScaffoldColumn(false)]
        public int StockId { get; set; }

        public int ModelId { get; set; }
        public virtual Model Model { get; set; }

        [DisplayName("Model")]
        public string ManufacturerModel
        {
            get { return Model==null?"":Model.Manufacturer.Name + " " + Model.ModelCode; }
        }

        [DisplayName("Serial Number")]
        public string SerialNumber { get; set;}

        [DisplayName("HTV Identifier")]
        public string Identifier { get; set; }

        [DisplayName("Landed Cost")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal LandedCost { get; set; }

        [DisplayName("Product Lifecycle")]
        [UIHint("_ProductLifeCycles")]
        public int ProductLifeCycleId { get; set; }
        public virtual ProductLifeCycle ProductLifeCycleStatus { get; set; }


        public int InvoiceStatusId { get; set; }
        public virtual InvoiceStatus InvoiceStatus { get; set; }

        public virtual List<StockHistory> History { get; set; }

       
        public int? CustomerAccountId { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }

       
    }

   

}