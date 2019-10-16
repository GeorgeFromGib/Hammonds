using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_2.Controllers;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    [Validator(typeof(DeletPurchaseUnitPromptVMValidator))]
    public class DeletPurchaseUnitPromptVM : PromptVM
    {
        public string Model { get; set; }

        [DisplayName("Removal Date")]
        [DataType(DataType.Date)]
        public DateTime RemovalDate { get; set; }

        [DisplayName("Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Refund?")]
        public bool RefundAmount { get; set; }
        
        [DisplayName("Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DisplayName("Change to")]
        public int ProductLifeCycleId { get; set; }

        public DeletPurchaseUnitPromptVM(int recordIndex,DialogPrompt dialogPrompt):base(recordIndex,dialogPrompt)
        {
            
        }

        public DeletPurchaseUnitPromptVM()
        {
            RefundAmount = false;
            RemovalDate = DateTime.Today;
        }
    }

    public class DeletPurchaseUnitPromptVMValidator : AbstractValidator<DeletPurchaseUnitPromptVM>
    {
        public DeletPurchaseUnitPromptVMValidator()
        {
            RuleFor(c => c.RemovalDate)
                .GreaterThanOrEqualTo(s => s.PurchaseDate)
                .WithMessage("Removal Date can not be earlier than Purchase Date");
        }
    }
}