using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class ServiceContractVM
    {
        public bool IsSelected { get; set; }

        public int ContractId { get; set; }
        public int CustomerAccountId { get; set; }

        [DisplayName("Contract Type")]
        [UIHint("_ContractTypes")]
        public int ContractTypeId { get; set; }

        [DisplayName("Contract Type")]
        public string ContractTypeDescription { get; set; }

        [DisplayName("Status")]
        public string ContractStatusDescription { get; set; }

        [DisplayName("Payment Period")]
        public string PaymentPeriodDescription { get; set; }

        [DisplayName("Cost")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal Charge { get; set; }

        [DisplayName("Length (m)")]
        [UIHint("_NumericInt")]
        public int ContractLengthInMonths { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [DisplayName("Days to Expiry")]
        [UIHint("_TimeSpanFormat")]
        public int NoOfDaysLeft { get; set; }

        [DisplayName("Expiry")]
        public string TimeToExpiry { get; set; }

        [DisplayName("Period Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public  decimal PeriodPaymentAmount { get; set; }

        public bool ExpiryWarning { get; set; }

        public bool IsExpiredToday { get; set; }

        public bool IsScheduled { get; set; }
    }
}