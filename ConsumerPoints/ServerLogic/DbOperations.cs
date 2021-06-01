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
            _context.Add(transaction);
            var payer = _context.PayerPoints.Find(transaction.Payer);
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
            var spendingMarker = _context.SpendingMarkers.Find(spendingMarkerId);
            Transaction oldestUnspentTransaction;

            while (withdrawal > 0)
            {
                oldestUnspentTransaction = GetOldestUnspentTrans(spendingMarker);

                int deduction = UpdateValues(spendingMarker, oldestUnspentTransaction, withdrawal);
                withdrawal -= deduction;
                spendingMarker.LastWasPartiallySpent = true;
                if (spendingMarker.Remainder == 0)
                    spendingMarker.LastWasPartiallySpent = false;
  
                ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestUnspentTransaction.Payer, deduction * -1);
                spendingMarker.LastSpentDate = oldestUnspentTransaction.Timestamp;

                var payerBalance = _context.PayerPoints.Find(oldestUnspentTransaction.Payer);
                payerBalance.Points -= deduction;
                _context.PayerPoints.Update(payerBalance);
            }

            _context.SpendingMarkers.Update(spendingMarker);
            _context.SaveChanges();

            return pointsSpentByPayer.Values.ToList();
        }


        private int UpdateValues(SpendingMarker spendingMarker, Transaction oldestUnspentTransaction, int withdrawal)
        {
            int deduction;
            if (spendingMarker.LastWasPartiallySpent)
                deduction = PartiallySpentLastTransaction(spendingMarker, oldestUnspentTransaction, withdrawal);
            else//last transaction was fully spent
                deduction = FullySpentLastTransaction(spendingMarker, oldestUnspentTransaction, withdrawal);

            return deduction;
        }


        private int PartiallySpentLastTransaction(SpendingMarker spendingMarker, Transaction oldestUnspentTransaction, int withdrawal)
        {
            if (withdrawal > spendingMarker.Remainder)
            {
                int deduction = withdrawal - spendingMarker.Remainder;
                spendingMarker.Remainder = 0;
                return deduction;
            }
            else
            {
                spendingMarker.Remainder -= withdrawal;
                return withdrawal;
            }
        }


        private int FullySpentLastTransaction(SpendingMarker spendingMarker, Transaction oldestUnspentTransaction, int withdrawal)
        {
            if (withdrawal > oldestUnspentTransaction.Points)
               return oldestUnspentTransaction.Points;
            else
            {
                spendingMarker.Remainder = oldestUnspentTransaction.Points - withdrawal;
                return withdrawal; 
            }
        }


        private Transaction GetOldestUnspentTrans(SpendingMarker spendingMarker)
        {
            return spendingMarker.LastWasPartiallySpent ?
                    _context.Transactions.Select(c => c).Where(c => c.Timestamp >= spendingMarker.LastSpentDate).First() :
                    _context.Transactions.Select(c => c).Where(c => c.Timestamp > spendingMarker.LastSpentDate).First();
        }


        private int GetTotalPoints()
        {
            var pointValues = (IEnumerable<int>)_context.PayerPoints.Select(c => c.Points);
            var pointsTotal = pointValues.Aggregate(0,
                (current, next) =>
                    current + next);

            return pointsTotal;
        }
    }
}
