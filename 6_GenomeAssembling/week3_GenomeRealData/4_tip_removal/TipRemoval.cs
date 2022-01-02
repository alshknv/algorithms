using System.Linq;
using System.Collections.Generic;
using System;


namespace _4_tip_removal
{
    public class Vertex
    {
        public int Index;
        public List<Edge> OutgoingEdges = new List<Edge>();
        public List<Edge> IncomingEdges = new List<Edge>();
        public int VisitedCount;
        public bool AllOutgoingVisited
        {
            get
            {
                return VisitedCount == OutgoingEdges.Count;
            }
        }
        public Vertex(int index)
        {
            Index = index;
        }
        public void AddOutgoingEdge(int destination)
        {
            OutgoingEdges.Add(new Edge(destination));
        }
        public void AddIncomingEdge(int destination)
        {
            IncomingEdges.Add(new Edge(destination));
        }
    }

    public class Edge
    {
        public int Connection;
        public bool Visited;
        public Edge(int connection)
        {
            Connection = connection;
        }
    }

    public class Graph
    {
        private readonly List<string> mers = new List<string>();
        private readonly Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
        public int VertexCount
        {
            get { return vertices.Keys.Count; }
        }

        public Vertex this[int index]
        {
            get
            {
                return vertices[mers[index]];
            }
            set
            {
                vertices[mers[index]] = value;
            }
        }

        public int Vertex(string mer)
        {
            Vertex vertex;
            if (!mers.Contains(mer))
            {
                vertex = new Vertex(mers.Count);
                mers.Add(mer);
                vertices.Add(mer, vertex);
            }
            else
            {
                vertex = vertices[mer];
            }
            return vertex.Index;
        }
    }

    public static class TipRemoval
    {
        private static Graph graph;
        private static HashSet<string> GetKMers(string[] reads, int k)
        {
            var kmers = new HashSet<string>();
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = 0; j <= reads[i].Length - k; j++)
                {
                    kmers.Add(reads[i].Substring(j, k));
                }
            }
            return kmers;
        }

        private static int RemoveSourceTip(int v)
        {
            var tipNum = 0;
            if (graph[v].IncomingEdges.Count < 2)
            {
                for (int j = 0; j < graph[v].OutgoingEdges.Count; j++)
                {
                    if (graph[v].OutgoingEdges[j].Visited) continue;
                    tipNum += RemoveSourceTip(graph[v].OutgoingEdges[j].Connection) + 1;
                    graph[v].OutgoingEdges[j].Visited = true;
                    for (int k = 0; k < graph[graph[v].OutgoingEdges[j].Connection].IncomingEdges.Count; k++)
                    {
                        if (graph[graph[v].OutgoingEdges[j].Connection].IncomingEdges[k].Connection == v)
                            graph[graph[v].OutgoingEdges[j].Connection].IncomingEdges[k].Visited = true;
                    }
                }
            }
            return tipNum;
        }

        private static int RemoveTargetTip(int v)
        {
            var tipNum = 0;
            if (graph[v].OutgoingEdges.Count < 2)
            {
                for (int j = 0; j < graph[v].IncomingEdges.Count; j++)
                {
                    if (graph[v].IncomingEdges[j].Visited) continue;
                    tipNum += RemoveTargetTip(graph[v].IncomingEdges[j].Connection) + 1;
                    graph[v].IncomingEdges[j].Visited = true;
                    for (int k = 0; k < graph[graph[v].IncomingEdges[j].Connection].IncomingEdges.Count; k++)
                    {
                        if (graph[graph[v].IncomingEdges[j].Connection].OutgoingEdges[k].Connection == v)
                            graph[graph[v].IncomingEdges[j].Connection].OutgoingEdges[k].Visited = true;
                    }
                }
            }
            return tipNum;
        }

        private static int ConstructGraph(string[] kmers)
        {
            graph = new Graph();

            // construct De Bruijn graph
            var edgeCount = 0;
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddOutgoingEdge(to);
                graph[to].AddIncomingEdge(from);
                edgeCount++;
            }
            return edgeCount;
        }

        private static int RemoveTips()
        {
            var tipCount = 0;
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (graph[i].IncomingEdges.Count == 0)
                {
                    // source tip
                    tipCount += RemoveSourceTip(i);
                }
                if (graph[i].OutgoingEdges.Count == 0)
                {
                    // target tip
                    tipCount += RemoveTargetTip(i);
                }
            }

            return tipCount;
        }

        private static List<int> FindCycle(int start)
        {
            var v = start;
            var cycle = new List<int>() { start };
            do
            {
                // continue to go through unvisited edges until we come back to start
                var edgeFound = false;
                for (int e = 0; e < graph[v].OutgoingEdges.Count; e++)
                {
                    if (!graph[v].OutgoingEdges[e].Visited)
                    {
                        graph[v].OutgoingEdges[e].Visited = true;
                        graph[v].VisitedCount++;
                        v = graph[v].OutgoingEdges[e].Connection;
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

        private static bool Exists(int edgeCount)
        {
            // find first cycle
            var cycle = FindCycle(0);
            if (cycle == null) return false;

            // continue while cycle length is less than total number of edges
            while (true)
            {
                var cycleExtended = false;
                for (int i = 0; i < cycle.Count; i++)
                {
                    if (graph[cycle[i]].AllOutgoingVisited) continue;
                    for (int j = 0; j < graph[cycle[i]].OutgoingEdges.Count; j++)
                    {
                        if (!graph[cycle[i]].OutgoingEdges[j].Visited)
                        {
                            cycleExtended = true;
                            // unvisited edge found
                            var newCycle = cycle.Take(i).ToList();
                            // insert new cycle in the middle of the old one and check for unvisited edges again
                            var nextCycle = FindCycle(cycle[i]);
                            if (nextCycle == null) return false;
                            newCycle.AddRange(nextCycle);
                            newCycle.AddRange(cycle.Skip(i).Take(cycle.Count - i));
                            cycle = newCycle;
                            break;
                        }
                    }
                }
                if (!cycleExtended) return cycle.Count == edgeCount;
            }
        }

        public static string Solve(string[] input)
        {
            for (int k = input[0].Length; k >= 1; k--)
            {
                var kmers = GetKMers(input.Skip(1).ToArray(), k).ToArray();
                var edges = ConstructGraph(kmers);
                var tips = RemoveTips();
                // check if eulerian graph exists
                if (Exists(edges - tips))
                    return tips.ToString();
            }
            return "0";
        }

        static void Main(string[] args)
        {
            var input = new List<string>();
            string inLine;
            while ((inLine = Console.ReadLine()) != null)
            {
                input.Add(inLine);
            }
            Console.WriteLine(Solve(input.ToArray()));
        }
    }
}
