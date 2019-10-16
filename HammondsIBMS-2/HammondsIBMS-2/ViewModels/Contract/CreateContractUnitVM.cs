using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.ViewModels.Contract
{
    //[Validator(typeof(CreateContractUnitVMValidator))]
    public class CreateContractUnitVM
    {

        public int CustomerId { get; set; }
        public AccountType ContractUnitType { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [DisplayName("Rental Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal RentalAmount { get; set; }

        public int StockId { get; set; }
        public bool IsStockInAccountAlready { get; set; }

        [DisplayName("Payment Period")]
        [UIHint("_PaymentPeriods")]
        public int PaymentPeriodId { get; set; }

        [DisplayName("Payment Period Amount")]
        [DataType(DataType.Currency)]
        public decimal PeriodPaymentAmount { get; set; }

    }

    public class CreateContractUnitVMValidator:AbstractValidator<CreateContractUnitVM>
    {
        public CreateContractUnitVMValidator()
        {
            //RuleFor(x => x.StockId).NotEmpty();
            RuleFor(x => x.IsStockInAccountAlready).Must(m=>m==false).WithMessage(
                "Selected Stock is already allotted to another account");
        }
    }

    

}