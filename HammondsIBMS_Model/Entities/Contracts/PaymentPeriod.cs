using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Invoicing;

namespace HammondsIBMS_Domain.Entities.Contracts
{

    public class PaymentPeriod
    {
        [Key]
        public int PaymentPeriodId { get; set; }

        [DisplayName("Period")]
        [Required]
        public string Description { get; set; }

        [DisplayName("No. of Months")]
        [UIHint("_NumericInt")]
        [Range(1,12,ErrorMessage ="Number of Months must be 1 to 12" )]
        public int PeriodInMonths { get; set; }



    }



}