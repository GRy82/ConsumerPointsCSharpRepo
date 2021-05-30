using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.CustomDataStructures;
using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;

namespace ConsumerPoints.Data
{
    public class LocalMemOperations : ITransactionStorage
    {

        public TransactionQueue storedTransactions = new TransactionQueue();

        public List<PayerBalance> GetPayerBalances()
        {
            Dictionary<string, int> pointBalanceByPayer = new Dictionary<string, int>();
            //foreach (var transaction in storedTransactions)
            //{
               
            //}


            return new List<PayerBalance>();
        }

        public void AddTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
                storedTransactions.Enqueue(transaction);
        }

        public List<ExpenditureByPayer> SpendPoints(int pointsToBeSpent)
        {
            return new List<ExpenditureByPayer>();
        }
    }
}
