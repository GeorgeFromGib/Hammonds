using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_2.ViewModels.StockInvoice
{
    public class SelectStockForInvoiceVM
    {
        public int StockInvoiceId { get; set; }
        public List<SelectableStockVM> SelectedStock { get; set; }
    }

    public class SelectableStockVM
    {
        public bool IsSelected { get; set; }
        public int StockId { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

        [DisplayName("HTV-Identifier")]
        public string Identifier { get; set; }

        [DisplayName("Cost")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [DataType(DataType.Date)]
        [Editable(false)]
        public DateTime? LastEntry { get; set; }
    }
}