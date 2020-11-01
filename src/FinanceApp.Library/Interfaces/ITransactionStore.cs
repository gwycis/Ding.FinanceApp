using System.Collections.Generic;
using FinanceApp.Library.Domain;

namespace FinanceApp.Library.Interfaces
{
    public interface ITransactionStore
    {
        IEnumerable<Transaction> GeTransactions();
        void SaveTransaction(Transaction transaction);
    }
}