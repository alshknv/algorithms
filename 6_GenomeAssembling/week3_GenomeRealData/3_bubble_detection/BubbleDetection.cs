﻿using System.Linq;
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
        public bool Visited;
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

        /*private static bool FindCycle(int start, out List<int> cycle)
        {
            var v = start;
            cycle = new List<int>() { start };
            do
            {
                // continue to go through unvisited edges until we come back to start
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
                if (!edgeFound) return false;
            } while (v != start);
            return true;
        }*/

        private static void DFS(Vertex[] graph, int start, int[] targets, int v, int depth, int maxdepth, List<int> path, Dictionary<Tuple<int, int>, List<List<int>>> bubbleCandidates)
        {
            if (v != start && targets.Contains(v))
            {
                // vertex with multiple incoming edges reached
                var tuple = new Tuple<int, int>(start, v);
                if (bubbleCandidates.ContainsKey(tuple))
                {
                    bubbleCandidates[tuple].Add(path);
                }
                else
                {
                    bubbleCandidates.Add(tuple, new List<List<int>>() { path });
                }
            }
            if (depth >= maxdepth) return;
            foreach (var e in graph[v].OutgoingEdges)
            {
                var newPath = new List<int>(path);
                newPath.Add(e.Connection);
                DFS(graph, start, targets, e.Connection, depth + 1, maxdepth, newPath, bubbleCandidates);
            }
        }

        private static int CountBubbles(string[] kmers, int t)
        {
            var graph = new Graph();

            // construct De Bruijn graph
            //var edgeCount = 0;
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddOutgoingEdge(to);
                graph[to].AddIncomingEdge(from);
                //edgeCount++;
            }

            var multipleIn = new List<int>();
            var multipleOut = new List<int>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (graph[i].IncomingEdges.Count > 1) multipleIn.Add(i);
                if (graph[i].OutgoingEdges.Count > 1) multipleOut.Add(i);
            }

            var bubbleCandidates = new Dictionary<Tuple<int, int>, List<List<int>>>();

            for (int i = 0; i < multipleOut.Count; i++)
            {
                DFS(graph.Vertices.Select(x => x.Value).ToArray(), multipleOut[i], multipleIn.ToArray(), multipleOut[i], 0, t, new List<int>() { multipleOut[i] }, bubbleCandidates);
            }

            return bubbleCandidates.Count(x => x.Value.Count > 1);
        }

        public static string Solve(string[] input)
        {
            var kt = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var kmers = GetKMers(input.Skip(1).ToArray(), kt[0]).ToArray();
            var bubbles = CountBubbles(kmers, kt[1]);
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
