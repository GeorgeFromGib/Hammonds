using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_Domain.Entities.Accounts
{
    public class OneOffItem
    {
        public int OneOffItemId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Range(1,999999,ErrorMessage ="Quantity must be greater than 0")]
        [UIHint("_NumericInt")]
        public int Quantity { get; set; }

        public int OneOffTypeId { get; set; }
        public virtual OneOffType OneOffType { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal Charge { get; set; }

        public DateTime? RemovalDate { get; set; }

        public bool Refunded { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Total Charge")]
        public decimal TotalCharge {
            get { return Quantity*Charge; }
        }
    }
}