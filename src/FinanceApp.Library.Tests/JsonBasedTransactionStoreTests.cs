using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Library.Domain;
using FinanceApp.Library.Tests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace FinanceApp.Library.Tests
{
    public sealed class JsonBasedTransactionStoreTests : IDisposable
    {
        public JsonBasedTransactionStoreTests()
        {
            DataFolderName = $"data-{Guid.NewGuid()}";
            DataFileName = $"transactions-{Guid.NewGuid()}.json";
            Sut = new JsonBasedTransactionStore(DataFolderName, DataFileName);

            if (Directory.Exists(DataFolder) == false)
                Directory.CreateDirectory(DataFolder);
        }

        private string DataFolderName { get; }
        private string DataFileName { get; }
        private string DataFolder => Path.Combine(Environment.CurrentDirectory, DataFolderName);
        private string DataFile => Path.Combine(DataFolder, DataFileName);

        private JsonBasedTransactionStore Sut { get; }

        [Fact]
        public void Given_File_When_NotFound_Then_ReturnEmpty()
        {
            // Arrange
            // Act
            var result = Sut.GeTransactions();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Given_File_When_TransactionsRetrieved_Then_ReturnAllTransactions()
        {
            // Arrange
            var content = @"[
  {
    ""Amount"": {
            ""Value"": 1000.00
        },
        ""Type"": ""Deposit"",
        ""Date"": ""2012-01-10""
    },
    {
    ""Amount"": {
        ""Value"": 2000.00
    },
    ""Type"": ""Deposit"",
    ""Date"": ""2012-01-13""
    },
    {
    ""Amount"": {
        ""Value"": 500
    },
    ""Type"": ""Withdrawal"",
    ""Date"": ""2012-01-14""
    }
    ]";
            await File.WriteAllTextAsync(DataFile, content);

            // Act
            var result = Sut.GeTransactions();
            
            // Assert
            Assert.Collection(result.OrderBy(r => r.Date), 
                t => t.AssertEqual((2012, 01, 10), TransactionType.Deposit, 1000m), 
                t => t.AssertEqual((2012, 01, 13), TransactionType.Deposit, 2000m), 
                t => t.AssertEqual((2012, 01, 14), TransactionType.Withdrawal, 500m));
        }

        [Fact]
        public async Task Given_NoFile_When_TransactionAdded_Then_SaveTransaction()
        {
            // Arrange
            // Act
            Sut.SaveTransaction(new Transaction(new Amount(1000m), TransactionType.Deposit, new DateTime(2012, 01, 10)));
            
            // Assert
            var content = await File.ReadAllTextAsync(DataFile);
            var result = JsonConvert.DeserializeObject<List<Transaction>>(content);

            // Assert
            Assert.Collection(result,
                t => t.AssertEqual((2012, 01, 10), TransactionType.Deposit, 1000m));
        }

        [Fact]
        public async Task Given_File_When_HasExistingTransactions_And_NewTransactionAdded_Then_SaveTransaction()
        {
            // Arrange
            var content = @"[
  {
    ""Amount"": {
            ""Value"": 1000.00
        },
        ""Type"": ""Deposit"",
        ""Date"": ""2012-01-10""
    },
    {
    ""Amount"": {
        ""Value"": 2000.00
    },
    ""Type"": ""Deposit"",
    ""Date"": ""2012-01-13""
    },
    {
    ""Amount"": {
        ""Value"": 500
    },
    ""Type"": ""Withdrawal"",
    ""Date"": ""2012-01-14""
    }
    ]";
            await File.WriteAllTextAsync(DataFile, content);

            // Act
            Sut.SaveTransaction(new Transaction(new Amount(5000m), TransactionType.Deposit, new DateTime(2012, 01, 20)));
            
            // Assert
            var updatedContent = await File.ReadAllTextAsync(DataFile);
            var result = JsonConvert.DeserializeObject<List<Transaction>>(updatedContent);

            // Assert
            Assert.Collection(result.OrderBy(r => r.Date),
                t => t.AssertEqual((2012, 01, 10), TransactionType.Deposit, 1000m),
                t => t.AssertEqual((2012, 01, 13), TransactionType.Deposit, 2000m),
                t => t.AssertEqual((2012, 01, 14), TransactionType.Withdrawal, 500m),
                t => t.AssertEqual((2012, 01, 20), TransactionType.Deposit, 5000m));
        }

        public void Dispose()
        {
            if (File.Exists(DataFile))
                File.Delete(DataFile);

            if (Directory.Exists(DataFolder))
                Directory.Delete(DataFolder, true);
        }
    }
}
