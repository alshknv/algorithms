using System.Linq;
using System.Collections.Generic;
using System;

namespace _2_eulerian_cycle
{
    public class Vertex
    {
        public int Id;
        public List<Edge> Edges = new List<Edge>();
        public Vertex(int id)
        {
            Id = id;
        }
    }

    public class Edge
    {
        public int Destination;
        public bool Visited;
        public Edge(int d)
        {
            Destination = d;
        }
    }

    public static class EulerianCycle
    {
        private static Vertex[] graph;

        private static List<int> FindCycle(int start)
        {
            var v = start;
            var cycle = new List<int>() { graph[start].Id };
            do
            {
                var edgeFound = false;
                for (int e = 0; e < graph[v].Edges.Count; e++)
                {
                    if (!graph[v].Edges[e].Visited)
                    {
                        graph[v].Edges[e].Visited = true;
                        v = graph[v].Edges[e].Destination;
                        if (v != start)
                            cycle.Add(graph[v].Id);
                        edgeFound = true;
                        break;
                    }
                }
                if (!edgeFound) return null;
            } while (v != start);
            return cycle;
        }

        public static string[] Solve(string[] input)
        {
            // construct graph
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            graph = new Vertex[nm[0]];
            for (int i = 0; i < nm[0]; i++) graph[i] = new Vertex(i + 1);
            for (int j = 1; j <= nm[1]; j++)
            {
                var edge = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                graph[edge[0] - 1].Edges.Add(new Edge(edge[1] - 1));
            }

            // find cycle
            int v;
            int e;
            var cycles = new List<Tuple<int, List<int>>>();
            for (v = 0; v < graph.Length; v++)
            {
                for (e = 0; e < graph[v].Edges.Count; e++)
                {
                    if (!graph[v].Edges[e].Visited)
                    {
                        // unvisited vertex found
                        var cycle = new Tuple<int, List<int>>(graph[v].Id, FindCycle(v));
                        if (cycle == null)
                        {
                            return new string[] { "0" };
                        }
                        cycles.Add(cycle);
                    }
                }
            }
            var nextCycle = 1;
            var fullCycle = new List<int>();
            for (var c = 0; c < cycles[0].Item2.Count; c++)
            {
                if (nextCycle < cycles.Count && cycles[0].Item2[c] == cycles[nextCycle].Item1)
                {
                    // insert cycle
                    for (var inc = 0; inc < cycles[nextCycle].Item2.Count; inc++)
                    {
                        fullCycle.Add(cycles[nextCycle].Item2[inc]);
                    }
                    nextCycle++;
                }
                fullCycle.Add(cycles[0].Item2[c]);
            }
            return new string[] { "1", string.Join(" ", fullCycle) };
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var m = int.Parse(nmline.Split(' ')[1]);
            var input = new string[m + 1];
            input[0] = nmline;
            for (int i = 1; i <= m; i++)
            {
                input[i] = Console.ReadLine();
            }
            foreach (var line in Solve(input))
            {
                Console.WriteLine(line);
            }
        }
    }
}
