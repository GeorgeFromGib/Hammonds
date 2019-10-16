using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_Domain.Entities.Customers
{
    [Validator(typeof(CustomerEmployerValidator))]
    public class CustomerEmployer
    {
        public int CustomerEmployerId { get; set; }

        public int CustomerId { get; set; }

        [UIHint("_Employer")]
        [DisplayName("Employer")]
        public int EmployerId { get; set; }
        public virtual Employer Employer { get; set; }
        
        [DisplayName("Contact Person")]
        public string  ContactPerson { get; set; }

        [DisplayName("Contact Notes")]
        public string ContactNotes{ get; set; }

    }

    public class CustomerEmployerValidator:AbstractValidator<CustomerEmployer>
    {
        public CustomerEmployerValidator()
        {
            RuleFor(f => f.ContactPerson).NotEmpty().When(g => g.EmployerId > -1);
        }
    }
}