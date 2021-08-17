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

    public static class NetworkSimulation
    {
        public static string[] Solve(string line1, string[] data)
        {
            var bufSize = int.Parse(line1.Split(' ')[0]);
            var queue = new Queue<Packet>();
            var packets = data.Select((x, i) =>
            {
                var d = x.Split(' ');
                return new Packet(i, long.Parse(d[0]), long.Parse(d[1]));
            }).ToArray();
            var result = new string[packets.Length];
            long beginProcess = 0;
            for (int i = 0; i < packets.Length; i++)
            {
                Packet packet;
                if (queue.TryPeek(out packet) && beginProcess + packet.ProcessTime <= packets[i].Arrival)
                {
                    packet = queue.Dequeue();
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
            Console.WriteLine(Solve(line1, data));
        }
    }
}
