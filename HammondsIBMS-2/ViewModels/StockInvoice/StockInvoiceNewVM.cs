using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.StockInvoice
{
    public class NewStockInvoiceVM
    {

        [ScaffoldColumn(false)]
        public int StockInvoiceid { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [Required]
        [DisplayName("Reference")]
        public string InvoiceRef { get; set; }

        [Range(0,10000,ErrorMessage = "Supplier must be selected")]
        [DisplayName("Supplier")]
        [UIHint("_Supplier")]
        public int SupplierId { get; set; }

    }
}