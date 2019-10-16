using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_Domain.Values
{
    [Validator(typeof(BankDetailsValidator))]
    public class BankDetails
    {
        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [DisplayName("Account Holder")]
        public string AccountHolder { get; set; }

        [DisplayName("Sort Code")]
        [UIHint("_SortCode")]
        public string SortCode { get; set; }

        [DisplayName("Account Number")]
        public string AccountNo { get; set; }

    }

    public class BankDetailsValidator:AbstractValidator<BankDetails>
    {
        public BankDetailsValidator()
        {
            RuleFor(x => x.SortCode).Matches(@"^(\d){2}-(\d){2}-(\d){2}$").WithMessage("Invalid Sort Code").NotEmpty().When(c => !string.IsNullOrEmpty(c.BankName));
            RuleFor(x => x.AccountNo).Matches(@"^(\d){7,8}$").WithMessage("Invalid Account Code").NotEmpty().When(c => !string.IsNullOrEmpty(c.BankName));
            RuleFor(x => x.BankName).NotEmpty().When(c => !string.IsNullOrEmpty(c.AccountHolder));
            RuleFor(x => x.AccountHolder).NotEmpty().When(c => !string.IsNullOrEmpty(c.BankName));

        }
    }
}
