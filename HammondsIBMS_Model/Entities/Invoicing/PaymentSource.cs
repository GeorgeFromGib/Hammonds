using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_Domain.Entities.Invoicing
{
    public class PaymentSource
    {
        [Key]
        [ScaffoldColumn(false)]
        public int PaymentSourceId { get; set; }

        [Required]
        [DisplayName("Payment Type")]
        public string Description { get; set; }
    }
}