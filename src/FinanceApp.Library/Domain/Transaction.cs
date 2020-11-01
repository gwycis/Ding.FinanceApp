using System;
using System.ComponentModel;

namespace FinanceApp.Library.Domain
{
    public class Transaction
    {
        public Amount Amount { get; }
        public TransactionType Type { get; }
        public DateTime Date { get; }

        public Transaction(Amount amount, TransactionType type, DateTime date)
        {
            if (!Enum.IsDefined(typeof(TransactionType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type,
                    typeof(TransactionType));
            Amount = amount ?? throw new ArgumentNullException(nameof(amount));
            Type = type;
            Date = date;
        }
    }
}