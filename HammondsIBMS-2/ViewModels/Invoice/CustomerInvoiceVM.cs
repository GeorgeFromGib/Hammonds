using System;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Invoice
{
    public class CustomerInvoiceVM
    {
        public int InvoiceId { get; set; }

        public int CustomerId { get; set; }

        public string BillCycle { get; set; }

        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }



        [DataType(DataType.Currency)]
        public decimal InvoicedTotal { get; set; }


        [DataType(DataType.Currency)]
        public decimal PreviousBalance { get; set; }

        [DataType(DataType.Currency)]
        public decimal InvoiceBalance { get; set; }

    }
}