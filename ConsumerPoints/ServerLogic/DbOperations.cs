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
        public DbOperations(ApplicationContext context)
        {
            _context = context;
        }

        public string GetPayerBalances()
        {
            Dictionary<string, int> balancesByPayer = new Dictionary<string, int>();
            var transactions = _context.Transactions.Select(c => new PayerPoints { Payer=c.Payer, Points=c.Points });
            foreach (var transaction in transactions)
                ServerHelper.InsertToDictionary(balancesByPayer, transaction.Payer, transaction.Points);

            return JsonConvert.SerializeObject(balancesByPayer);
        }


        public void AddTransaction(Transaction transaction)
        {
            _context.Add(transaction);
            _context.SaveChanges();
        }


        public List<PayerPoints> SpendPoints(int withdrawal)
        {
            var totalPoints = GetTotalPoints();
            if (withdrawal == 0) return new List<PayerPoints>();
            if (withdrawal > totalPoints) throw new Exception("You do not have the funds for that purchase.");

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();

            do
            {
                var oldestTransaction = _context.Transactions.Select(c => c).OrderBy(c => c.Timestamp).First();
                if (ServerHelper.TransactionConsumed(withdrawal, oldestTransaction))
                {
                    ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, oldestTransaction.Points * -1);
                    withdrawal -= oldestTransaction.Points;
                    _context.Transactions.Remove(oldestTransaction);
                }
                else
                {
                    oldestTransaction.Points -= withdrawal;
                    ServerHelper.InsertToDictionary(pointsSpentByPayer, oldestTransaction.Payer, withdrawal * -1);
                    withdrawal = 0;
                    _context.Update(oldestTransaction);
                }
                _context.SaveChanges();
            }
            while (withdrawal > 0);

            return pointsSpentByPayer.Values.ToList();
        }


        private int GetTotalPoints()
        {
            var pointValues = (IEnumerable<int>)_context.Transactions.Select(c => c.Points);
            var pointsTotal = pointValues.Aggregate(0,
                (current, next) =>
                    current + next);

            return pointsTotal;
        }
    }
}
