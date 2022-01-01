using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_bubble_detection
{
    public class Vertex
    {
        public int Index;
        public bool Visited;
        public int Depth;
        public List<int> Splits;
        public List<Edge> Edges = new List<Edge>();
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

    public class QueueItem
    {
        public int Index;
        public int Depth;
        public List<int> Splits = new List<int>();
    }

    public static class BubbleDetection
    {
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

        public static string Solve(string[] input)
        {
            var kt = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var kmers = GetKMers(input.Skip(1).ToArray(), kt[0]).ToArray();

            // construct De Bruijn graph
            var graph = new Graph();
            var edgeCount = 0;
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddEdge(to);
                edgeCount++;
            }

            //breadth-first bubble BubbleDetection
            var queue = new Queue<QueueItem>();
            var bubbleCount = 0;
            queue.Enqueue(new QueueItem());
            while (queue.Count > 0)
            {
                var v = queue.Dequeue();
                if (graph[v.Index].Visited)
                {
                    var intersect = v.Splits.Intersect(graph[v.Index].Splits).ToArray();
                    for (int i = 0; i < intersect.Length; i++)
                    {
                        if (v.Depth - intersect[i] <= kt[1]) bubbleCount++;
                    }
                }
                else
                {
                    graph[v.Index].Visited = true;
                    graph[v.Index].Splits = v.Splits;
                    if (graph[v.Index].Edges.Count > 1) v.Splits.Add(v.Depth);
                    for (int i = 0; i < graph[v.Index].Edges.Count; i++)
                    {
                        if (graph[v.Index].Edges[i].Destination > 0)
                            queue.Enqueue(new QueueItem() { Index = graph[v.Index].Edges[i].Destination, Depth = v.Depth + 1, Splits = graph[v.Index].Splits });
                    }
                }
            }
            return bubbleCount.ToString();
        }

        static void Main(string[] args)
        {
            var gg = Solve(new string[] { "3 3", "AACG", "AAGG", "ACGT", "AGGT", "CGTT", "GCAA", "GGTT", "GTTG", "TGCA", "TTGC" });
            return;
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
