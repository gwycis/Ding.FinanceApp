using System.Collections.Generic;
using FinanceApp.Library.Domain;

namespace FinanceApp.Library
{
    public interface IStatementPrinter
    {
        void Print(IReadOnlyList<TransactionReport> reports);
    }
}