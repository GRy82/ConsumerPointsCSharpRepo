using ConsumerPoints.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.CustomDataStructures
{
    public class TransactionQueue : IEnumerable
    {
        private MinHeap minHeap = new MinHeap();

        public void Enqueue(Transaction transaction)
        {
            minHeap.Insert(transaction);
        }

        public Transaction Dequeue()
        {
            return minHeap.Remove();
        }

        public bool IsEmpty()
        {
            return minHeap.IsEmpty();
        }

        public List<Transaction> ToList()
        {
            return minHeap.ToList();
        }

        public IEnumerator GetEnumerator()
        {
            int count = minHeap.Count;
            var transactions = minHeap.GetTransactions();
            for (int i = 0; i < count; i++)
            {
                yield return transactions[i];            
            }
        }
    }
}
