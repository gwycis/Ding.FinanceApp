using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Library;
using FinanceApp.Library.Domain;

namespace FinanceApp.ConsoleUI
{
    public sealed class ConsoleStatementPrinter : IStatementPrinter
    {  
        private const int Size = 14;

        public void Print(IReadOnlyList<TransactionReport> reports)
        {
            if (reports == null) throw new ArgumentNullException(nameof(reports));
            
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("     Date     ||    Amount    ||    Balance   ");
            sb.AppendLine("----------------------------------------------");

            if (reports.Count == 0)
                sb.AppendLine(" *** No transactions found ***");
            else
            {
                foreach (var report in reports)
                {
                    var date = report.Date.ToString("dd/MM/yyyy").Pad(Size);
                    var amount = report.Amount.ToString("N2").Pad(Size);
                    var balance = report.Balance.ToString("N2").Pad(Size);

                    sb.AppendLine($"{date}||{amount}||{balance}");
                }
            }

            Console.WriteLine(sb.ToString());
        }
    }
}