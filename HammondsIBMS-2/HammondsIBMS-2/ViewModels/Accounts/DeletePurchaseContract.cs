using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    [Validator(typeof(DeletePurchaseContractPromptVMValidator))]
    public class DeletePurchaseContractPromptVM : PromptVM
    {
        [DisplayName("Contract to expire")]
        public string Message { get; set; }

        [DisplayName("Pro-rated Refund?")]
        public bool ProRateRefunded { get; set; }

        [DisplayName("Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        public DateTime SheduledExpiryDate { get; set; }

    }

    public class DeletePurchaseContractPromptVMValidator : AbstractValidator<DeletePurchaseContractPromptVM>
    {
        public DeletePurchaseContractPromptVMValidator()
        {
            RuleFor(r => r.ExpiryDate)
                .GreaterThanOrEqualTo(c => c.StartDate)
                .WithMessage("Expiry date can not be earlier than start date.");
            RuleFor(r => r.ExpiryDate)
                .LessThanOrEqualTo(c => c.SheduledExpiryDate)
                .WithMessage("Can not expire contract beyond scheduled expiry date.");
        }
    }
}