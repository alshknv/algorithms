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
        public readonly List<Edge> Edges;
        public readonly List<Edge> EdgesR;
        public Vertex()
        {
            Edges = new List<Edge>(100);
            EdgesR = new List<Edge>(100);
        }
        public void AddEdge(int destination, int weight)
        {
            Edges.Add(new Edge(destination, weight));
        }
        public void AddEdgeR(int destination, int weight)
        {
            EdgesR.Add(new Edge(destination, weight));
        }
    }

    public class QueueItem
    {
        public readonly int Index;
        public readonly long Value;

        public QueueItem(int index, long value)
        {
            Index = index;
            Value = value;
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
                if (p >= 0 && (data[index].Value < data[p].Value))
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

        public PriorityQueue(int count)
        {
            data = new List<QueueItem>(count);
        }

        public void Clear()
        {
            data.Clear();
        }

        public QueueItem ExtractMin()
        {
            var result = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            SiftDown(0);
            return result;
        }

        public void SetPriority(int item, long priority)
        {
            data.Add(new QueueItem(item, priority));
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
        private static long[] dist;
        private static long[] distR;
        private static bool[] proc;
        private static bool[] procR;
        private static PriorityQueue queue;
        private static PriorityQueue queueR;
        private static Dictionary<int, long[]> visitedNodes;

        private static Vertex[] vertices;

        private static void Process(QueueItem q, PriorityQueue queue, long[] dist, bool[] proc, bool forward)
        {
            if (dist[q.Index] < long.MaxValue && !proc[q.Index])
            {
                foreach (var e in forward ? vertices[q.Index].Edges : vertices[q.Index].EdgesR)
                {
                    if (dist[e.Destination] > dist[q.Index] + e.Weight)
                    {
                        dist[e.Destination] = dist[q.Index] + e.Weight;
                        queue.SetPriority(e.Destination, dist[e.Destination]);

                        if (!visitedNodes.ContainsKey(e.Destination))
                        {
                            visitedNodes.Add(e.Destination, new long[2] { forward ? dist[e.Destination] : long.MaxValue, forward ? long.MaxValue : dist[e.Destination] });
                        }
                        else
                        {
                            visitedNodes[e.Destination][forward ? 0 : 1] = dist[e.Destination];
                        }
                    }
                }
                proc[q.Index] = true;
            }
        }

        private static long ShortestPath()
        {
            var distance = long.MaxValue;
            foreach (var value in visitedNodes.Values)
            {
                if (value[0] < long.MaxValue && value[1] < long.MaxValue)
                {
                    if (value[0] + value[1] < distance)
                        distance = value[0] + value[1];
                }
            }
            return distance;
        }

        private static long BidirectionalDijkstra(int start, int end)
        {
            for (int i = 0; i < dist.Length; i++)
            {
                proc[i] = procR[i] = false;
                dist[i] = distR[i] = long.MaxValue;
            }
            dist[start] = 0;
            distR[end] = 0;
            if (start == end) return 0;
            visitedNodes.Clear();
            visitedNodes.Add(start, new long[2] { 0, long.MaxValue });
            visitedNodes.Add(end, new long[2] { long.MaxValue, 0 });

            queue.Clear();
            queue.SetPriority(start, 0);
            queueR.Clear();
            queueR.SetPriority(end, 0);

            while (!queue.Empty() || !queueR.Empty())
            {
                if (!queue.Empty())
                {
                    var q = queue.ExtractMin();
                    Process(q, queue, dist, proc, true);
                    if (procR[q.Index])
                        return ShortestPath();
                }

                if (!queueR.Empty())
                {
                    var q = queueR.ExtractMin();
                    Process(q, queueR, distR, procR, false);
                    if (proc[q.Index])
                        return ShortestPath();
                }
            }
            return -1;
        }

        public static string[] Solve(int vertexCount, string[] edges, string[] queries)
        {
            vertices = new Vertex[vertexCount + 1];
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

            dist = new long[vertices.Length];
            distR = new long[vertices.Length];
            proc = new bool[vertices.Length];
            procR = new bool[vertices.Length];

            queue = new PriorityQueue(vertexCount);
            queueR = new PriorityQueue(vertexCount);

            visitedNodes = new Dictionary<int, long[]>();

            //queries
            var result = new string[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var query = queries[i].AsIntArray();
                result[i] = BidirectionalDijkstra(query[0], query[1]).ToString();
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
