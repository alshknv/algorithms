using System.Collections.Generic;
using System.Linq;
using System;
namespace _3_stock_charts
{
    public class Edge
    {
        public int Flow;
        public int Source;
        public int Destination;
    }

    public class TwoWayEdge
    {
        public Edge EdgeF;
        public Edge EdgeR;
    }

    public class Node
    {
        public bool Visited;
        public TwoWayEdge PathBack;
        public List<TwoWayEdge> Edges = new List<TwoWayEdge>();
    }

    public class Path
    {
        public TwoWayEdge[] Edges;
        public int MinCapacity;
    }

    public static class StockCharts
    {
        private static Path FindPath(Node[] residual)
        {
            //breadth-first search
            var queue = new Queue<int>();
            queue.Enqueue(0);
            Path result = null;
            var touched = new List<int>();
            while (queue.Count > 0)
            {
                var i = queue.Dequeue();
                if (i == residual.Length - 1)
                {
                    // reconstruct path if sink was found
                    var pathEdges = new Stack<TwoWayEdge>();
                    var minCapacity = int.MaxValue;
                    while (i > 0)
                    {
                        pathEdges.Push(residual[i].PathBack);
                        if (residual[i].PathBack.EdgeF.Flow < minCapacity) minCapacity = residual[i].PathBack.EdgeF.Flow;
                        i = residual[i].PathBack.EdgeF.Source;
                    }
                    result = new Path()
                    {
                        Edges = pathEdges.ToArray(),
                        MinCapacity = minCapacity
                    };
                    break;
                }
                for (int j = 0; j < residual[i].Edges.Count; j++)
                {
                    // add children with positive flows that weren't visited yet to queue
                    if (residual[i].Edges[j].EdgeF.Flow <= 0) continue;
                    var next = residual[i].Edges[j].EdgeF.Destination;
                    if (residual[next].Visited) continue;
                    touched.Add(next);
                    residual[next].Visited = true;
                    residual[next].PathBack = residual[i].Edges[j];
                    queue.Enqueue(next);
                }
            }

            // restore after search
            foreach (var t in touched)
            {
                residual[t].Visited = false;
                residual[t].PathBack = null;
            }
            return result;
        }

        private static bool Compare(int[] chart1, int[] chart2)
        {
            for (int i = 0; i < chart1.Length; i++)
            {
                if (chart1[i] <= chart2[i]) return false;
            }
            return true;
        }

        public static string Solve(string[] input)
        {
            // initialization
            var nmCount = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var chartCount = nmCount[0];
            var pointCount = nmCount[1];

            // network: source, 1-based flights and crews, sink
            var network = new Node[2 * chartCount + 2];

            //source
            network[0] = new Node()
            {
                Edges = new List<TwoWayEdge>(chartCount)
            };
            //sink
            network[2 * chartCount + 1] = new Node()
            {
                Edges = new List<TwoWayEdge>(chartCount)
            };

            for (int i = 1; i <= chartCount; i++)
            {
                // source to U
                var uedgeF = new Edge() { Destination = i, Source = 0, Flow = 1 };
                var uedgeR = new Edge() { Destination = 0, Source = i, Flow = 0 };
                network[i] = new Node();
                network[0].Edges.Add(new TwoWayEdge() { EdgeF = uedgeF, EdgeR = uedgeR });
                network[i].Edges.Add(new TwoWayEdge() { EdgeF = uedgeR, EdgeR = uedgeF });

                // V to sink
                var vedgeF = new Edge() { Destination = 2 * chartCount + 1, Source = chartCount + i, Flow = 1 };
                var vedgeR = new Edge() { Destination = chartCount + i, Source = 2 * chartCount + 1, Flow = 0 };
                network[chartCount + i] = new Node();
                network[chartCount + i].Edges.Add(new TwoWayEdge() { EdgeF = vedgeF, EdgeR = vedgeR });
                network[2 * chartCount + 1].Edges.Add(new TwoWayEdge() { EdgeF = vedgeR, EdgeR = vedgeF });
            }

            // analyzing charts
            int[][] charts = new int[chartCount + 1][];

            charts[1] = input[1].Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int i = 1; i <= chartCount; i++)
            {
                for (int j = i + 1; j <= chartCount; j++)
                {
                    if (charts[j] == null)
                        charts[j] = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                    var idx1 = j;
                    var idx2 = i;
                    if (!Compare(charts[i], charts[j]))
                    {
                        if (!Compare(charts[j], charts[i])) continue;
                        idx1 = i;
                        idx2 = j;
                    }
                    // add edge i->j if chart[i] above chart[j]
                    var edgeF = new Edge() { Destination = chartCount + idx1, Source = idx2, Flow = 1 };
                    var edgeR = new Edge() { Destination = idx2, Source = chartCount + idx1, Flow = 0 };
                    network[idx2].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
                    network[chartCount + idx1].Edges.Add(new TwoWayEdge() { EdgeF = edgeR, EdgeR = edgeF });
                }
            }

            // Edmonds-Karp algorithm
            while (true)
            {
                var path = FindPath(network);
                if (path == null) break;
                for (int i = 0; i < path.Edges.Length; i++)
                {
                    path.Edges[i].EdgeF.Flow -= path.MinCapacity;
                    path.Edges[i].EdgeR.Flow += path.MinCapacity;
                }
            }

            //reconstruct pairings
            var matchings = 0;
            for (int i = 1; i <= chartCount; i++)
            {
                for (int j = 1; j < network[i].Edges.Count; j++)
                {
                    if (network[i].Edges[j].EdgeF.Flow == 0)
                    {
                        matchings++;
                    }
                }
            }

            //overlaid charts needed = total chart count - max matchings found
            return (chartCount - matchings).ToString();
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var nmCount = nmline.Split(' ').Select(x => int.Parse(x)).ToArray();
            var input = new string[nmCount[0] + 1];
            input[0] = nmline;
            for (int i = 1; i <= nmCount[0]; i++)
                input[i] = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
