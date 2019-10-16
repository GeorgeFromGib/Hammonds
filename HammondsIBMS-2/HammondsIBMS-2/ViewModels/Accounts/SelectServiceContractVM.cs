using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    [Validator(typeof(SelectContractTypesVMValidator))]
    public class SelectContractTypesVM
    {
        public int UnitId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        public DateTime MinDate { get; set; }

        public List<ContractTypeVM> ContractTypes { get; set; }
    }

    public class SelectContractTypesVMValidator : AbstractValidator<SelectContractTypesVM>
    {
        public SelectContractTypesVMValidator()
        {
            RuleFor(v => v.PurchaseDate)
                .GreaterThanOrEqualTo(v => v.MinDate)
                .WithMessage("Date can not be earlier than the {0:d}", v => v.MinDate.Date);
        }
    }
}