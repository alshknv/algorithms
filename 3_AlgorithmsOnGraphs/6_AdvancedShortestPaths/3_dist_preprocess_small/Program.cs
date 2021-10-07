using System;
using System.Collections.Generic;
using System.Linq;

namespace _3_dist_preprocess_small
{
    public static class Extensions
    {
        public static int[] AsIntArray(this string line)
        {
            return line.Split(' ').Select(x => int.Parse(x)).ToArray();
        }
    }

    public class Edge
    {
        public readonly int Source;
        public readonly int Destination;
        public readonly int Weight;

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }

    public class Vertex
    {
        public readonly List<Edge> Edges;
        public readonly List<Edge> Predecessors;
        public List<Edge> EdgesR;
        public int Order;
        public int Level;
        public int ContractedNeighbors;
        public int EdgeDifference;
        public int ShortcutCover;
        public bool Contracted;
        public int Importance
        {
            get { return EdgeDifference + ShortcutCover + ContractedNeighbors + Level; }
        }
        public Vertex()
        {
            Edges = new List<Edge>(10000);
            Predecessors = new List<Edge>(10000);
        }
        public void AddEdge(int source, int destination, int weight)
        {
            Edges.Add(new Edge(source, destination, weight));
        }
        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }
        public void AddPredecessor(int source, int destination, int weight)
        {
            Predecessors.Add(new Edge(source, destination, weight));
        }
        public void AddPredecessor(Edge edge)
        {
            Predecessors.Add(edge);
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

        public PriorityQueue(Vertex[] vertices)
        {
            data = vertices.Select((x, i) => new QueueItem(i + 1, 0, true)).ToList();
        }

        public PriorityQueue(long[] dist)
        {
            data = dist.Select((x, i) => new QueueItem(i + 1, x, true)).ToList();
            for (int i = (data.Count - 1) / 2; i >= 0; i--)
            {
                SiftDown(i);
            }
        }

        public QueueItem GetMin()
        {
            return data.Count > 0 ? data[0] : null;
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

        public void Clear()
        {
            data.Clear();
        }

        public bool Empty()
        {
            return data.Count == 0;
        }
    }


    public static class DistPreprocessSmall
    {
        private static Vertex[] Vertices;

        private static Edge[] WitnessSearch(int nindex)
        {
            var shortcuts = new List<Edge>(1000);
            var maxL = 0;
            foreach (var pred in Vertices[nindex].Predecessors)
            {
                foreach (var succ in Vertices[nindex].Edges)
                {
                    if (pred.Weight + succ.Weight > maxL) maxL = pred.Weight + succ.Weight;
                }
            }

            foreach (var pred in Vertices[nindex].Predecessors)
            {
                if (Vertices[pred.Source].Contracted) continue;
                // witness dijkstra
                var dist = new long[Vertices.Length];
                var hops = new int[Vertices.Length];
                for (int i = 0; i < Vertices.Length; i++)
                {
                    dist[i] = long.MaxValue;
                }
                dist[pred.Source] = 0;
                var queue = new PriorityQueue(dist.Skip(1).ToArray());
                while (!queue.Empty())
                {
                    var u = queue.ExtractMin();
                    if (dist[u.Index] == long.MaxValue) continue;
                    foreach (var v in Vertices[u.Index].Edges)
                    {
                        if (v.Destination == nindex) continue;
                        if (dist[v.Destination] > dist[u.Index] + v.Weight)
                        {
                            dist[v.Destination] = dist[u.Index] + v.Weight;
                            hops[v.Destination] = hops[u.Index] + 1;
                            queue.ChangePriority(u, v.Destination, dist[v.Destination]);
                            if (dist[v.Destination] > maxL || hops[v.Destination] > 10)
                            {
                                queue.Clear();
                                break;
                            }
                        }
                    }
                }

                foreach (var succ in Vertices[nindex].Edges)
                {
                    if (dist[succ.Destination] > pred.Weight + succ.Weight && !Vertices[succ.Destination].Contracted)
                    {
                        //planning shortcut
                        shortcuts.Add(new Edge(pred.Source, succ.Destination, pred.Weight + succ.Weight));
                    }
                }
            }

            return shortcuts.ToArray();
        }

        private static void UpdateNeighborImportance(Vertex v)
        {
            var level = v.Level + 1;
            foreach (var pred in v.Predecessors)
            {
                Vertices[pred.Source].ContractedNeighbors++;
                level = Math.Max(level, Vertices[pred.Source].Level);
            }
            foreach (var succ in v.Edges)
            {
                Vertices[succ.Source].ContractedNeighbors++;
                level = Math.Max(level, Vertices[succ.Source].Level);
            }
        }

        public static void Preprocess(int vertexCount, string[] graph)
        {
            //graph init
            Vertices = new Vertex[vertexCount + 1];
            for (int i = 1; i < Vertices.Length; i++)
            {
                Vertices[i] = new Vertex();
            }
            for (int i = 0; i < graph.Length; i++)
            {
                var e = graph[i].AsIntArray();
                Vertices[e[0]].AddEdge(e[0], e[1], e[2]);
                Vertices[e[1]].AddPredecessor(e[0], e[1], e[2]);
            }
            var importanceQueue = new PriorityQueue(Vertices.Skip(1).ToArray());
            var contractOrder = 0;
            while (!importanceQueue.Empty())
            {
                var node = importanceQueue.ExtractMin();
                var shortcuts = WitnessSearch(node.Index);
                Vertices[node.Index].EdgeDifference = shortcuts.Length - Vertices[node.Index].Predecessors.Count - Vertices[node.Index].Edges.Count;
                Vertices[node.Index].ShortcutCover = shortcuts.Length;
                var nextMin = importanceQueue.GetMin();
                if (nextMin == null || Vertices[node.Index].Importance <= Vertices[nextMin.Index].Importance)
                {
                    // contract the least important node
                    Vertices[node.Index].Contracted = true;
                    Vertices[node.Index].Order = contractOrder++;
                    UpdateNeighborImportance(Vertices[node.Index]);

                    // after node is contracted we store upward outgoing edges for forward search
                    // and upward predecessor connections as edges for reverse search
                    Vertices[node.Index].EdgesR = Vertices[node.Index].Predecessors
                        .Where(p => !Vertices[p.Source].Contracted || Vertices[p.Source].Order > Vertices[node.Index].Order)
                        .Select(p => new Edge(p.Destination, p.Source, p.Weight))
                        .ToList();
                    Vertices[node.Index].Predecessors.Clear();
                    Vertices[node.Index].Edges
                        .RemoveAll(e => Vertices[e.Destination].Contracted && Vertices[e.Destination].Order < Vertices[node.Index].Order);

                    // adding shortcut
                    foreach (var shortcut in shortcuts)
                    {
                        Vertices[shortcut.Source].AddEdge(shortcut);
                        Vertices[shortcut.Destination].AddPredecessor(shortcut);
                    }
                }
                else
                {
                    importanceQueue.ChangePriority(node, node.Index, Vertices[node.Index].Importance);
                    continue;
                }
            }
        }

        private static bool Process(QueueItem q, PriorityQueue queue, long[] dist, bool[] proc, bool forward)
        {
            if (dist[q.Index] < long.MaxValue && !proc[q.Index])
            {
                foreach (var edge in forward ? Vertices[q.Index].Edges : Vertices[q.Index].EdgesR)
                {
                    if (dist[edge.Destination] > dist[q.Index] + edge.Weight)
                    {
                        dist[edge.Destination] = dist[q.Index] + edge.Weight;
                        queue.ChangePriority(q, edge.Destination, dist[edge.Destination]);
                    }
                }
                proc[q.Index] = true;
                return true;
            }
            return false;
        }

        public static string[] ProcessQueries(string[] queries)
        {
            var result = new string[queries.Length];
            for (int k = 0; k < queries.Length; k++)
            {
                var query = queries[k].AsIntArray();
                // bidirectional upward Dijkstra
                var estimate = long.MaxValue;
                var dist = new long[Vertices.Length];
                var distR = new long[Vertices.Length];
                var proc = new bool[Vertices.Length];
                var procR = new bool[Vertices.Length];
                for (int i = 0; i < Vertices.Length; i++)
                {
                    dist[i] = distR[i] = long.MaxValue;
                }
                dist[query[0]] = distR[query[1]] = 0;

                var queue = new PriorityQueue(dist.Skip(1).ToArray());
                var queueR = new PriorityQueue(distR.Skip(1).ToArray());

                while (!queue.Empty() && !queueR.Empty())
                {
                    if (!queue.Empty())
                    {
                        var q = queue.ExtractMin();
                        if (dist[q.Index] <= estimate)
                        {
                            if (Process(q, queue, dist, proc, true))
                            {
                                if (procR[q.Index] && dist[q.Index] + distR[q.Index] < estimate)
                                    estimate = dist[q.Index] + distR[q.Index];
                            }
                        }
                    }
                    if (!queueR.Empty())
                    {
                        var q = queueR.ExtractMin();
                        if (distR[q.Index] <= estimate)
                        {
                            if (Process(q, queueR, distR, procR, false))
                            {
                                if (proc[q.Index] && dist[q.Index] + distR[q.Index] < estimate)
                                    estimate = dist[q.Index] + distR[q.Index];
                            }
                        }
                    }
                }
                result[k] = (estimate < long.MaxValue ? estimate : -1).ToString();
            }
            return result;
        }

        static void Main(string[] args)
        {
            var nm = Console.ReadLine().AsIntArray();
            var graph = new string[nm[1]];
            for (int i = 0; i < nm[1]; i++)
            {
                graph[i] = Console.ReadLine();
            }
            Preprocess(nm[0], graph);
            Console.WriteLine("Ready");
            var q = int.Parse(Console.ReadLine());
            var queries = new string[q];
            for (int i = 0; i < q; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in ProcessQueries(queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
