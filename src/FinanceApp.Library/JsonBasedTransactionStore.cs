using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinanceApp.Library.Domain;
using FinanceApp.Library.Interfaces;
using Newtonsoft.Json;

namespace FinanceApp.Library
{
    public class JsonBasedTransactionStore : ITransactionStore
    {
        private readonly string _dataFolderName;
        private readonly string _dataFileName;

        private string DataFolder => Path.Combine(Environment.CurrentDirectory, _dataFolderName);
        private string DataFile => Path.Combine(DataFolder, _dataFileName);

        public JsonBasedTransactionStore(string dataFolderName, string dataFileName)
        {
            if (string.IsNullOrWhiteSpace(dataFolderName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dataFolderName));
            if (string.IsNullOrWhiteSpace(dataFileName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dataFileName));
            _dataFolderName = dataFolderName;
            _dataFileName = dataFileName;
        }

        public IEnumerable<Transaction> GeTransactions()
        {
            if (File.Exists(DataFile) == false)
                return new List<Transaction>();

            var content = File.ReadAllText(DataFile);
            return JsonConvert.DeserializeObject<List<Transaction>>(content);
        }

        public void SaveTransaction(Transaction transaction)
        {
            var existingTransactions = GeTransactions().ToList();
            
            existingTransactions.Add(transaction);

            var transactions = JsonConvert.SerializeObject(existingTransactions, Formatting.Indented);

            if (Directory.Exists(DataFolder) == false)
                Directory.CreateDirectory(DataFolder);

            File.WriteAllText(DataFile, transactions);
        }
    }
}