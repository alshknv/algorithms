﻿using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_bubble_detection
{
    public class Vertex
    {
        public int Index;
        public List<Edge> Edges = new List<Edge>();
        public int VisitedCount;
        public bool AllVisited
        {
            get
            {
                return VisitedCount == Edges.Count;
            }
        }
        public Vertex(int index)
        {
            Index = index;
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

    public class Graph
    {
        private readonly List<string> mers = new List<string>();
        private readonly Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
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

    public static class BubbleDetection
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

        private static bool FindCycle(int start, out List<int> cycle)
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
        }

        public static int CountBubbles(string[] kmers, int t)
        {
            graph = new Graph();

            // construct De Bruijn graph
            var edgeCount = 0;
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddEdge(to);
                edgeCount++;
            }

            var bubbleCount = 0;
            // find first cycle
            List<int> cycle;
            if (!FindCycle(0, out cycle)) return bubbleCount;

            // continue while cycle length is less than total number of edges

            while (cycle.Count < edgeCount)
            {
                var hasUnexplored = false;
                for (int i = 0; i < cycle.Count; i++)
                {
                    if (graph[cycle[i]].AllVisited) continue;
                    for (int j = 0; j < graph[cycle[i]].Edges.Count; j++)
                    {
                        if (!graph[cycle[i]].Edges[j].Visited)
                        {
                            // unvisited edge found
                            hasUnexplored = true;
                            List<int> nextCycle;
                            if (FindCycle(cycle[i], out nextCycle))
                            {
                                // insert new cycle in the middle of the old one and check for unvisited edges again
                                var newCycle = cycle.Take(i).ToList();
                                newCycle.AddRange(nextCycle);
                                newCycle.AddRange(cycle.Skip(i).Take(cycle.Count - i));
                                cycle = newCycle;
                            }
                            else if (nextCycle.Count - 1 <= t)
                            {
                                bubbleCount++;
                            }
                            break;
                        }
                    }
                }
                if (!hasUnexplored) break;
            }
            return bubbleCount;
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
