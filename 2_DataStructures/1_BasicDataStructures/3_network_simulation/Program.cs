using System.Collections.Generic;
using System.Linq;
using System;

namespace _3_network_simulation
{
    public class Packet
    {
        public int Index;
        public long Arrival;
        public long ProcessTime;
        public Packet(int index, long arrival, long processTime)
        {
            Index = index;
            Arrival = arrival;
            ProcessTime = processTime;
        }
    }

    public class QueueItem<T>
    {
        public QueueItem<T> Next;
        public QueueItem<T> Prev;
        public T Item;
    }

    public class MyQueue<T>
    {
        private QueueItem<T> begin;
        private QueueItem<T> end;

        private int count = 0;

        public int Count
        {
            get { return count; }
        }

        public void Enqueue(T item)
        {
            var newEnd = new QueueItem<T>()
            {
                Item = item
            };
            if (end == null)
            {
                end = begin = newEnd;
            }
            else
            {
                newEnd.Prev = new QueueItem<T>()
                {
                    Item = end.Item,
                    Next = newEnd,
                    Prev = end
                };
                if (begin.Equals(end)) begin = newEnd.Prev;
                end.Next = newEnd;
                end = newEnd;
            }
            count++;
        }

        public bool TryPeek(out T item)
        {

            if (Empty())
            {
                item = default(T);
                return false;
            }
            else
            {
                item = begin.Item;
                return true;
            }
        }

        public T Dequeue()
        {
            var item = begin.Item;
            begin = begin.Next;
            if (begin == null) end = null;
            count--;
            return item;
        }

        public bool Empty()
        {
            return begin == null;
        }
    }

    public static class NetworkSimulation
    {
        public static string[] Solve(string line1, string[] data)
        {
            var bufSize = int.Parse(line1.Split(' ')[0]);
            var queue = new MyQueue<Packet>();
            var packets = data.Select((x, i) =>
            {
                var d = x.Split(' ');
                return new Packet(i, long.Parse(d[0]), long.Parse(d[1]));
            }).ToArray();
            var result = new string[packets.Length];
            long beginProcess = 0;
            for (int i = 0; i < packets.Length; i++)
            {
                if (i == 0) beginProcess = packets[i].Arrival;
                Packet packet;
                if (queue.TryPeek(out packet) && beginProcess + packet.ProcessTime <= packets[i].Arrival)
                {
                    packet = queue.Dequeue();
                    if (packet.Arrival > beginProcess)
                    {
                        beginProcess = packet.Arrival;
                    }
                    result[packet.Index] = beginProcess.ToString();
                    beginProcess += packet.ProcessTime;
                }
                if (queue.Count < bufSize)
                {
                    queue.Enqueue(packets[i]);
                }
                else
                {
                    result[i] = "-1";
                }
            }
            while (queue.Count > 0)
            {
                var packet = queue.Dequeue();
                if (packet.Arrival > beginProcess)
                {
                    beginProcess = packet.Arrival;
                }
                result[packet.Index] = beginProcess.ToString();
                beginProcess += packet.ProcessTime;
            }
            return result;
        }

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine();
            var n = int.Parse(line1.Split(' ')[1]);
            var data = new string[n];
            for (int i = 0; i < n; i++)
            {
                data[i] = Console.ReadLine();
            }
            foreach (var line in Solve(line1, data))
            {
                Console.WriteLine(line);
            }
        }
    }
}
