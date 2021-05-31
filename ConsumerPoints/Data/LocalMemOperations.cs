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
        List<Transaction> transactions = new List<Transaction>();

        public List<PayerBalance> GetPayerBalances()
        {
            Dictionary<string, PayerBalance> pointBalanceByPayer = new Dictionary<string, PayerBalance>();
            foreach (var transaction in storedTransactions.ToList())
            {
                if (pointBalanceByPayer.ContainsKey(transaction.Payer))
                    pointBalanceByPayer[transaction.Payer].Points += transaction.Points;
                else
                    pointBalanceByPayer.Add(transaction.Payer, new PayerBalance {
                        Payer=transaction.Payer, Points=transaction.Points
                    });
            }

            return pointBalanceByPayer.Values.ToList();
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
