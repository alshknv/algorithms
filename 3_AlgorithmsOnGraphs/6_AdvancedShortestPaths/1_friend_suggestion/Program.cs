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
        public Vertex(int count)
        {
            Edges = new List<Edge>(count);
            EdgesR = new List<Edge>(count);
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
        private static List<int> visitedNodes;
        private static long minDistance;

        private static Vertex[] vertices;

        private static void ProcessFwd(int node)
        {
            if (dist[node] < long.MaxValue && !proc[node])
            {
                foreach (var e in vertices[node].Edges)
                {
                    if (dist[e.Destination] > dist[node] + e.Weight)
                    {
                        dist[e.Destination] = dist[node] + e.Weight;
                        queue.SetPriority(e.Destination, dist[e.Destination]);
                        if (dist[e.Destination] < long.MaxValue && distR[e.Destination] < long.MaxValue && dist[e.Destination] + distR[e.Destination] < minDistance)
                            minDistance = dist[e.Destination] + distR[e.Destination];
                        visitedNodes.Add(e.Destination);
                    }
                }
                if (distR[node] < long.MaxValue && dist[node] + distR[node] < minDistance)
                    minDistance = dist[node] + distR[node];
                proc[node] = true;
            }
        }

        private static void ProcessBck(int node)
        {
            if (distR[node] < long.MaxValue && !procR[node])
            {
                foreach (var e in vertices[node].EdgesR)
                {
                    if (distR[e.Destination] > distR[node] + e.Weight)
                    {
                        distR[e.Destination] = distR[node] + e.Weight;
                        queueR.SetPriority(e.Destination, distR[e.Destination]);
                        if (dist[e.Destination] < long.MaxValue && dist[e.Destination] + distR[e.Destination] < minDistance)
                            minDistance = dist[e.Destination] + distR[e.Destination];
                        visitedNodes.Add(e.Destination);
                    }
                }
                if (dist[node] < long.MaxValue && dist[node] + distR[node] < minDistance)
                    minDistance = dist[node] + distR[node];
                procR[node] = true;
            }
        }

        private static long BidirectionalDijkstra(int start, int end)
        {
            dist[start] = 0;
            distR[end] = 0;
            visitedNodes.Clear();
            visitedNodes.Add(start);
            if (end != start) visitedNodes.Add(end);

            queue.Clear();
            queue.SetPriority(start, 0);
            queueR.Clear();
            queueR.SetPriority(end, 0);

            minDistance = long.MaxValue;

            while (!queue.Empty() || !queueR.Empty())
            {
                if (!queue.Empty())
                {
                    var q = queue.ExtractMin();
                    ProcessFwd(q.Index);
                    if (procR[q.Index])
                        break;
                }

                if (!queueR.Empty())
                {
                    var q = queueR.ExtractMin();
                    ProcessBck(q.Index);
                    if (proc[q.Index])
                        break;
                }
            }
            foreach (var i in visitedNodes)
            {
                proc[i] = procR[i] = false;
                dist[i] = distR[i] = long.MaxValue;
            }
            return minDistance < long.MaxValue ? minDistance : -1;
        }

        public static string[] Solve(int vertexCount, string[] edges, string[] queries)
        {
            vertices = new Vertex[vertexCount + 1];
            for (int i = 1; i <= vertexCount; i++)
            {
                vertices[i] = new Vertex(50);
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

            for (int i = 0; i < dist.Length; i++)
            {
                proc[i] = procR[i] = false;
                dist[i] = distR[i] = long.MaxValue;
            }

            queue = new PriorityQueue(vertexCount);
            queueR = new PriorityQueue(vertexCount);

            visitedNodes = new List<int>(1000);

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
