using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Invoicing;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class EditServiceContractVM
    {
        [ScaffoldColumn(false)]
        [Key]
        public int ContractId { get; set; }

        [DisplayName("Contract Type")]
        public string ContractTypeDescription { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [ScaffoldColumn(false)]
        public int CustomerAccountId { get; set; }

        [DisplayName("Cost")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public  decimal Charge { get; set; }

        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        public  DateTime? ExpiryDate { get; set; }

        [DisplayName("Payment Period Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public  decimal PeriodPaymentAmount { get; set; }


        [DisplayName("Payment Period")]
        [UIHint("_PaymentPeriods")]
        public int PaymentPeriodId { get; set; }

        [ScaffoldColumn(false)]
        public PaymentPeriod PaymentPeriod { get; set; }

        public BillCycle LastBilled { get; set; }

        [DisplayName("Contract Length (months)")]
        [UIHint("_NumericInt")]
        public int ContractLengthInMonths { get; set; }

        [DisplayName("Expiry")]
        public string TimeToExpiry { get; set; }

        [DisplayName("Contract Status")]
        public string ContractStatus { get; set; }
    }
}