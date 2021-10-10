using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _2_dist_with_coords
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
        public readonly long X;
        public readonly long Y;
        public Vertex(int count, long x, long y)
        {
            X = x; Y = y;
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
        public readonly double Value;

        public QueueItem(int index, double value)
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

        public void SetPriority(int item, double priority)
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

    public static class DistWithCoords
    {
        private static long[] dist;
        private static long[] distR;
        private static bool[] proc;
        private static bool[] procR;
        private static double[] distPi;
        private static double[] distPiR;
        private static PriorityQueue queue;
        private static PriorityQueue queueR;
        private static List<int> visitedNodes;
        private static Vertex[] vertices;
        private static long minDistance;

        private static double Distance(Vertex u, Vertex v)
        {
            return Math.Sqrt(Math.Pow(u.X - v.X, 2) + Math.Pow(u.Y - v.Y, 2));
        }

        private static double Pi(Vertex target, Vertex start, Vertex u)
        {
            return (Distance(target, u) - Distance(u, start)) / 2;
        }

        private static void ProcessFwd(int q, int start, int target)
        {
            if (!proc[q])
            {
                foreach (var e in vertices[q].Edges)
                {
                    var lpi = e.Weight - Pi(vertices[target], vertices[start], vertices[q])
                            + Pi(vertices[target], vertices[start], vertices[e.Destination]);
                    if (distPi[e.Destination] > distPi[q] + lpi)
                    {
                        dist[e.Destination] = dist[q] + e.Weight;
                        distPi[e.Destination] = distPi[q] + lpi;
                        queue.SetPriority(e.Destination, distPi[e.Destination]);
                        if (distR[e.Destination] < long.MaxValue && dist[e.Destination] + distR[e.Destination] < minDistance)
                            minDistance = dist[e.Destination] + distR[e.Destination];
                        visitedNodes.Add(e.Destination);
                    }
                }
                if (distR[q] < long.MaxValue && dist[q] + distR[q] < minDistance)
                    minDistance = dist[q] + distR[q];
                proc[q] = true;
            }
        }

        private static void ProcessBck(int q, int start, int target)
        {
            if (!procR[q])
            {
                foreach (var e in vertices[q].EdgesR)
                {
                    var lpi = e.Weight - Pi(vertices[target], vertices[start], vertices[q])
                            + Pi(vertices[target], vertices[start], vertices[e.Destination]);
                    if (distPiR[e.Destination] > distPiR[q] + lpi)
                    {
                        distR[e.Destination] = distR[q] + e.Weight;
                        distPiR[e.Destination] = distPiR[q] + lpi;
                        queueR.SetPriority(e.Destination, distPiR[e.Destination]);
                        if (dist[e.Destination] < long.MaxValue && dist[e.Destination] + distR[e.Destination] < minDistance)
                            minDistance = dist[e.Destination] + distR[e.Destination];
                        visitedNodes.Add(e.Destination);
                    }
                }
                if (dist[q] < long.MaxValue && dist[q] + distR[q] < minDistance)
                    minDistance = dist[q] + distR[q];
                procR[q] = true;
            }
        }

        private static long BidirectionalAStar(int start, int end)
        {
            dist[start] = 0;
            distPi[start] = 0;
            distR[end] = 0;
            distPiR[end] = 0;
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
                    ProcessFwd(q.Index, start, end);
                    if (procR[q.Index])
                        break;
                }

                if (!queueR.Empty())
                {
                    var q = queueR.ExtractMin();
                    ProcessBck(q.Index, end, start);
                    if (proc[q.Index])
                        break;
                }
            }
            foreach (var i in visitedNodes)
            {
                proc[i] = procR[i] = false;
                dist[i] = distR[i] = long.MaxValue;
                distPi[i] = distPiR[i] = double.MaxValue;
            }
            return minDistance < long.MaxValue ? minDistance : -1;
        }

        public static string[] Solve(string[] vertexData, string[] edgeData, string[] queryData)
        {
            //graph init
            vertices = new Vertex[vertexData.Length + 1];
            for (int i = 1; i <= vertexData.Length; i++)
            {
                var v = vertexData[i - 1].AsIntArray();
                vertices[i] = new Vertex(50, v[0], v[1]);
            }
            for (int i = 0; i < edgeData.Length; i++)
            {
                var e = edgeData[i].AsIntArray();
                vertices[e[0]].AddEdge(e[1], e[2]);
                vertices[e[1]].AddEdgeR(e[0], e[2]);
            }

            dist = new long[vertices.Length];
            distR = new long[vertices.Length];
            proc = new bool[vertices.Length];
            procR = new bool[vertices.Length];
            distPi = new double[vertices.Length];
            distPiR = new double[vertices.Length];
            queue = new PriorityQueue(vertexData.Length);
            queueR = new PriorityQueue(vertexData.Length);
            visitedNodes = new List<int>(1000);

            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = distR[i] = long.MaxValue;
                distPi[i] = distPiR[i] = double.MaxValue;
                proc[i] = procR[i] = false;
            }

            //queries
            var result = new string[queryData.Length];
            for (int i = 0; i < queryData.Length; i++)
            {
                var q = queryData[i].AsIntArray();
                result[i] = BidirectionalAStar(q[0], q[1]).ToString();
            }
            return result;
        }

        public static void Main(string[] args)
        {
            var nm = Console.ReadLine().AsIntArray();
            var vertices = new string[nm[0]];
            for (int i = 0; i < nm[0]; i++)
            {
                vertices[i] = Console.ReadLine();
            }
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
            foreach (var line in Solve(vertices, edges, queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
