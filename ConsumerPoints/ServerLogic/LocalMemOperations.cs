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
    public class LocalMemOperations : ITransactionStorage
    {

        public TransactionQueue storedTransactions = new TransactionQueue();

        //public LocalMemOperations()
        //{
        //    AddTransaction( new Transaction
        //    {
        //        Payer = "CVS Pharmacy",
        //        Points = 400,
        //        Timestamp = new DateTime(2021, 02, 04)
        //    });
        //    AddTransaction(new Transaction
        //    {
        //        Payer = "Wegman's",
        //        Points = 300,
        //        Timestamp = new DateTime(2021, 03, 04)
        //    });
        //    AddTransaction( new Transaction
        //    {
        //        Payer = "CVS Pharmacy",
        //        Points = 300,
        //        Timestamp = new DateTime(2021, 04, 04)
        //    });
      
        //}


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

            return pointBalanceByPayer.Values.ToList();
        }


        public void AddTransaction(Transaction transaction)
        {
            storedTransactions.Enqueue(transaction);
        }


        public List<PayerPoints> SpendPoints(int withdrawal)
        {
            int totalPoints = GetTotalPoints();
            if (withdrawal == 0) return new List<PayerPoints>();
            if (withdrawal > totalPoints) throw new Exception("You do not have the funds for that purchase.");

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();
            
            do
            {
                Transaction oldestTransaction;
                oldestTransaction = storedTransactions.Dequeue();

                if (ServerHelper.TransactionConsumed(withdrawal, oldestTransaction))
                {
                    ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, oldestTransaction.Points * -1);
                    withdrawal -= oldestTransaction.Points;
                }
                else
                {
                    oldestTransaction.Points -= withdrawal;
                    storedTransactions.Enqueue(oldestTransaction);
                    ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, withdrawal * -1);
                    withdrawal = 0;
                }
            } while (withdrawal > 0);

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
