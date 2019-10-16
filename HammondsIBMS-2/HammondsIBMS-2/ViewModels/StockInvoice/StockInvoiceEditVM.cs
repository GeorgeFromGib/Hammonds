using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.StockInvoice
{
    public class StockInvoiceVM
    {

        public int StockInvoiceid { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? InvoiceDate { get; set; }

        [Required]
        [DisplayName("Reference")]
        public string InvoiceRef { get; set; }


        [DisplayName("Supplier")]
        [UIHint("_Supplier")]
        public int SupplierId { get; set; }

        public string Supplier { get; set; }

        [DisplayName("Invoice Currency")]
        [UIHint("ExchangeRate")]
        public int ExchangeRateId { get; set; }

        [DisplayName("Invoice Exchange Rate")]
        [UIHint("_Numeric")]
        [Range(0,1000)]
        public decimal NativeCurrencyToGBP { get; set; }

        [UIHint("ForeignCurrency")]
        public ForeignCurrency Freight { get; set; }

        [UIHint("ForeignCurrency")]
        public ForeignCurrency InvoiceCurrency { get; set; }

        [DisplayName("Other Charges")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal OtherCharges { get; set; }

        [DisplayName("Total Charges")]
        [DataType(DataType.Currency)]
        public decimal TotalCharges { get; set; }


        [DisplayName("Invoice Total")]
        [DataType(DataType.Currency)]
        public decimal InvoiceTotal { get; set; }

        [DisplayName("Items Total")]
        [UIHint("ForeignCurrency")]
        public ForeignCurrency ItemsTotal { get; set; }

        [DisplayName("Date Closed")]
        [DataType(DataType.Date)]
        public DateTime? DateProcessed { get; set; }

        [DisplayName("Close Invoice?")]
        public bool IsProcessed { get; set; }

        [DisplayName("Invoice Cancelled?")]
        public bool IsCancelled { get; set; }
    }

    public class StockInvoiceChargesVM
    {
        public int StockInvoiceid { get; set; }

        [UIHint("ForeignCurrency")]
        public ForeignCurrency Freight { get; set; }

        [DisplayName("Other Charges")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal OtherCharges { get; set; }
    }
}
