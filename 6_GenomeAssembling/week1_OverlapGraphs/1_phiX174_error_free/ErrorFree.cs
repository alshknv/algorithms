using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_phiX174_error_free
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

    public static class ErrorFree
    {
        private static int FindLongestOverlap(string s1, string s2)
        {
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] == s2[0])
                {
                    var k = 1;
                    while (i + k < s1.Length && k < s2.Length && s1[i + k] == s2[k]) k++;
                    if (i + k == s1.Length) return k;
                }
            }
            return 0;
        }

        public static string Assemble(string[] reads)
        {
            var overlapGraph = new Vertex[reads.Length];

            // build overlap graph
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = i + 1; j < reads.Length; j++)
                {
                    var overlapIJ = FindLongestOverlap(reads[i], reads[j]);
                    if (overlapGraph[i] == null) overlapGraph[i] = new Vertex(i, reads[i]);
                    if (overlapIJ > 0)
                        overlapGraph[i].AddEdge(overlapIJ, j);
                    var overlapJI = FindLongestOverlap(reads[j], reads[i]);
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

            var genomeArray = new string[overlapGraph.Length];
            genomeArray[0] = path[0].Read;
            for (int i = 1; i < overlapGraph.Length; i++)
            {
                genomeArray[i - 1] = genomeArray[i - 1].Substring(0, genomeArray[i - 1].Length - overlaps[i - 1]);
                genomeArray[i] = path[i].Read;
            }
            var genome = string.Concat(genomeArray);
            var cycleOverlap = FindLongestOverlap(genome, path[0].Read);
            if (cycleOverlap > 0)
                genome = genome.Substring(0, genome.Length - cycleOverlap);
            return genome;
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
