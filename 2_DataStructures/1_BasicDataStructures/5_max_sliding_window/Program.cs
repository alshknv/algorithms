using System;
using System.Collections.Generic;
using System.Linq;

namespace _5_max_sliding_window
{

    public class StackItem
    {
        public StackItem Next;
        public long Max;
        public long Value;
    }

    public class MaxStack
    {
        private StackItem top;
        public void Push(long value)
        {
            top = new StackItem()
            {
                Value = value,
                Max = Math.Max(top?.Max ?? long.MinValue, value),
                Next = top
            };
        }

        public long Pop()
        {
            var pop = top.Value;
            top = top.Next;
            return pop;
        }

        public long Max()
        {
            return top?.Max ?? long.MinValue;
        }

        public bool Empty()
        {
            return top == null;
        }
    }

    public class MaxSlidingQueue
    {
        private readonly MaxStack head = new MaxStack();
        private readonly MaxStack tail = new MaxStack();

        public void Enqueue(long item)
        {
            tail.Push(item);
        }

        public long Dequeue()
        {
            if (head.Empty())
            {
                while (!tail.Empty())
                {
                    head.Push(tail.Pop());
                }
            }
            return head.Pop();
        }

        public long Max()
        {
            return Math.Max(head.Max(), tail.Max());
        }
    }

    public static class MaxSlidingWindow
    {
        public static string Naive(string dataline, string mline)
        {
            var data = dataline.Split(' ').Select(x => int.Parse(x)).ToArray();
            var m = int.Parse(mline);
            var result = new int[data.Length - m + 1];

            for (int i = 0; i <= data.Length - m; i++)
            {
                result[i] = int.MinValue;
                for (int j = 0; j < m; j++)
                {
                    if (data[i + j] > result[i])
                    {
                        result[i] = data[i + j];
                    }
                }
            }
            return string.Join(" ", result);
        }

        public static string Solve(string dataline, string mline)
        {
            var data = dataline.Split(' ').Select(x => long.Parse(x)).ToArray();
            var m = int.Parse(mline);
            var result = new long[data.Length - m + 1];
            var queue = new MaxSlidingQueue();
            for (int i = 0; i < m; i++)
            {
                queue.Enqueue(data[i]);
            }
            for (int j = m; j < data.Length; j++)
            {
                result[j - m] = queue.Max();
                queue.Dequeue();
                queue.Enqueue(data[j]);
            }
            result[data.Length - m] = queue.Max();
            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            var data = Console.ReadLine();
            var mline = Console.ReadLine();
            Console.WriteLine(Solve(data, mline));
        }
    }
}
