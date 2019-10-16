using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    public class NewAccountVM
    {
        public int CustomerId { get; set; }

        [DisplayName("Account Type")]
        public AccountType AccountType { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public CustomerAccount Account { get; set; }

        [DisplayName("Instructions")]
        [DataType(DataType.MultilineText)]
        public string AlternateAddressInstructions { get; set; }

        [UIHint("_Address")]
        public Address AlternateAddress { get; set; }
    }
}