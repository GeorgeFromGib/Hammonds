using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_Domain.Entities.Misc
{
    [Validator(typeof(MiscValidator))]
    public class Misc
    {
        public int MiscId { get; set; }
        public string Identifier { get; set; }
        public string Value { get; set; }
        public string ValueRegexFilter { get; set; }
        public string FormatExample { get; set; }
        public bool CanEdit { get; set; }
    }

    public class MiscValidator:AbstractValidator<Misc>
    {
        public MiscValidator()
        {
            RuleFor(v => v.Value).NotEmpty();
            RuleFor(v => v.Value).Must((misc, value) => Regex.IsMatch(value, misc.ValueRegexFilter)).WithMessage("Invalid value format (see example)");
        }
    }

    public enum MiscIdentifier
    {
        DefaultDutyPercentage = 1,
        LastBillCycle = 2,
        MinPeriodForBillCycles=3,
        ContractExpiryWarningPeriod=4,
        DefaultDepositPerUnit=5,
        LastHouseKeep=1000
    }
}
