using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_Domain.Model.Supplier
{
    public partial class ExchangeRate
    {
        [ScaffoldColumn(false)]
        public int ExchangeRateId { get; set; }

        [StringLength(1)]
        [Required]
        public string Symbol { get; set; }

        [DisplayName("Currency Abbreviation")]
        public string Abbreviation { get; set; }

        [DisplayName("Currency Name")]
        public string CurrencyName { get; set; }

        [Required]
        public decimal RateToGBP { get; set; }
    }
}