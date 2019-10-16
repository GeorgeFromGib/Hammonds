using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.Basket
{
    [Validator(typeof(AcceptBasketValidation))]
    public class AcceptBasketVM
    {
        public int CustomerAccountId { get; set; }
        public bool FullPayment { get; set; }

        [DisplayName("Amount to pay")]
        [DataType(DataType.Currency)]
        public decimal AmountPayed { get; set; }

        [DisplayName("Payment Received")]
        public bool PaymentReceived { get; set; }

        [DisplayName("Accept Basket")]
        public bool AcceptContract { get; set; }
    }

    public class AcceptBasketValidation : AbstractValidator<AcceptBasketVM>
    {
        public AcceptBasketValidation()
        {
            RuleFor(m => m.AcceptContract).Must(c => c).WithMessage("Confirm Acceptance before completing!");
            RuleFor(m => m.PaymentReceived)
                .Must(c => c)
                .When(w => w.FullPayment)
                .WithMessage("Confirm Payment Received");
        }
    }
}