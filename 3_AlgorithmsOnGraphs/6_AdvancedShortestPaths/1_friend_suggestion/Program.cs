using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_friend_suggestion
{
    public class Edge
    {
        public readonly int Destination;
        public readonly int Weight;

        public Edge(int destination, int weight)
        {
            Destination = destination;
            Weight = weight;
        }
    }

    public class Vertex
    {
        public readonly LinkedList<Edge> Edges;
        public readonly LinkedList<Edge> EdgesR;
        public Vertex()
        {
            Edges = new LinkedList<Edge>();
            EdgesR = new LinkedList<Edge>();
        }
        public void AddEdge(int destination, int weight)
        {
            Edges.AddLast(new Edge(destination, weight));
        }
        public void AddEdgeR(int destination, int weight)
        {
            EdgesR.AddLast(new Edge(destination, weight));
        }
    }

    public class QueueItem
    {
        public readonly int Index;
        public readonly long Value;
        public bool Active;

        public QueueItem(int index, long value, bool active)
        {
            Index = index;
            Value = value;
            Active = active;
        }
    }

    public class PriorityQueue
    {
        private readonly List<QueueItem> data;
        private void Swap(int a, int b)
        {
            var buf = data[a];
            data[a] = data[b];
            data[b] = buf;
        }

        private void SiftUp(int index)
        {
            var nextI = index;
            do
            {
                index = nextI;
                var p = (index - 1) / 2;
                if (!data[p].Active || (p >= 0 && (data[index].Value < data[p].Value)))
                {
                    nextI = p;
                    Swap(index, nextI);
                }
            } while (nextI != index);
        }

        private void SiftDown(int i)
        {
            while (i < data.Count)
            {
                var c1 = 2 * i + 1;
                var c2 = 2 * i + 2;
                var nextI = c1 < data.Count && data[c1].Value < data[i].Value ? c1 : i;
                nextI = c2 < data.Count && data[c2].Value < data[nextI].Value ? c2 : nextI;
                if (nextI != i)
                {
                    Swap(i, nextI);
                    i = nextI;
                }
                else
                {
                    break;
                }
            }
        }

        public PriorityQueue(long[] dist)
        {
            data = dist.Select((x, i) => new QueueItem(i + 1, x, true)).ToList();
            for (int i = (data.Count - 1) / 2; i >= 0; i--)
            {
                SiftDown(i);
            }
        }

        public QueueItem ExtractMin()
        {
            var result = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            SiftDown(0);
            return result;
        }

        public void ChangePriority(QueueItem current, int item, long priority)
        {
            current.Active = false;
            data.Add(new QueueItem(item, priority, true));
            SiftUp(data.Count - 1);
        }

        public bool Empty()
        {
            return data.Count == 0;
        }
    }

    public static class Extensions
    {
        public static int[] AsIntArray(this string line)
        {
            return line.Split(' ').Select(x => int.Parse(x)).ToArray();
        }
    }

    public static class FriendSuggestion
    {
        private static bool Process(QueueItem q, Vertex[] vertices, PriorityQueue queue, long[] dist, int[] prev, bool[] proc, bool forward)
        {
            if (dist[q.Index] < long.MaxValue && !proc[q.Index])
            {
                var listItem = forward ? vertices[q.Index].Edges.First : vertices[q.Index].EdgesR.First;
                while (listItem != null)
                {
                    var e = listItem.Value;
                    if (dist[e.Destination] > dist[q.Index] + e.Weight)
                    {
                        dist[e.Destination] = dist[q.Index] + e.Weight;
                        prev[e.Destination] = q.Index;
                        queue.ChangePriority(q, e.Destination, dist[e.Destination]);
                    }
                    listItem = listItem.Next;
                }

                proc[q.Index] = true;
                return true;
            }
            return false;
        }

        private static long ShortestPath(long[] dist, bool[] proc, long[] distR, bool[] procR)
        {
            var distance = long.MaxValue;
            for (int i = 0; i < proc.Length; i++)
            {
                if (proc[i] || procR[i])
                {
                    if (dist[i] + distR[i] >= 0 && dist[i] + distR[i] < distance)
                    {
                        distance = dist[i] + distR[i];
                    }
                }
            }
            return distance;
        }

        private static long BidirectionalDijkstra(Vertex[] vertices, int start, int end)
        {
            var dist = new long[vertices.Length];
            var prev = new int[vertices.Length];
            var distR = new long[vertices.Length];
            var prevR = new int[vertices.Length];
            var proc = new bool[vertices.Length];
            var procR = new bool[vertices.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = distR[i] = long.MaxValue;
                prev[i] = prevR[i] = -1;
            }
            dist[start] = 0;
            distR[end] = 0;

            var queue = new PriorityQueue(dist.Skip(1).ToArray());
            var queueR = new PriorityQueue(distR.Skip(1).ToArray());

            while (!queue.Empty() && !queueR.Empty())
            {
                if (!queue.Empty())
                {
                    var q = queue.ExtractMin();
                    if (Process(q, vertices, queue, dist, prev, proc, true) && procR[q.Index])
                    {
                        return ShortestPath(dist, proc, distR, procR);
                    }
                }

                if (!queueR.Empty())
                {
                    var q = queueR.ExtractMin();
                    if (Process(q, vertices, queueR, distR, prevR, procR, false) && proc[q.Index])
                    {
                        return ShortestPath(dist, proc, distR, procR);
                    }
                }
            }
            return -1;
        }

        public static string[] Solve(int vertexCount, string[] edges, string[] queries)
        {
            var vertices = new Vertex[vertexCount + 1];
            for (int i = 1; i <= vertexCount; i++)
            {
                vertices[i] = new Vertex();
            }
            //graph init
            for (int i = 0; i < edges.Length; i++)
            {
                var e = edges[i].AsIntArray();
                vertices[e[0]].AddEdge(e[1], e[2]);
                vertices[e[1]].AddEdgeR(e[0], e[2]);
            }
            //queries
            var result = new string[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var query = queries[i].AsIntArray();
                result[i] = BidirectionalDijkstra(vertices, query[0], query[1]).ToString();
            }

            return result;
        }

        public static void Main(string[] args)
        {
            var nm = Console.ReadLine().AsIntArray();
            var edges = new string[nm[1]];
            for (int i = 0; i < nm[1]; i++)
            {
                edges[i] = Console.ReadLine();
            }
            var q = int.Parse(Console.ReadLine());
            var queries = new string[q];
            for (int i = 0; i < q; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in Solve(nm[0], edges, queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
