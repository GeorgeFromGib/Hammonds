using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.ModelView
{
    [Validator(typeof(ModelSpecificContractVMValidator))]
    public class ModelSpecificContractVM
    {
        public int ModelSpecificContractId { get; set; }

        [DisplayName("Contract")]
        [UIHint("_ContractTypes")]
        public int ContractTypeId { get; set; }

        public string ContractDescription { get; set; }

        [DisplayName("Period In Months")]
        [UIHint("_NumericInt")]
        public int PeriodInMonths { get; set; }

        [DisplayName("Term Amount")]
        [UIHint("Money")]
        public decimal NormalTermAmount { get; set; }
        public int ModelId { get; set; }

        public bool Default { get; set; }
    }

    public class ModelSpecificContractVMValidator:AbstractValidator<ModelSpecificContractVM>
    {
        public ModelSpecificContractVMValidator()
        {
            RuleFor(r => r.ContractTypeId).GreaterThan(0).WithMessage("Contract must be selected");
        }
    }
}