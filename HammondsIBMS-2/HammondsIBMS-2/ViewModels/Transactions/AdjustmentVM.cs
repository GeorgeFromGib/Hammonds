using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.Transactions
{
    [Validator(typeof(AdjustmentVMValidator))]
    public class AdjustmentVM
    {
        [ScaffoldColumn(false)]
        public int CustomerId { get; set; }

        public int CustomerAccountId { get; set; }

        [DisplayName("Account")]
        public string CustomerAccount { get; set; }

        public string Note { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("CurrencyWithNegatives")]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Transaction Type")]
        public string AccountTransactionType { get; set; }

        public int AccountTransactionTypeId { get; set; }

        public int GroupId { get; set; }
    }

    public class AdjustmentVMValidator : AbstractValidator<AdjustmentVM>
    {
        public AdjustmentVMValidator()
        {
           
            RuleFor(v => v.Amount).NotEqual(0).WithMessage("Amount can not be zero");
            RuleFor(v => v.Note).NotEmpty();
        }
    }
}