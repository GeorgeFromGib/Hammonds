using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.Payment
{
    [Validator(typeof(MakePaymentVMValidator))]
    public class MakePaymentVM
    {
        public int CustomerId { get; set; }
        
        [DisplayName("Payment Type")]
        [UIHint("_PaymentSource")]
        public int PreferredPaymentSourceId { get; set; }

        [DisplayName("Reference")]
        public string Reference { get; set; }

        [DisplayName("Current Balance")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public List<RolledTransactionVM> RolledTransactions { get; set; }

        [DisplayName(" Settle In Full")]
        public bool SettleAllInFull { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Tendered")]
        [UIHint("Money")]
        public decimal TenderedAmount { get; set; }

        public int CreditedAccountId { get; set; }
        public decimal CreditedAmount { get; set; }

        public List<CustomerAccount> CustomerAccounts { get; set; }

        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class RolledTransactionVM
    {
        public int CustomerAccountId { get; set; }
        public string AccountType { get; set; }
        public string BillCycle { get; set; }
        public int BillCycleNo { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        public decimal Payment { get; set; }

        [DataType(DataType.Currency)]
        [Editable(false)]
        public decimal Balance { get; set; }
        public bool SettleInFull { get; set; }
        public bool Exclude { get; set; }
    }

    public class MakePaymentVMValidator:AbstractValidator<MakePaymentVM>
    {
        public MakePaymentVMValidator()
        {
            RuleFor(m => m.TenderedAmount).NotEqual(0).WithMessage("Tendered amount has not been entered.");
            RuleFor(m => m.CreditedAccountId).GreaterThan(0).When(m => m.CreditedAmount > 0).WithMessage("Account must be selected for credited amount");
        }
    }
}