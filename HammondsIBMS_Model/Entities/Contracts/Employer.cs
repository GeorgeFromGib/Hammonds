using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public class Employer
    {
        public int EmployerId { get; set; }
        //public int CustomerId { get; set; }
        //public virtual Customer Customer { get; set; }

        [DisplayName("Employer Name")]
        [Required]
        public string EmployerName { get; set; }


        public Address Address { get; set; }

        [DisplayName("Contact Info.")]
        [UIHint("_ContactInfo")]
        public ContactInfo ContactInfo { get; set; }

        public Employer()
        {
            Address=new Address();
            ContactInfo=new ContactInfo();
        }
    }
}
