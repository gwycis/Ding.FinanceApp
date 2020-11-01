using System;

namespace FinanceApp.Library.Domain
{
    public sealed class TransactionReport
    {
        public DateTime Date { get; }
        public decimal Amount { get; }
        public decimal Balance { get; }

        public TransactionReport(DateTime date, decimal amount, decimal balance)
        {
            Date = date;
            Amount = amount;
            Balance = balance;
        }

        public override string ToString() => $"{nameof(Date)}: {Date}, {nameof(Amount)}: {Amount}, {nameof(Balance)}: {Balance}";
    }
}