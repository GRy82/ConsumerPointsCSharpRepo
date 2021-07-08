using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.CustomDataStructures;
using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ConsumerPoints.ServerLogic
{
    // In the LocalMemOperations version of this service, a priority queue is built on a min heap and is
    // used as the underlying data structure.  Transactions are not documented for documentation's sake.
    // They are only stored, and then used and removed as needed, with the oldest points being spent first.  
    public class LocalMemOperations : ITransactionStorage
    {

        public TransactionQueue storedTransactions = new TransactionQueue();


        public string GetPayerBalances()
        {
            Dictionary<string, int> pointBalanceByPayer = new Dictionary<string, int>();
            foreach (var transaction in storedTransactions.ToList())
                ServerHelper.InsertToDictionary(pointBalanceByPayer, transaction.Payer, transaction.Points);

            return JsonConvert.SerializeObject(pointBalanceByPayer);
        }


        public List<PayerPoints> GetPayerBalanceList()
        {
            Dictionary<string, PayerPoints> pointBalanceByPayer = new Dictionary<string, PayerPoints>();
            foreach (var transaction in storedTransactions.ToList())
                ServerHelper.InsertToDictionary(pointBalanceByPayer, transaction.Payer, transaction.Points);
            //Returns a list of objects, each representing a payer, and the total points per payer.
            return pointBalanceByPayer.Values.ToList();
        }


        public void AddTransaction(Transaction transaction)
        {
            storedTransactions.Enqueue(transaction);
        }


        public List<PayerPoints> SpendPoints(int withdrawal)
        {
            // edge case 1: withdrawal attempted is moot. 
            if (withdrawal == 0) return new List<PayerPoints>();
            //edge case 2: withdrawal exceeds funds.
            int totalPoints = GetTotalPoints();
            if (withdrawal > totalPoints) throw new Exception("You do not have the funds for that purchase.");

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();
            
            // Proceeds to consume transactions, least recent to most recent, until demands of the withdrawal are met.  
            do
            {
                Transaction oldestTransaction = storedTransactions.Dequeue();

                // Processes the transaction in the following manner if all points will be consumed.
                if (ServerHelper.TransactionConsumed(withdrawal, oldestTransaction))
                {
                    ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, oldestTransaction.Points * -1);
                    withdrawal -= oldestTransaction.Points;
                }
                else // Processes the transaction in the following manner if it will only be partially consumed.
                {
                    oldestTransaction.Points -= withdrawal;
                    storedTransactions.Enqueue(oldestTransaction);
                    ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, withdrawal * -1);
                    withdrawal = 0;
                }
            } while (withdrawal > 0);

            // Returns list of objects that describes the transactions consumed.  
            return pointsSpentByPayer.Values.ToList();
        }


        private int GetTotalPoints()
        {
            var payerPoints = GetPayerBalanceList();
            var totalPoints = payerPoints.Aggregate(0,
                (current, next) => 
                    current + next.Points);
            return totalPoints;
        }
    }
}
