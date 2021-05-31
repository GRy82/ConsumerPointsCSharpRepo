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

        public List<PayerPoints> GetPayerBalances()
        {
            Dictionary<string, PayerPoints> pointBalanceByPayer = new Dictionary<string, PayerPoints>();
            foreach (var transaction in storedTransactions.ToList())
                InsertToDictionary(pointBalanceByPayer, transaction.Payer, transaction.Points);

            return pointBalanceByPayer.Values.ToList();
        }

        public void AddTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
                storedTransactions.Enqueue(transaction);
        }

        public List<PayerPoints> SpendPoints(int withdrawal)
        {
            int totalPoints = GetTotalPoints();
            if (withdrawal > totalPoints || withdrawal == 0) return new List<PayerPoints>(); ;

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();
            
            do
            {
                Transaction oldestTransaction;
                oldestTransaction = storedTransactions.Dequeue();

                if (TransactionConsumed(withdrawal, oldestTransaction))
                {
                    InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, oldestTransaction.Points * -1);
                    withdrawal -= oldestTransaction.Points;
                }
                else
                {
                    oldestTransaction.Points -= withdrawal;
                    storedTransactions.Enqueue(oldestTransaction);
                    InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, withdrawal * -1);
                    withdrawal = 0;
                }
            } while (withdrawal > 0);

            return pointsSpentByPayer.Values.ToList();
        }

        public int GetTotalPoints()
        {
            return GetPayerBalances().Aggregate(0,
                (current, next) => 
                    current += next.Points);
        }

        private bool TransactionConsumed(int pointsNeeded, Transaction transaction)
        {
            return pointsNeeded > transaction.Points;
        }

        private void InsertToDictionary(Dictionary<string, PayerPoints> payerPoints, string Payer, int Points)
        {
            if (payerPoints.ContainsKey(Payer))
                payerPoints[Payer].Points += Points;
            else
                payerPoints.Add(Payer, new PayerPoints
                {
                    Payer = Payer,
                    Points = Points
                });
        }
        
    }
}
