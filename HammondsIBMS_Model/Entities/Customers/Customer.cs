using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Payments;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Customers
{
    public class Customer
    {
        [DisplayName("Customer Number")]
        public int CustomerId { get; set; }

        [DisplayName("First Name")]
        public String FirstName { get; set; }

        [Required]
        public String Surname { get; set; }

        [DisplayName("Name")]
        public String SurnameFirstName
        {
            get { return Surname + ", " + FirstName; }
        }

        [DisplayName("Contact Info.")]
        [UIHint("_ContactInfo")]
        public  ContactInfo ContactInfo { get; set; }

        [UIHint("_Address")]
        public  Address Address { get; set; }

        [DisplayName("Bank Details")]
        [UIHint("_BankDetails")]
        public BankDetails BankDetails { get; set; }

        public virtual List<CustomerAccount> CustomerAccounts { get; set; }

        public int? CustomerEmployerId { get; set; }
        public virtual CustomerEmployer CustomerEmployer { get; set; }

        [UIHint("_PaymentSource")]
        [DisplayName("Payment Type")]
        public int PreferredPaymentSourceId { get; set; }

        [DisplayName("Preferred Payment")]
        [ForeignKey("PreferredPaymentSourceId")]
        public virtual PaymentSource PreferredPaymentSource { get; set; }


        public virtual List<CustomerDocument> Documents { get; set; }

        public virtual List<Payment> Payments { get; set; }

        public virtual List<OneOffItem> OneOfItems { get; set; }

        public Customer()
        {
            ContactInfo=new ContactInfo();
            Address=new Address();
            BankDetails=new BankDetails();
            //PreferredPaymentSourceId = 1;
            //CustomerEmployer=new CustomerEmployer();
        }

    }
}
