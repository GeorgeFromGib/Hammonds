using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.Customer
{
    public class CustomerBankPaymentDetailsVM
    {
        public int CustomerId { get; set; }

        [UIHint("_PaymentSource")]
        [DisplayName("Payment Type")]
        public int PreferredPaymentSourceId { get; set; }

        [DisplayName("Bank Details")]
        [UIHint("_BankDetails")]
        public BankDetails BankDetails { get; set; }
    }
}