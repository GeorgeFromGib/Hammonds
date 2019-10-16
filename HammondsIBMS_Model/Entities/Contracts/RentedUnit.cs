using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    [Validator(typeof(RentedUnitValidator))]
    public class RentedUnit : Unit
    {
        //public int RentedUnitId { get; set; }

        [DataType(DataType.Date)]
        public DateTime RentedDate { get; set;}


        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public decimal OldAmount { get; set; }

        [DataType(DataType.Currency)]
        public decimal Deposit { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidUntil { get; set; }

        public bool IsDepositRefunded { get; set; }

    }

    public class RentedUnitValidator : AbstractValidator<RentedUnit>
    {
        public RentedUnitValidator()
        {
            RuleFor(x => x.RemovalDate).GreaterThanOrEqualTo(o => o.RentedDate).WithMessage("Removal cannot be before rental date.");
            RuleFor(x => x.ChangedDate)
                .GreaterThanOrEqualTo(o => o.RentedDate)
                .WithMessage("Change date can not be before rental date.");
        }
    }

 
}