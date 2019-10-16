using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.Suppliers
{
    public class SupplierVM
    {
        public int SupplierId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [DisplayName("Main Contact")]
        public string MainContact { get; set; }

        [DisplayName("Preferred Currency")]
        [UIHint("ExchangeRate")]
        public int ExchangeRateId { get; set; }

        [DisplayName("Preferred Currency")]
        public string PreferedExchangeRate { get; set; }

        [DisplayName("Contact Info.")]
        [UIHint("_ContactInfo")]
        public  ContactInfo ContactInfo { get; set; }

        [UIHint("Address")]
        public  Address Address { get; set; }

    }
}