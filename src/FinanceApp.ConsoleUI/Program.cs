using System;
using FinanceApp.Library;

namespace FinanceApp.ConsoleUI
{
    class Program
    {
        private const string DataFolder = "Data";
        private const string DataFile = "Transactions.json";

        static void Main(string[] args)
        {
            try
            {
                var store = new JsonBasedTransactionStore(DataFolder, DataFile);
                var printer = new ConsoleStatementPrinter();
                var accountService = new AccountService(store, printer);

                accountService.PrintStatement();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
