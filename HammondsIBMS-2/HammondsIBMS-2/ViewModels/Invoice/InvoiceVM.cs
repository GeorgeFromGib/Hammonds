using System;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Invoicing;

namespace HammondsIBMS_2.ViewModels.Invoice
{
    public class InvoiceVM
    {
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Editable(false)]
        public BillCycle BillCycle { get; set; }
    }
}