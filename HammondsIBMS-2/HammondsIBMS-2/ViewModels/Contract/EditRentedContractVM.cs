using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class EditRentedContractVM
    {
        public int CustomerAccountId { get; set; }

        public int ContractId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [DisplayName("Monthly Charge")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal MonthlyCharge { get; set; }

        [DisplayName("No. Units")]
        public int NoOfUnits { get; set; }

        [DisplayName("Payment Period")]
        [UIHint("_PaymentPeriods")]
        public int PaymentPeriodId { get; set; }

        public int OldPaymentPeriodId { get; set; }

        [DisplayName("Payment Period Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal PeriodPaymentAmount { get; set; }

        [DisplayName("Contract Status")]
        public string ContractStatus { get; set; }

        [DisplayName("Payment Period")]
        public string PaymentPeriodDescription { get; set; }

    }
}