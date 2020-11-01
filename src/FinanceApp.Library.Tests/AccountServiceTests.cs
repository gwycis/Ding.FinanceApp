using System;
using System.Collections.Generic;
using FinanceApp.Library.Domain;
using FinanceApp.Library.Interfaces;
using FinanceApp.Library.Tests.Helpers;
using NSubstitute;
using Xunit;

namespace FinanceApp.Library.Tests
{
    public class AccountServiceTests
    {
        public AccountServiceTests()
        {
            Store = Substitute.For<ITransactionStore>();
            Printer = Substitute.For<IStatementPrinter>();
            Sut = new AccountService(Store, Printer);
        }

        private ITransactionStore Store { get; }
        private IStatementPrinter Printer { get; }

        private AccountService Sut { get; }

        [Fact]
        public void Given_Deposit_When_Saved_Then_PassTransactionDetailsToStore()
        {
            // Arrange
            // Act
            Sut.Deposit(new Amount(2000m));

            // Assert
            Store.Received(1).SaveTransaction(Arg.Is<Transaction>(t =>
                t.Amount.Value == 2000m && t.Type == TransactionType.Deposit));
        }

        [Fact]
        public void Given_Withdrawal_When_Saved_Then_PassTransactionDetailsToStore()
        {
            // Arrange
            // Act
            Sut.Withdraw(new Amount(1000m));

            // Assert
            Store.Received(1).SaveTransaction(Arg.Is<Transaction>(t =>
                t.Amount.Value == 1000m && t.Type == TransactionType.Withdrawal));
        }

        [Fact]
        public void Given_ExistingTransactions_When_Printed_Then_AggregateOnDailyBasis_And_Print()
        {
            // Arrange
            Store.GeTransactions().Returns(new List<Transaction>
            {
                new Transaction(new Amount(1000m), TransactionType.Deposit, new DateTime(2012, 01, 10)),
                new Transaction(new Amount(2000m), TransactionType.Deposit, new DateTime(2012, 01, 13)),
                new Transaction(new Amount(500m), TransactionType.Withdrawal, new DateTime(2012, 01, 14))
            });

            List<TransactionReport> result = null;
            
            Printer.Print(Arg.Do<List<TransactionReport>>(r => result = r));

            // Act
            Sut.PrintStatement();

            // Assert
            Assert.Collection(result,
                r => r.AssertEqual((2012, 01, 14), -500, 2500),
                r => r.AssertEqual((2012, 01, 13), 2000, 3000),
                r => r.AssertEqual((2012, 01, 10), 1000, 1000));
        }

        [Fact]
        public void Given_NoTransaction_When_Printed_Then_PrintEmpty()
        {
            // Arrange
            Store.GeTransactions().Returns(new List<Transaction>());

            // Act
            Sut.PrintStatement();

            // Assert
            Printer.Received(1).Print(Arg.Is<List<TransactionReport>>(r => r.Count == 0));
        }
    }
}