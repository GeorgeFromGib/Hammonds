using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    public class ContractTypeVM
    {
        public int ContractTypeId { get; set; }

        public string Description { get; set; }

        [DisplayName("Length (m)")]
        [UIHint("_NumericInt")]
        public int PeriodInMonths { get; set; }

        [DisplayName("Normal Term Charge")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal NormalTermAmount { get; set; }


        [DisplayName("Payment Period")]
        [UIHint("_PaymentPeriods")]
        public string PaymentPeriodDescription { get; set; }
         
    }
}