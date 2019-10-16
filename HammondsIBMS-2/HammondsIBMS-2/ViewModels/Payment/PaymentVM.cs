using System;
using System.ComponentModel;

namespace HammondsIBMS_2.ViewModels.Payment
{
    public class PaymentVM
    {
        [DisplayName("Payment No.")]
        public int PaymentId { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentSource  { get; set; }

        public decimal Amount { get; set; }

        public string Reference { get; set; }

    }
}