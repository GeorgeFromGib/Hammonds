using System.ComponentModel;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    [Validator(typeof(AcceptAccountValidation))]
    public class AcceptAccountVM
    {
        public int CustomerAccountId { get; set; }
        public bool FullPayment { get; set; }

        [DisplayName("Accept Account")]
        public bool AcceptContract { get;  set; }
    }

    public class AcceptAccountValidation:AbstractValidator<AcceptAccountVM>
    {
        public AcceptAccountValidation()
        {
            RuleFor(m => m.AcceptContract).Must(c => c).WithMessage("Confirm Acceptance before completing!");
        }
    }
}