using System;
using FinanceApp.Library.Domain;
using Xunit;

namespace FinanceApp.Library.Tests.Helpers
{
    public static class TransactionExtensions
    {
        public static void AssertEqual(this Transaction transaction, (int Year, int Month, int Day) date,
            TransactionType type, decimal amount)
        {
            Assert.Equal(new DateTime(date.Year, date.Month, date.Day), transaction.Date);
            Assert.Equal(type, transaction.Type);
            Assert.Equal(amount, transaction.Amount.Value);
        }
    }
}