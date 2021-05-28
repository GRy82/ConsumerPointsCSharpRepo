using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;

namespace ConsumerPoints.Data
{
    public class LocalMemOperations : ITransactionStorage
    {
        public List<PayerBalance> GetPayerBalances() {
            return new List<PayerBalance>();
        }

        public void AddTransactions(List<Transaction> transactions)
        {

        }

        public List<ExpenditureByPayer> SpendPoints(int pointToBeSpent)
        {
            return new List<ExpenditureByPayer>();
        }
    }
}
