using ConsumerPoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Interfaces
{
    public interface ITransactionStorage
    {
        public List<PayerBalance> GetPayerBalances();

        public void AddTransactions(List<Transaction> transactions);

        public List<ExpenditureByPayer> SpendPoints(int pointToBeSpent);
        
    }
}
