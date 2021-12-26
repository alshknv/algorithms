using System.Collections.Generic;
using System;
using System.Linq;

namespace _3_k_universal_circular_string
{
    public class Vertex
    {
        public List<Edge> Edges = new List<Edge>();
        public int VisitedCount;
        public bool AllVisited
        {
            get
            {
                return VisitedCount == Edges.Count;
            }
        }
        public void AddEdge(int destination)
        {
            Edges.Add(new Edge(destination));
        }
    }

    public class Edge
    {
        public int Destination;
        public bool Visited;
        public Edge(int destination)
        {
            Destination = destination;
        }
    }

    public static class KUniversalCircularString
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
                        graph[v].VisitedCount++;
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

        private static List<int> GetEulerianCycle(int cycleLength)
        {
            // find first cycle
            var cycle = FindCycle(0);

            // continue while cycle length is less than total number of edges
            while (cycle.Count < cycleLength)
            {
                for (int i = 0; i < cycle.Count; i++)
                {
                    if (graph[cycle[i]].AllVisited) continue;
                    for (int j = 0; j < graph[cycle[i]].Edges.Count; j++)
                    {
                        if (!graph[cycle[i]].Edges[j].Visited)
                        {
                            // unvisited edge found
                            var newCycle = cycle.Take(i).ToList();
                            // insert new cycle in the middle of the old one and check for unvisited edges again
                            var nextCycle = FindCycle(cycle[i]);
                            newCycle.AddRange(nextCycle);
                            newCycle.AddRange(cycle.Skip(i).Take(cycle.Count - i));
                            cycle = newCycle;
                            break;
                        }
                    }
                }
            }
            return cycle;
        }

        public static string Solve(int k)
        {
            graph = new Vertex[(int)Math.Pow(2, k - 1)]; // vertices are k-1 mers
            for (int i = 0; i < graph.Length; i++)
            {
                graph[i] = new Vertex();
            }

            var length = (int)Math.Pow(2, k);
            var lastMer = length - 1;
            var mask1 = lastMer - 1;
            var mask2 = lastMer >> 1;
            for (int j = 0; j < length; j++)
            {
                var from = (j & mask1) >> 1;
                var to = j & mask2;
                graph[from].AddEdge(to);
            }

            // find Eulerian cycle
            var cycle = GetEulerianCycle(length);

            // compose circular string
            var circularStringArray = new string[cycle.Count];
            for (int c = 0; c < cycle.Count; c++)
            {
                circularStringArray[c] = (cycle[c] & 1).ToString();
            }
            return string.Concat(circularStringArray);
        }

        static void Main(string[] args)
        {
            int k = int.Parse(Console.ReadLine());
            Console.WriteLine(Solve(k));
        }
    }
}
