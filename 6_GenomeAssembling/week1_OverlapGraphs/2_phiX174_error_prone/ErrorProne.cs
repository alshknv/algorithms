using System.Collections.Generic;
using System;
using System.Linq;

namespace _2_phiX174_error_prone
{
    public class Edge
    {
        public int Destination;
        public int Overlap;
    }

    public class Vertex
    {
        public string Read;
        public bool Visited;
        public int Id;
        public LinkedList<Edge> Edges = new LinkedList<Edge>();
        public LinkedListNode<Edge> CurrentEdge;

        public Vertex(int id, string read)
        {
            Id = id;
            Read = read;
        }
        public void AddEdge(int overlap, int destination)
        {
            var edge = Edges.First;
            var newEdge = new LinkedListNode<Edge>(new Edge() { Overlap = overlap, Destination = destination });
            if (edge == null)
            {
                Edges.AddFirst(newEdge);
            }
            else
            {
                while (edge != null && edge.Value.Overlap > overlap) edge = edge.Next;
                if (edge == null)
                {
                    Edges.AddLast(newEdge);
                }
                else
                {
                    Edges.AddBefore(edge, newEdge);
                }
            }
        }
    }

    public static class ErrorProne
    {
        private static int FindLongestOverlap(string s1, string s2, int error)
        {
            var matrix = new int[s1.Length, s2.Length];
            for (int i = 0; i < s1.Length; i++)
            {
                for (int j = 0; j < s2.Length; j++)
                {
                    if (s1[i] == s2[j])
                    {
                        matrix[i, j] = 1;
                    }
                }
            }
            var maxOverlap = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (matrix[i, 0] == 1 || error > 0)
                {
                    var b = 1;
                    var e = matrix[i, 0] == 1 ? 0 : 1;
                    while (i + b < s1.Length && b < s2.Length && (matrix[i + b, b] == 1 || e < error))
                    {
                        if (matrix[i + b, b] == 0) e++;
                        b++;
                    }
                    if (i + b == s1.Length && b > maxOverlap) maxOverlap = b;
                }
            }
            return maxOverlap;
        }

        public static string Assemble(string[] reads)
        {
            var overlapGraph = new Vertex[reads.Length];

            // build overlap graph
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = i + 1; j < reads.Length; j++)
                {
                    var overlapIJ = FindLongestOverlap(reads[i], reads[j], 1);
                    if (overlapGraph[i] == null) overlapGraph[i] = new Vertex(i, reads[i]);
                    if (overlapIJ > 0)
                        overlapGraph[i].AddEdge(overlapIJ, j);
                    var overlapJI = FindLongestOverlap(reads[j], reads[i], 1);
                    if (overlapGraph[j] == null) overlapGraph[j] = new Vertex(j, reads[j]);
                    if (overlapJI > 0)
                        overlapGraph[j].AddEdge(overlapJI, i);
                }
            }

            for (int i = 0; i < overlapGraph.Length; i++)
            {
                overlapGraph[i].CurrentEdge = overlapGraph[i].Edges.First;
            }

            // greedy hamiltonian path
            var path = new List<Vertex>(overlapGraph.Length) { overlapGraph[0] };
            var overlaps = new List<int>(overlapGraph.Length);
            do
            {
                var vertex = path[path.Count - 1];
                vertex.Visited = true;
                while (vertex.CurrentEdge != null && overlapGraph[vertex.CurrentEdge.Value.Destination].Visited)
                    vertex.CurrentEdge = vertex.CurrentEdge.Next;
                if (vertex.CurrentEdge == null)
                {
                    overlapGraph[vertex.Id].Visited = false;
                    path.RemoveAt(path.Count - 1);
                    if (path.Count > 0)
                    {
                        path[path.Count - 1].CurrentEdge = path[path.Count - 1].CurrentEdge.Next;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    path.Add(overlapGraph[vertex.CurrentEdge.Value.Destination]);
                    overlaps.Add(vertex.CurrentEdge.Value.Overlap);
                    if (path.Count == overlapGraph.Length) break;
                }
            } while (true);

            var genome = new string[overlapGraph.Length];
            genome[0] = path[0].Read;
            for (int i = 1; i < overlapGraph.Length; i++)
            {
                genome[i] = path[i].Read.Substring(overlaps[i - 1]);
            }
            var lastVertex = overlapGraph[path[path.Count - 1].Id];
            while (lastVertex.CurrentEdge?.Value.Destination != 0) lastVertex.CurrentEdge = lastVertex.CurrentEdge.Next;
            var cycleOverlap = lastVertex.CurrentEdge?.Value.Overlap ?? 0;
            genome[genome.Length - 1] = genome[genome.Length - 1].Substring(0, cycleOverlap / 2);
            genome[0] = genome[0].Substring(cycleOverlap - cycleOverlap / 2);
            return string.Concat(genome);
        }

        static void Main(string[] args)
        {
            var reads = new string[1618];
            for (int i = 0; i < 1618; i++)
                reads[i] = Console.ReadLine();
            Console.WriteLine(Assemble(reads));
        }
    }
}
