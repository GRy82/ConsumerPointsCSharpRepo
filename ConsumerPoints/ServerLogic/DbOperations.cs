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
            return JsonConvert.SerializeObject(_context.PayerPoints
                .Select(c => c)
                .OrderByDescending(c => c.Points));
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
            if (withdrawal > totalPoints) throw new Exception("You do not have the funds for that purchase.");

            Dictionary<string, PayerPoints> pointsSpentByPayer = new Dictionary<string, PayerPoints>();

            var oldestTransactions = _context.Transactions
                .Select(c => c).Where(c => c.Points > 0)
                .OrderBy(c => c.Timestamp).ToList();
            do
            {
          

                if (withdrawal > )
                
                _context.SaveChanges();
            }
            while (withdrawal > 0);

            return pointsSpentByPayer.Values.ToList();
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
