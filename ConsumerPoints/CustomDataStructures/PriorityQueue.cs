using ConsumerPoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.CustomDataStructures
{
    public class PriorityQueue
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
    }
}
