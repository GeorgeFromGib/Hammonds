using System;
using System.Collections.Generic;
using System.Linq;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Payments;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class PaymentService:ServiceBase
    {
        public class RolledTransaction
        {
            public int CustomerAccountId { get; set; }
            public string AccountType { get; set; }
            public BillCycle BillCycle { get; set; }
            public decimal Amount { get; set; }
        }

        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IUnitOfWork unitOfWork,ITransactionRepository transactionRepository,IPaymentRepository paymentRepository) : base(unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _paymentRepository = paymentRepository;
        }

        public IEnumerable<RolledTransaction> GetRolledTransactions(int customerId)
        {
            var trans = _transactionRepository.GetMany(t => t.CustomerAccount.CustomerId == customerId);
            var rolledtrans =
                trans.GroupBy(g => new {g.CustomerAccountId,g.BillCycle.BillCycleNo}).Select(
                    c => new RolledTransaction {Amount = c.Sum(p => p.Amount), CustomerAccountId = c.Key.CustomerAccountId,BillCycle = new BillCycle(c.Key.BillCycleNo),AccountType = c.Max(p=>p.CustomerAccount.AccountType.ToString())});
            return rolledtrans;
        }

        public void AddPayment(Payment pymnt)
        {
            _paymentRepository.Add(pymnt);

            foreach (var pItem in pymnt.PaymentItems)
            {
                var trans = new Transaction
                                {
                                    Amount = -pItem.Amount,
                                    BillCycle = pItem.BillCycle,
                                    //CustomerAccountId = pItem.CustomerAccountId,
                                    DateTime = DateTime.Today,
                                    TransactionTypeId = (int) PaymentItemTypeToTransactionTypeConvertor.GetTransactionType(pItem.PaymentItemTypeId)
                                };
                _transactionRepository.Add(trans);
            }
        }

        public IEnumerable<Payment> GetPaymentsForCustomer(int id)
        {
            return _paymentRepository.GetMany(p => p.CustomerId == id);
        }

        public IEnumerable<PaymentItem> GetPaymentItemsForPayment(int id)
        {
            return _paymentRepository.GetById(id).PaymentItems;
        }
    }
}