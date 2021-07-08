using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;
using ConsumerPoints.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsumerPoints.ServerLogic
{
    // In the DbOperations version of this service, a MS SQL Server database is interacted with.
    // The Db has two tables: Transactions and PayerPoints
    // When points are spent, points from the least recent transaction are spent first.
    // Record of transactions remains in the database even after being spent.  

    public class DbOperations : ITransactionStorage
    {
        private readonly ApplicationContext _context;

        public DbOperations(ApplicationContext context)
        {
            _context = context;
        }

        public string GetPayerBalances()
        {
            var payerBalances = (IEnumerable<PayerPoints>)_context.PayerPoints.Select(c => c);
            Dictionary<string, int> payerBalancesDict = new Dictionary<string, int>();
            foreach (var payerBalance in payerBalances)
                ServerHelper.InsertToDictionary(payerBalancesDict, payerBalance.Payer, payerBalance.Points);

            return JsonConvert.SerializeObject(payerBalancesDict);
        }


        public void AddTransaction(Transaction transaction)
        {
            var payer = _context.PayerPoints.Find(transaction.Payer);
            // Maintains consistency(ACID) within the database.
            if(!ValidTransactionPoints(payer, transaction)) 
                throw new Exception("Payer balances cannot be negative.");

            transaction.UnspentPoints = transaction.Points;
            _context.Transactions.Add(transaction);

            if (payer == null) _context.PayerPoints.Add( // If this payer will be new to the database
                 new PayerPoints
                 {
                     Payer=transaction.Payer,
                     Points=transaction.Points
                 });
            else  // If this payer is already in the database, just add points.
            {
                payer.Points += transaction.Points;
                _context.PayerPoints.Update(payer);
            }

            _context.SaveChanges();
        }


        public List<PayerPoints> SpendPoints(int withdrawal)
        {
            var totalPoints = GetTotalPoints(); 
            if (withdrawal == 0) return new List<PayerPoints>();
            if (withdrawal > totalPoints) throw new Exception("You do not have the points for that purchase.");

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();
            Transaction oldestUnspentTransaction;

            // Continue to consume points, from oldest to more recent transactions until demands of withdrawal are met. 
            while (withdrawal > 0)
            {
                // Find oldest transaction. Determine deduction from withdrawal, and use it to update... 
                // ...withdrawal, and unspent points of the given oldest transaction.
                oldestUnspentTransaction = GetOldestUnspentTrans();
                int deduction = GetDeduction(oldestUnspentTransaction, withdrawal);
                withdrawal -= deduction;
                oldestUnspentTransaction.UnspentPoints -= deduction;

                // This dict collects the information used for the HTTP Response.
                ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestUnspentTransaction.Payer, deduction * -1);

                //update both db tables based on the changed values in the code block above.
                var payerBalance = _context.PayerPoints.Find(oldestUnspentTransaction.Payer);
                payerBalance.Points -= deduction;
                _context.PayerPoints.Update(payerBalance);
                _context.Transactions.Update(oldestUnspentTransaction);
                _context.SaveChanges();
            }

            // Returns list of objects that describes the transactions consumed. 
            return pointsSpentByPayer.Values.ToList();
        }


        private int GetDeduction(Transaction oldestUnspentTransaction, int withdrawal)
        {
            int deduction = 0;
            // if the transaction points will be completely consumed
            if (withdrawal > oldestUnspentTransaction.UnspentPoints)
                deduction = oldestUnspentTransaction.UnspentPoints;
            else // if te transaction points will be partially consumed
                deduction = withdrawal;

            return deduction;
        }


        private Transaction GetOldestUnspentTrans()
        {
            return _context.Transactions.Where(u => u.UnspentPoints > 0)
                .OrderBy(c => c.Timestamp).First();
        }


        private int GetTotalPoints()
        {
            var pointValues = (IEnumerable<int>)_context.PayerPoints.Select(c => c.Points);
            var pointsTotal = pointValues.Aggregate(0,
                (current, next) =>
                    current + next);

            return pointsTotal;
        }

        // Helper function helping to enforce ACID database Consistency.
        // Returns true if transaction passed is valid. False if it would result in negative balance.
        private bool ValidTransactionPoints(PayerPoints payer, Transaction transaction)
        {
            if (payer != null && payer.Points + transaction.Points < 0
                || payer == null && transaction.Points < 0)
                return false;

            return true;
        }
    }
}
