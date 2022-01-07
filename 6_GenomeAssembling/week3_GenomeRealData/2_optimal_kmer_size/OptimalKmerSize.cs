using System.Collections.Generic;
using System;
using System.Linq;

namespace _2_optimal_kmer_size
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
        public Edge(int connection)
        {
            Connection = connection;
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

        public int VertexCount
        {
            get { return vertices.Count; }
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

    public static class OptimalKmerSize
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

        public static string Solve(string[] reads)
        {
            var min = 1;
            var max = reads[0].Length;
            var k = 0;
            while (max > min)
            {
                var mid = (int)Math.Floor(((double)max + min) / 2);
                var kmers = GetKMers(reads, mid).ToArray();
                var graph = new Graph();

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

                // check if graph is eulerian, i.e. number of incoming edges is equal to number of outgoing edges at every vertex
                var eulerian = true;
                for (int i = 0; i < graph.VertexCount; i++)
                {
                    if (graph[i].IncomingEdges.Count != graph[i].OutgoingEdges.Count)
                    {
                        eulerian = false;
                        break;
                    }
                }

                if (eulerian)
                {
                    k = mid;
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return k.ToString();
        }

        static void Main(string[] args)
        {
            var reads = new string[400];
            for (int i = 0; i < reads.Length; i++)
            {
                reads[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(reads));
        }
    }
}
