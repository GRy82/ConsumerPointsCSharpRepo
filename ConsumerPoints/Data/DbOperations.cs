using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Data
{
    public class DbOperations : ITransactionStorage
    {
        private readonly ApplicationContext _context;
        public DbOperations(ApplicationContext context)
        {
            _context = context;
        }

        public List<PayerPoints> GetPayerBalances()
        {
            Dictionary<string, PayerPoints> balancesByPayer = new Dictionary<string, PayerPoints>();
            var transactions = _context.Transactions.Select(c => new PayerPoints { Payer=c.Payer, Points=c.Points });
            foreach (var transaction in transactions)
                InsertToDictionary(balancesByPayer, transaction.Payer, transaction.Points);

            return balancesByPayer.Values.ToList();
        }
        public void AddTransaction(Transaction transaction)
        {
            _context.Add(transaction);
            _context.SaveChanges();
        }

        public List<PayerPoints> SpendPoints(int withdrawal)
        {
            var totalPoints = _context.Transactions.Select(c => c.Points).Aggregate(0, (current, next) => + next);
            if (withdrawal == 0) return new List<PayerPoints>();
            if (withdrawal > totalPoints) throw new Exception("You do not have the funds for that purchase.");

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();

            do
            {
                var oldestTransaction = _context.Transactions.Select(c => c).Min();
                if (TransactionConsumed(withdrawal, oldestTransaction))
                {
                    InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, oldestTransaction.Points * -1);
                    withdrawal -= oldestTransaction.Points;
                    _context.Transactions.Remove(oldestTransaction);
                }
                else
                {
                    oldestTransaction.Points -= withdrawal;
                    InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, withdrawal * -1);
                    withdrawal = 0;
                    _context.Update(oldestTransaction);
                }
                _context.SaveChanges();
            }
            while (withdrawal < 0);

            return pointsSpentByPayer.Values.ToList();
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


        public int GetTotalPoints()
        {
            var pointValues = (IEnumerable<int>)_context.Transactions.Select(c => c.Points);
            var pointsTotal = pointValues.Aggregate(0,
                (current, next) =>
                    current + next);

            return pointsTotal;
        }

        private bool TransactionConsumed(int pointsNeeded, Transaction transaction)
        {
            return pointsNeeded > transaction.Points;
        }
    }
}
