using System.Linq;
using System.Collections.Generic;
using System;


namespace _4_tip_removal
{
    public class Vertex
    {
        public int Index;
        public bool Deleted;
        public List<Edge> OutgoingEdges = new List<Edge>();
        public List<Edge> IncomingEdges = new List<Edge>();

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
                graph[v].Deleted = true;
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
                graph[v].Deleted = true;
                for (int j = 0; j < graph[v].IncomingEdges.Count; j++)
                {
                    if (graph[v].IncomingEdges[j].Visited) continue;
                    tipNum += RemoveTargetTip(graph[v].IncomingEdges[j].Connection) + 1;
                    graph[v].IncomingEdges[j].Visited = true;
                    for (int k = 0; k < graph[graph[v].IncomingEdges[j].Connection].OutgoingEdges.Count; k++)
                    {
                        if (graph[graph[v].IncomingEdges[j].Connection].OutgoingEdges[k].Connection == v)
                            graph[graph[v].IncomingEdges[j].Connection].OutgoingEdges[k].Visited = true;
                    }
                }
            }
            return tipNum;
        }

        private static void ConstructGraph(string[] kmers)
        {
            graph = new Graph();

            // construct De Bruijn graph
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddOutgoingEdge(to);
                graph[to].AddIncomingEdge(from);
            }
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

        private static bool EulerianGraphExists()
        {
            var vertexCount = 0;
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (graph[i].Deleted) continue;
                vertexCount++;
                var incoming = graph[i].IncomingEdges.Count(x => !x.Visited);
                var outgoing = graph[i].OutgoingEdges.Count(x => !x.Visited);
                if (incoming == 0 || outgoing == 0 || incoming != outgoing)
                    return false;
            }
            return vertexCount > 0;
        }

        public static string Solve(string[] input)
        {
            var kmers = GetKMers(input.Skip(1).ToArray(), input.Length >= 15 ? 15 : 3).ToArray();
            ConstructGraph(kmers);
            var tips = RemoveTips();
            // check if eulerian graph is possible
            if (EulerianGraphExists())
                return tips.ToString();
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
