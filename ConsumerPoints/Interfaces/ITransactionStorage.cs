using ConsumerPoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Interfaces
{
    public interface ITransactionStorage
    {
        //public List<Transaction> GetTransactions();
        public string GetPayerBalances();

        public void AddTransaction(Transaction transaction);

        public List<PayerPoints> SpendPoints(int pointToBeSpent);
        
    }
}
