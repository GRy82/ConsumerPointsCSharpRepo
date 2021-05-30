using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.Models;

namespace ConsumerPoints.Data
{
    public class MinHeap
    {
        private int capacity = 10;
        private int count = 0;
        Transaction[] transactions = new Transaction[10];

        public void Insert(Transaction transaction)
        {
            if (IsFull()) Resize();

            int addedindex = count;
            transactions[addedindex] = transaction;
           
            BubbleUp(transaction, addedindex);
        }

        private void BubbleUp(Transaction transaction, int transactionIndex)
        {
            int parentIndex = transactionIndex % 2 == 0 ? 
                transactionIndex / 2 - 1 : transactionIndex / 2;

            if (transactionIndex == 0 || transaction.Timestamp.CompareTo(transactions[parentIndex]) > 1) 
                return;

            transactions[transactionIndex] = transactions[parentIndex];
            transactions[parentIndex] = transaction;

            BubbleUp(transaction, parentIndex);
        }

        private bool IsFull()
        {
            if (count == capacity) return true;

            return false;
        }

        private void Resize()
        {
            capacity *= 2;
            Transaction[] temp = transactions;
            transactions = new Transaction[capacity];
            for (int i = 0; i < temp.Length; i++)
                transactions[i] = temp[i];
        }
    }
}
