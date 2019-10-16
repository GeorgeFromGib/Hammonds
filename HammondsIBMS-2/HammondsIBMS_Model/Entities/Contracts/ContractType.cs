using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    [Validator(typeof(ContractTypeValidator))]
    public class ContractType
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

        public bool Expired { get; set; }

        [DisplayName("Purchase Default")]
        public bool PurchaseDefault { get; set; }

        [DisplayName("Hide from Invoice")]
        public bool HideFromInvoice { get; set; }

        [DisplayName("Payment Period")]
        [UIHint("_PaymentPeriods")]
        public int PaymentPeriodId { get; set; }

        public virtual PaymentPeriod DefaultPaymentPeriod { get; set; }

        [DataType(DataType.MultilineText)] 
        public string Explanation { get; set; }
    
    }

    public class ContractTypeValidator:AbstractValidator<ContractType>
    {
        public ContractTypeValidator()
        {
            RuleFor(x => x.PeriodInMonths).GreaterThan(0).WithMessage("Period must not be less that 1 month");
            RuleFor(x => x.NormalTermAmount).GreaterThanOrEqualTo(0).WithMessage("Amount can not be Negative");
            RuleFor(x => x.NormalTermAmount).Equal(0).When(x => x.HideFromInvoice).WithMessage(
                "Amount must be 0 when hidden from Invoice");
        }
    }

  
}