using System;
using System.Collections.Generic;
using System.Linq;
using FinanceApp.Library.Domain;
using FinanceApp.Library.Interfaces;

namespace FinanceApp.Library
{
    public sealed class AccountService : IAccountService
    {
        private readonly ITransactionStore _store;
        private readonly IStatementPrinter _printer;

        public AccountService(ITransactionStore store, IStatementPrinter printer)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _printer = printer ?? throw new ArgumentNullException(nameof(printer));
        }

        public void Deposit(Amount deposit)
        {
            var transaction = new Transaction(deposit, TransactionType.Deposit, DateTime.Now);
            _store.SaveTransaction(transaction);
            
        }

        public void Withdraw(Amount withdraw)
        {
            var transaction = new Transaction(withdraw, TransactionType.Withdrawal, DateTime.Now);
            _store.SaveTransaction(transaction);
        }

        public void PrintStatement()
        {
            var transactions = _store.GeTransactions();
            var balance = 0.0m;
            var reports = new List<TransactionReport>();

            foreach (var transaction in transactions.OrderBy(r => r.Date))
            {
                var amount = transaction.Amount.Value;

                if (transaction.Type == TransactionType.Withdrawal)
                    amount = amount * -1;
                
                balance = balance + amount;
                reports.Add(new TransactionReport(transaction.Date, amount, balance));
            }

            reports = reports.OrderByDescending(r => r.Date).ToList();

            _printer.Print(reports);
        }
    }
}