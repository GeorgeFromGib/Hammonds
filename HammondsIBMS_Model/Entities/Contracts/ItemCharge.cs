using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public class ItemCharge
    {
        public int ItemChargeId { get; set; }

        [Required]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DisplayName("Rental Default")]
        public bool RentalDefault { get; set; }

        [DisplayName("Purchase Default")]
        public bool PurchaseDefault { get; set; }

    }
}
