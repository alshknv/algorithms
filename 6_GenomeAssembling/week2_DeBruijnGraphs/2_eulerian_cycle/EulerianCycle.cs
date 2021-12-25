using System.Linq;
using System.Collections.Generic;
using System;

namespace _2_eulerian_cycle
{
    public class Vertex
    {
        public List<Edge> Edges = new List<Edge>();
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
            var cycle = new List<int>() { start };
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
                            cycle.Add(v);
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
            graph = new Vertex[nm[0] + 1];
            for (int i = 1; i <= nm[0]; i++) graph[i] = new Vertex();
            for (int j = 1; j <= nm[1]; j++)
            {
                var edge = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                graph[edge[0]].Edges.Add(new Edge(edge[1]));
            }

            // find first cycle
            var cycle = FindCycle(1);
            if (cycle == null) return new string[] { "0" };

            // continue while cycle length is less than total number of edges
            while (cycle.Count < nm[1])
            {
                for (int i = 0; i < cycle.Count; i++)
                {
                    for (int j = 0; j < graph[cycle[i]].Edges.Count; j++)
                    {
                        if (!graph[cycle[i]].Edges[j].Visited)
                        {
                            // unvisited edge found
                            var newCycle = cycle.Take(i).ToList();
                            if (newCycle == null)
                            {
                                // no eulerian cycle 
                                return new string[] { "0" };
                            }
                            // insert new cycle in the middle of the old one and check for unvisited edges again
                            var nextCycle = FindCycle(cycle[i]);
                            if (nextCycle == null) return new string[] { "0" };
                            newCycle.AddRange(nextCycle);
                            newCycle.AddRange(cycle.Skip(i).Take(cycle.Count - i));
                            cycle = newCycle;
                            break;
                        }
                    }
                }
            }
            return new string[] { "1", string.Join(" ", cycle) };
        }

        static void Main(string[] args)
        {
            var gg = Solve(new string[] { "3 4", "2 3", "2 2", "1 2", "3 1" });
            return;
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
