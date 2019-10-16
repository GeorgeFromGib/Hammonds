using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Supplier
{
    public partial class Supplier
    {
        [ScaffoldColumn(false)]
        public int SupplierId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [DisplayName("Main Contact")]
        public string MainContact { get; set; }

        [DisplayName("Preferred Currency")]
        [UIHint("ExchangeRate")]
        public int ExchangeRateId { get; set; }

        [DisplayName("Preferred Currency")]
        [ScaffoldColumn(false)]
        public  virtual ExchangeRate PreferedExchangeRate { get; set; }

        [DisplayName("Contact Info.")]
        [UIHint("_ContactInfo")]
        public  ContactInfo ContactInfo { get; set; }

        [UIHint("Address")]
        public  Address Address { get; set; }

        public Supplier()
        {
            ContactInfo=new ContactInfo();
            Address=new Address();
        }
    }
}
