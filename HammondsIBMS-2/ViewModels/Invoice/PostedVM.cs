using System;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Invoice
{
    public class PostedVM
    {
        public int PostedId { get; set; }

        public string ContractUnitDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public string InvoiceInfo { get; set; }

        public bool HasBeenInvoiced { get; set; }
    }
}