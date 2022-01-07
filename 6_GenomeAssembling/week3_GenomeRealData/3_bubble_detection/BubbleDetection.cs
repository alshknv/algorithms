using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_bubble_detection
{
    public class Vertex
    {
        public int Index;
        public List<Edge> IncomingEdges = new List<Edge>();
        public List<Edge> OutgoingEdges = new List<Edge>();
        public Vertex(int index)
        {
            Index = index;
        }
        public void AddIncomingEdge(int destination)
        {
            IncomingEdges.Add(new Edge(destination));
        }
        public void AddOutgoingEdge(int destination)
        {
            OutgoingEdges.Add(new Edge(destination));
        }
    }

    public class Edge
    {
        public int Connection;
        //public bool Visited;
        public Edge(int connection)
        {
            Connection = connection;
        }
    }

    public class Graph
    {
        private readonly List<string> mers = new List<string>();
        public Dictionary<string, Vertex> Vertices = new Dictionary<string, Vertex>();
        public Vertex this[int index]
        {
            get
            {
                return Vertices[mers[index]];
            }
            set
            {
                Vertices[mers[index]] = value;
            }
        }

        public int VertexCount
        {
            get { return Vertices.Count; }
        }

        public int Vertex(string mer)
        {
            Vertex vertex;
            if (!mers.Contains(mer))
            {
                vertex = new Vertex(mers.Count);
                mers.Add(mer);
                Vertices.Add(mer, vertex);
            }
            else
            {
                vertex = Vertices[mer];
            }
            return vertex.Index;
        }
    }

    public static class BubbleDetection
    {
        //private static Graph graph;
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

        private static void DFS(Vertex[] graph, int start, int[] targets, int v, int maxdepth, List<int> path, Dictionary<Tuple<int, int>, List<List<int>>> bubbleCandidates)
        {
            if (v != start && targets.Contains(v))
            {
                // vertex with multiple incoming edges reached
                var tuple = new Tuple<int, int>(start, v);
                if (bubbleCandidates.ContainsKey(tuple))
                    bubbleCandidates[tuple].Add(path);
                else
                    bubbleCandidates.Add(tuple, new List<List<int>>() { path });
            }
            if (path.Count > maxdepth) return;
            foreach (var e in graph[v].OutgoingEdges)
            {
                var newPath = new List<int>(path)
                {
                    e.Connection
                };
                DFS(graph, start, targets, e.Connection, maxdepth, newPath, bubbleCandidates);
            }
        }

        private static int FindBubbles(string[] kmers, int t)
        {
            var graph = new Graph();

            // construct De Bruijn graph
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddOutgoingEdge(to);
                graph[to].AddIncomingEdge(from);
            }

            var multipleIn = new List<int>();
            var multipleOut = new List<int>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (graph[i].IncomingEdges.Count > 1) multipleIn.Add(i);
                if (graph[i].OutgoingEdges.Count > 1) multipleOut.Add(i);
            }

            // for each pair of mult-out & mult-in nodes find all paths between them shorter than threshold
            var bubbleCandidates = new Dictionary<Tuple<int, int>, List<List<int>>>();
            var vertices = graph.Vertices.Select(x => x.Value).ToArray();
            for (int i = 0; i < multipleOut.Count; i++)
            {
                DFS(vertices, multipleOut[i], multipleIn.ToArray(), multipleOut[i], t, new List<int>() { multipleOut[i] }, bubbleCandidates);
            }

            // find all pairs of non-overlapping disjoint paths
            var bubbleCount = 0;
            foreach (var pair in bubbleCandidates.Keys.ToArray())
            {
                for (int i = 0; i < bubbleCandidates[pair].Count; i++)
                {
                    for (int j = i + 1; j < bubbleCandidates[pair].Count; j++)
                    {
                        var path1 = bubbleCandidates[pair][i];
                        var path2 = bubbleCandidates[pair][j];
                        if (path1.GroupBy(x => x).Count() < path1.Count) continue;
                        if (path2.GroupBy(x => x).Count() < path2.Count) continue;
                        if (!path1.Skip(1).Take(bubbleCandidates[pair][i].Count - 2).Intersect(path2.Skip(1).Take(bubbleCandidates[pair][j].Count - 2)).Any())
                        {
                            bubbleCount++;
                        }
                    }
                }
            }
            return bubbleCount;
        }

        public static string Solve(string[] input)
        {
            var kt = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var kmers = GetKMers(input.Skip(1).ToArray(), kt[0]).ToArray();
            var bubbles = FindBubbles(kmers, kt[1]);
            return bubbles.ToString();
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
