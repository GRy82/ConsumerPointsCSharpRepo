using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsumerPoints.Models;
using ConsumerPoints.CustomDataStructures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ConsumerPoints.Data;

namespace ConsumerPoints.Test
{
    [TestClass]
    public class MinHeapTests
    {
        [TestMethod]
        public void Test()
        {
            List<Transaction> transactions = new List<Transaction> {
                new Transaction{
                    Payer="CVS Pharmacy",
                    Points=400,
                    Timestamp=new DateTime(2021, 03, 04)
                },
                new Transaction{
                    Payer="Wegman's",
                    Points=300,
                    Timestamp=new DateTime(2021, 05, 04)
                }
            };

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            var firstDequeue = localMemOperations.storedTransactions.Dequeue();

            Assert.AreEqual(firstDequeue, transactions[0]);
            Assert.AreEqual(firstDequeue.Payer, transactions[0].Payer);
            Assert.AreEqual(localMemOperations.storedTransactions.Dequeue(), transactions[1]);
            Assert.IsTrue(localMemOperations.storedTransactions.IsEmpty());
        }

        [TestMethod]
        public void GetPayerBalance_HappyPath_ReturnList()
        {
            List<Transaction> transactions = ArrangeTransactions();

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            int totalPoints = localMemOperations.GetTotalPoints();
            var payerBalances = localMemOperations.GetPayerBalances();
            Assert.AreEqual(totalPoints, 1000);
            Assert.AreEqual(localMemOperations.storedTransactions.ToList().Count, 3);
            Assert.AreEqual(payerBalances[0].Points, 700);
            Assert.AreEqual(payerBalances[0].Payer, "CVS Pharmacy");
            Assert.AreEqual(payerBalances[1].Payer, "Wegman's");

        }


        [TestMethod]
        public void SpendPoints_ConsumeOneTransaction_TransactionRemoved()
        {
            List<Transaction> transactions = ArrangeTransactions();

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            List<PayerPoints> expenditure = localMemOperations.SpendPoints(400);
            List<PayerPoints> balances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(balances[0].Points, 300);
            Assert.AreEqual(balances[1].Points, 300);
            Assert.AreEqual(expenditure.Count, 1);
            Assert.AreEqual(expenditure[0].Points, -400);
            Assert.AreEqual(expenditure[0].Payer, "CVS Pharmacy");
        }


        [TestMethod]
        public void SpendPoints_ConsumeTwoTransactions_TransactionsRemoved()
        {
            List<Transaction> transactions = ArrangeTransactions();

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            var expenditure = localMemOperations.SpendPoints(700);
            var balances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(balances[0].Points, 0);
            Assert.AreEqual(balances[1].Points, 300);
            Assert.AreEqual(balances.Count, 2);
            Assert.AreEqual(expenditure.Count, 2);
            Assert.AreEqual(expenditure[0].Points, -400);
            Assert.AreEqual(expenditure[0].Payer, "CVS Pharmacy");
            Assert.AreEqual(expenditure[1].Points, -300);
            Assert.AreEqual(expenditure[1].Payer, "Wegman's");

        }

        [TestMethod]
        public void SpendPoints_HalfConsumedTransaction_PayerPointsDeducted()
        {
            List<Transaction> transactions = ArrangeTransactions();

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            var expenditure = localMemOperations.SpendPoints(500);
            var balances = localMemOperations.GetPayerBalances();

            Assert.AreEqual(balances[1].Points, 300);
            Assert.AreEqual(balances[0].Points, 200);
            Assert.AreEqual(balances.Count, 2);
            Assert.AreEqual(expenditure.Count, 2);
            Assert.AreEqual(expenditure[0].Points, -400);
            Assert.AreEqual(expenditure[0].Payer, "CVS Pharmacy");
            Assert.AreEqual(expenditure[1].Points, -100);
            Assert.AreEqual(expenditure[1].Payer, "Wegman's");

        }

        public List<Transaction> ArrangeTransactions()
        {
            return new List<Transaction> {
                new Transaction{
                    Payer="CVS Pharmacy",
                    Points=400,
                    Timestamp=new DateTime(2021, 02, 04)
                },
                new Transaction{
                    Payer="Wegman's",
                    Points=300,
                    Timestamp=new DateTime(2021, 03, 04)
                },
                new Transaction{
                    Payer="CVS Pharmacy",
                    Points=300,
                    Timestamp=new DateTime(2021, 04, 04)
                }
            };
        }
    }
}
