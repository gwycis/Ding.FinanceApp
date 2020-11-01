using System;
using FinanceApp.Library.Domain;
using Xunit;

namespace FinanceApp.Library.Tests.Helpers
{
    public static class TransactionReportExtensions
    {
        public static void AssertEqual(this TransactionReport report, (int Year, int Month, int Day) date,
            decimal amount, decimal balance)
        {
            Assert.Equal(new DateTime(date.Year, date.Month, date.Day), report.Date);
            Assert.Equal(amount, report.Amount);
            Assert.Equal(balance, report.Balance);
        }
    }
}