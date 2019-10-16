using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Accounts;

using HammondsIBMS_Domain.Entities.Payments;

namespace HammondsIBMS_Domain.Entities.Invoicing
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }


        public int CustomerAccountId { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; }

        public BillCycle BillCycle { get; set; }

        public int TransactionTypeId { get; set; }
        public virtual TransactionType EntryType { get; set; }
    }

    public class TransactionType
    {
        public int TransactionTypeId { get; set; }
        public string Description { get; set; }
    }

    public enum TransactionTypes
    {
        None=0,
        Invoice = 1,
        Adjustment = 2,
        Payment = 3,
        CreditPayment=4
    }

    public static class PaymentItemTypeToTransactionTypeConvertor
    {
        static readonly Dictionary<PaymentItemTypes, TransactionTypes> _transTypesList = new Dictionary<PaymentItemTypes,TransactionTypes>()
                                                                                             {
                                                                                                 {PaymentItemTypes.InvoiceSettlement ,TransactionTypes.Payment}, 
                                                                                                 {PaymentItemTypes.Credit, TransactionTypes.CreditPayment}
                                                                                             };

        public static TransactionTypes GetTransactionType(PaymentItemTypes paymentItemTypes)
        {
            return _transTypesList[paymentItemTypes];
        }

        public static TransactionTypes GetTransactionType(int paymentItemIndex)
        {
            return _transTypesList[(PaymentItemTypes)Enum.ToObject(typeof(PaymentItemTypes), paymentItemIndex)];
        }

    }
}