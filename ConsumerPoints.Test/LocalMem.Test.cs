using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsumerPoints.Models;
using ConsumerPoints.CustomDataStructures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ConsumerPoints.ServerLogic;

namespace ConsumerPoints.Test
{
    [TestClass]
    public class LocalMemTests
    {
        [TestMethod]
        public void GetPayerBalance_TwoPayerPoints_ReturnJsonString()
        {
            LocalMemOperations localMemOperations = ArrangeTransactions();
            var payerBalances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(localMemOperations.storedTransactions.ToList().Count, 3);
            Assert.AreEqual(payerBalances, "{\"CVS Pharmacy\":700,\"Wegman's\":300}");
        }

        [TestMethod]
        public void AddTransaction_ThreeTransactions_StoredInLocal()
        {
            LocalMemOperations localMemOperations = ArrangeTransactions();
            
            Assert.IsFalse(localMemOperations.storedTransactions.IsEmpty());
        }


        [TestMethod]
        public void SpendPoints_ConsumeOneTransaction_TransactionRemoved()
        {
            LocalMemOperations localMemOperations = ArrangeTransactions();

            List<PayerPoints> expenditure = localMemOperations.SpendPoints(400);
            var balances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(balances, "{\"CVS Pharmacy\":300,\"Wegman's\":300}");
            Assert.AreEqual(expenditure.Count, 1);
            Assert.AreEqual(expenditure[0].Points, -400);
            Assert.AreEqual(expenditure[0].Payer, "CVS Pharmacy");
        }


        [TestMethod]
        public void SpendPoints_ConsumeTwoTransactions_TransactionsRemoved()
        {
            LocalMemOperations localMemOperations = ArrangeTransactions();
            var expenditure = localMemOperations.SpendPoints(700);
            var balances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(balances, "{\"Wegman's\":0,\"CVS Pharmacy\":300}");
            Assert.AreEqual(expenditure.Count, 2);
            Assert.AreEqual(expenditure[0].Points, -400);
            Assert.AreEqual(expenditure[0].Payer, "CVS Pharmacy");
            Assert.AreEqual(expenditure[1].Points, -300);
            Assert.AreEqual(expenditure[1].Payer, "Wegman's");

        }

        [TestMethod]
        public void SpendPoints_HalfConsumedTransaction_PayerPointsDeducted()
        {
            LocalMemOperations localMemOperations = ArrangeTransactions();
            var expenditure = localMemOperations.SpendPoints(500);
            var balances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(balances, "{\"Wegman's\":200,\"CVS Pharmacy\":300}");
            Assert.AreEqual(expenditure.Count, 2);
            Assert.AreEqual(expenditure[0].Points, -400);
            Assert.AreEqual(expenditure[0].Payer, "CVS Pharmacy");
            Assert.AreEqual(expenditure[1].Points, -100);
            Assert.AreEqual(expenditure[1].Payer, "Wegman's");

        }

        public LocalMemOperations ArrangeTransactions()
        {
            var T1 = new Transaction {
                Payer="CVS Pharmacy",
                Points=400,
                Timestamp=new DateTime(2021, 02, 04)
            };
            var T2 = new Transaction {
                Payer = "Wegman's",
                Points = 300,
                Timestamp = new DateTime(2021, 03, 04)
            };
            var T3 = new Transaction {
                Payer = "CVS Pharmacy",
                Points = 300,
                Timestamp = new DateTime(2021, 04, 04)
            };

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransaction(T1);
            localMemOperations.AddTransaction(T2);
            localMemOperations.AddTransaction(T3);

            return localMemOperations;
        }
    }
}
