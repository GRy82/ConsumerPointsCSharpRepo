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
    public class DbOperations : ITransactionStorage
    {
        private readonly ApplicationContext _context;
        const int spendingMarkerId = 1;
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
            if(!ValidTransactionPoints(payer, transaction)) 
                throw new Exception("Payer balances cannot be negative.");

            transaction.UnspentPoints = transaction.Points;
            _context.Transactions.Add(transaction);

            if (payer == null) _context.PayerPoints.Add(
                 new PayerPoints
                 {
                     Payer=transaction.Payer,
                     Points=transaction.Points
                 });
            else
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

            while (withdrawal > 0)
            {
                oldestUnspentTransaction = GetOldestUnspentTrans();
                int deduction = GetDeduction(oldestUnspentTransaction, withdrawal);
                withdrawal -= deduction;
                oldestUnspentTransaction.UnspentPoints -= deduction;
  
                ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestUnspentTransaction.Payer, deduction * -1);

                var payerBalance = _context.PayerPoints.Find(oldestUnspentTransaction.Payer);
                payerBalance.Points -= deduction;
                _context.PayerPoints.Update(payerBalance);
                _context.Transactions.Update(oldestUnspentTransaction);
            }

            _context.SaveChanges();
            return pointsSpentByPayer.Values.ToList();
        }


        private int GetDeduction(Transaction oldestUnspentTransaction, int withdrawal)
        {
            int deduction = 0;
            if (withdrawal > oldestUnspentTransaction.UnspentPoints)
                deduction = oldestUnspentTransaction.UnspentPoints;
            else
                deduction = withdrawal;

            return deduction;
        }


        private Transaction GetOldestUnspentTrans()
        {
            return _context.Transactions.Select(c => c)
                                        .Where(c => c.UnspentPoints > 0)
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

        private bool ValidTransactionPoints(PayerPoints payer, Transaction transaction)
        {
            if (payer != null && payer.Points + transaction.Points < 0
                || payer == null && transaction.Points < 0)
                return false;

            return true;
        }
    }
}
