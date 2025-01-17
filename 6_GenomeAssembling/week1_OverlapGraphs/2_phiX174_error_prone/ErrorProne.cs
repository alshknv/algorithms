﻿using System.Collections.Generic;
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
                var count = 0;
                while (edge != null && count++ < 5 && edge.Value.Overlap >= overlap)
                    edge = edge.Next;
                if (edge == null)
                {
                    Edges.AddLast(newEdge);
                }
                else if (count < 5)
                {
                    Edges.AddBefore(edge, newEdge);
                }
            }
        }
    }

    public static class ErrorProne
    {
        private static int FindLongestOverlap(string s1, string s2, int errors)
        {
            for (int i = 0; i < s1.Length - 12; i++)
            {
                if (s1[i] == s2[0] || errors > 0)
                {
                    var k = 1;
                    var e = s1[i] == s2[0] ? 0 : 1;
                    while (i + k < s1.Length && k < s2.Length && (s1[i + k] == s2[k] || e < errors))
                    {
                        if (s1[i + k] != s2[k]) e++;
                        k++;
                    }
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
                    var overlapIJ = FindLongestOverlap(reads[i], reads[j], 2);
                    if (overlapGraph[i] == null) overlapGraph[i] = new Vertex(i, reads[i]);
                    if (overlapIJ > 0)
                        overlapGraph[i].AddEdge(overlapIJ, j);
                    var overlapJI = FindLongestOverlap(reads[j], reads[i], 2);
                    if (overlapGraph[j] == null) overlapGraph[j] = new Vertex(j, reads[j]);
                    if (overlapJI > 0)
                        overlapGraph[j].AddEdge(overlapJI, i);
                }
            }

            for (int i = 0; i < overlapGraph.Length; i++)
            {
                overlapGraph[i].CurrentEdge = overlapGraph[i].Edges.First;
            }

            // find greedy hamiltonian path
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

            // calculate positions of reads
            var readPositions = new int[overlaps.Count + 1];
            readPositions[0] = 0;
            for (int i = 1; i <= overlaps.Count; i++)
            {
                readPositions[i] += readPositions[i - 1] + (path[i].Read.Length - overlaps[i - 1]);
            }

            // find overlap between end and start and cut the genome string
            var genomeArray = new string[overlapGraph.Length];
            var pathReads = new string[overlapGraph.Length];
            genomeArray[0] = pathReads[0] = path[0].Read;
            for (int i = 1; i < overlapGraph.Length; i++)
            {
                genomeArray[i - 1] = genomeArray[i - 1].Substring(0, genomeArray[i - 1].Length - overlaps[i - 1]);
                pathReads[i] = genomeArray[i] = path[i].Read;
            }
            var genomeString = string.Concat(genomeArray);
            var cycleOverlap = FindLongestOverlap(genomeString, path[0].Read, 5);
            if (cycleOverlap > 0)
                genomeString = genomeString.Substring(0, genomeString.Length - cycleOverlap);

            // replace errors with most popular char at given position among all reads that contains it
            var genome = genomeString.ToCharArray();
            for (int i = 0; i < genome.Length; i++)
            {
                var charStat = new Dictionary<char, int>();
                for (int j = 0; j < pathReads.Length; j++)
                {
                    var end = readPositions[j] + pathReads[j].Length;
                    var start = end <= genome.Length || i >= readPositions[j] ? readPositions[j] : 0;
                    var readShift = end > genome.Length && i < readPositions[j] ? genome.Length - readPositions[j] : 0;
                    if (end > genome.Length && i < readPositions[j]) end -= genome.Length;
                    if (i >= start && i < end)
                    {
                        var readChar = pathReads[j][readShift + (i - start)];
                        if (!charStat.ContainsKey(readChar))
                        {
                            charStat.Add(readChar, 1);
                        }
                        else
                        {
                            charStat[readChar]++;
                        }
                    }
                }
                var correctChar = charStat.OrderByDescending(x => x.Value).First().Key;
                if (genome[i] != correctChar)
                {
                    genome[i] = correctChar;
                }
            }
            return new string(genome);
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
